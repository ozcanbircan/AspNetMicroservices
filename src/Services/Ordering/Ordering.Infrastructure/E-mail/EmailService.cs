using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Ordering.Application.Contracts.Infrastructure;
using Ordering.Application.Models;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Ordering.Infrastructure.E_mail {

	public class EmailService : IEmailService {
		private readonly EmailSettings _emailSettings;
		private readonly ILogger<EmailService> _logger;

		public EmailService(IOptions<EmailSettings> emailSettings, ILogger<EmailService> logger) {
			if (emailSettings == null) 
				throw new ArgumentNullException(nameof(emailSettings));
			_emailSettings = emailSettings.Value;

			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
		}

		public async Task<bool> SendEmail(Email email) {
			var from = new EmailAddress { Email = _emailSettings.FromAddress, Name = _emailSettings.FromName };
			var to = new EmailAddress(email.To);
			SendGridMessage message = MailHelper.CreateSingleEmail(from, to, email.Subject, email.Body, email.Body);

			var client = new SendGridClient(_emailSettings.ApiKey);
			Response response = await client.SendEmailAsync(message);
			if (response.StatusCode is HttpStatusCode.Accepted or HttpStatusCode.OK) {
				_logger.LogInformation("Email sent successfully.");
				return true;
			}

			_logger.LogInformation("Email sending failed." );
			return false;
		}
	}
}