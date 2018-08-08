using System;
using System.IO;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.ApplicationModel.Email;
using Windows.Foundation;
using Windows.Storage.Streams;

namespace SLD.Tezos.Client.OS
{
	public class WindowsMail : ISendMail
	{
		public async Task<bool> Send(string recipients, string subject, string body, string attachmentName, byte[] attachmentData)
		{
			if (!string.IsNullOrWhiteSpace(recipients))
			{
				// TODO recipients
				//emailMessage.To.Add(new EmailRecipient("***@***.com"));
				throw new NotImplementedException();
			}

			EmailMessage emailMessage = new EmailMessage
			{
				Body = body,
				Subject = subject,
			};

			if (attachmentData != null)
			{
				var stream = new InMemoryRandomAccessStream();

				await stream.WriteAsync(attachmentData.AsBuffer());

				emailMessage.Attachments.Add(new EmailAttachment(
					attachmentName,
					RandomAccessStreamReference.CreateFromStream(stream)));
			}

			var result = EmailManager.ShowComposeNewEmailAsync(emailMessage);

			await result;

			return result.Status == AsyncStatus.Completed;
		}
	}
}