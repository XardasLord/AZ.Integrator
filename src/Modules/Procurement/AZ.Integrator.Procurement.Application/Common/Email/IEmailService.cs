namespace AZ.Integrator.Procurement.Application.Common.Email;

/// <summary>
/// Service for sending emails.
/// </summary>
public interface IEmailService
{
    /// <summary>
    /// Sends an email to the specified recipients.
    /// </summary>
    /// <param name="to">List of recipient email addresses</param>
    /// <param name="subject">Email subject</param>
    /// <param name="body">Email body content</param>
    /// <param name="isHtml">Indicates whether the body is HTML</param>
    /// <param name="cc">List of CC (carbon copy) email addresses</param>
    /// <param name="cancellationToken">Cancellation token</param>
    Task SendEmailAsync(
        IEnumerable<string> to,
        string subject,
        string body,
        bool isHtml = false,
        IEnumerable<string>? cc = null,
        CancellationToken cancellationToken = default);
}

