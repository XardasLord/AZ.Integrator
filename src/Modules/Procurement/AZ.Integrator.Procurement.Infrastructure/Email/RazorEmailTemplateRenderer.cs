using AZ.Integrator.Procurement.Application.Common.Email;
using Microsoft.Extensions.Logging;
using RazorLight;

namespace AZ.Integrator.Procurement.Infrastructure.Email;

public class RazorEmailTemplateRenderer : IEmailTemplateRenderer
{
    private readonly RazorLightEngine _engine;
    private readonly ILogger<RazorEmailTemplateRenderer> _logger;
    private readonly string _templatesPath;

    public RazorEmailTemplateRenderer(ILogger<RazorEmailTemplateRenderer> logger, string templatesPath)
    {
        _logger = logger;
        _templatesPath = templatesPath;
        
        var fullPath = Path.Combine(AppContext.BaseDirectory, templatesPath);
        
        _engine = new RazorLightEngineBuilder()
            .UseFileSystemProject(fullPath)
            .UseMemoryCachingProvider()
            .Build();
    }

    public async Task<string> RenderAsync<TModel>(
        string templateName, 
        TModel model, 
        CancellationToken cancellationToken = default)
    {
        try
        {
            var templateKey = $"{templateName}.cshtml";
            
            _logger.LogDebug("Rendering template {TemplateName} with model type {ModelType}", 
                templateName, typeof(TModel).Name);
            
            var result = await _engine.CompileRenderAsync(templateKey, model);
            
            _logger.LogDebug("Template {TemplateName} rendered successfully", templateName);
            
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to render template {TemplateName}", templateName);
            throw;
        }
    }
}

