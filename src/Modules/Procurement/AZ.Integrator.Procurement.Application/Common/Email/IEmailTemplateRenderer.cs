namespace AZ.Integrator.Procurement.Application.Common.Email;

/// <summary>
/// Service for rendering email templates.
/// </summary>
public interface IEmailTemplateRenderer
{
    /// <summary>
    /// Renders a template with the specified model.
    /// </summary>
    /// <param name="templateName">Name of the template file (without extension)</param>
    /// <param name="model">Model object to pass to the template</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Rendered HTML content</returns>
    Task<string> RenderAsync<TModel>(
        string templateName, 
        TModel model, 
        CancellationToken cancellationToken = default);
}

