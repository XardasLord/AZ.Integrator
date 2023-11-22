namespace AZ.Integrator.Shared.Infrastructure.Persistence.GraphQL;

// public class GraphQlErrorFilter : IErrorFilter
// {
//     private readonly ILogger<GraphQlErrorFilter> _logger;
//
//     public GraphQlErrorFilter(ILogger<GraphQlErrorFilter> logger)
//     {
//         _logger = logger;
//     }
// 		
//     public IError OnError(IError error)
//     {
//         var message = $"Error when executing GraphQL query: {error.Exception?.Message ?? error.Message}";
// 			
//         _logger.LogError(message);
// 			
//         return error.WithMessage(error.Exception?.Message ?? error.Message);
//     }
// }