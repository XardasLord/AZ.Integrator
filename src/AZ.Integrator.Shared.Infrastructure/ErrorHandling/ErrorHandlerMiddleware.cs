using System.Net;
using System.Text.Json;
using AZ.Integrator.Domain.SharedKernel.Exceptions;
using AZ.Integrator.Invoices.Application.Common.Exceptions;
using AZ.Integrator.Orders.Application.Common.Exceptions;
using AZ.Integrator.Shared.Infrastructure.Exceptions;
using AZ.Integrator.Shipments.Application.Common.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace AZ.Integrator.Shared.Infrastructure.ErrorHandling;

public class ErrorHandlerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ErrorHandlerMiddleware> _logger;
    
    public ErrorHandlerMiddleware(RequestDelegate next, ILogger<ErrorHandlerMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }
    
    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Application error");
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception ex)
    {
        var response = ex switch
        {
            DomainException exception => new ExceptionResponse(exception.Code, exception.Message,
                HttpStatusCode.BadRequest, ex.InnerException?.Message),
            ShipmentsModuleApplicationException exception => new ExceptionResponse(exception.Code, exception.Message,
                HttpStatusCode.BadRequest, ex.InnerException?.Message),
            OrdersModuleApplicationException exception => new ExceptionResponse(exception.Code, exception.Message,
                HttpStatusCode.BadRequest, ex.InnerException?.Message),
            InvoicesModuleApplicationException exception => new ExceptionResponse(exception.Code, exception.Message,
                HttpStatusCode.BadRequest, ex.InnerException?.Message),
            InfrastructureException exception => new ExceptionResponse(exception.Code, exception.Message,
                HttpStatusCode.BadRequest, ex.InnerException?.Message),
            HttpRequestException exception => new ExceptionResponse("http_request_error", exception.Message, exception.StatusCode ?? HttpStatusCode.BadRequest, exception.InnerException?.Message),
            _ => new ExceptionResponse("unexpected_error", ex.Message, HttpStatusCode.InternalServerError, ex.InnerException?.Message)
        };

        var result = JsonSerializer.Serialize(response);
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)response.HttpStatusCode;

        return context.Response.WriteAsync(result);
    }

    private class ExceptionResponse
    {
        public string Code { get; }
        public string Message { get; }
        public string AdditionalMessage { get; }
        public HttpStatusCode HttpStatusCode { get; }

        public ExceptionResponse(string code, string message, HttpStatusCode httpStatusCode, string additionalMessage = null)
        {
            Code = code;
            Message = message;
            AdditionalMessage = additionalMessage;
            HttpStatusCode = httpStatusCode;
        }
    }
}