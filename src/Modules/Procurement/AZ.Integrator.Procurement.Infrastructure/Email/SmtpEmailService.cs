using AZ.Integrator.Procurement.Application.Common.Email;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;

namespace AZ.Integrator.Procurement.Infrastructure.Email;

public class SmtpEmailService(
    IOptions<SmtpSettings> settings,
    ILogger<SmtpEmailService> logger)
    : IEmailService
{
    private readonly SmtpSettings _settings = settings.Value;

    public async Task SendEmailAsync(
        IEnumerable<string> to,
        string subject,
        string body,
        bool isHtml = false,
        IEnumerable<string>? cc = null,
        CancellationToken cancellationToken = default)
    {
        var recipients = to.ToList();
        var ccRecipients = cc?.ToList() ?? [];
        
        if (recipients.Count == 0)
        {
            logger.LogWarning("No recipients specified for email with subject: {Subject}", subject);
            return;
        }

        try
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(_settings.FromName, _settings.From));
            
            foreach (var recipient in recipients)
            {
                message.To.Add(MailboxAddress.Parse(recipient));
            }
            
            foreach (var ccRecipient in ccRecipients)
            {
                message.Cc.Add(MailboxAddress.Parse(ccRecipient));
            }
            
            message.Subject = subject;
            
            var bodyBuilder = new BodyBuilder();
            if (isHtml)
            {
                bodyBuilder.HtmlBody = body;
            }
            else
            {
                bodyBuilder.TextBody = body;
            }
            
            message.Body = bodyBuilder.ToMessageBody();

            using var client = new SmtpClient();
            
            await client.ConnectAsync(_settings.Host, _settings.Port, _settings.UseSsl, cancellationToken);
            
            if (!string.IsNullOrEmpty(_settings.Username) && !string.IsNullOrEmpty(_settings.Password))
            {
                await client.AuthenticateAsync(_settings.Username, _settings.Password, cancellationToken);
            }
            
            await client.SendAsync(message, cancellationToken);
            await client.DisconnectAsync(true, cancellationToken);
            
            var logMessage = ccRecipients.Count != 0
                ? $"Email sent successfully to {recipients.Count} recipient(s) and {ccRecipients.Count} CC recipient(s). Subject: {subject}"
                : $"Email sent successfully to {recipients.Count} recipient(s). Subject: {subject}";
                
            logger.LogInformation(logMessage);
        }
        catch (Exception ex)
        {
            logger.LogError(
                ex, 
                "Failed to send email to {RecipientCount} recipient(s). Subject: {Subject}", 
                recipients.Count, 
                subject);
            throw;
        }
    }
}

