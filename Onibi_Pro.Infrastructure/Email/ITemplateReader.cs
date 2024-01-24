
using System.Net.Mail;

namespace Onibi_Pro.Infrastructure.Email;

internal interface ITemplateReader
{
    Task<string> ReadTemplateAsync(string fileName, CancellationToken cancellationToken);
    (Stream Stream, string Filename, string ContentId) ReadImage(string fileName);
}