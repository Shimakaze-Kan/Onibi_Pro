using MimeKit.Utils;

namespace Onibi_Pro.Infrastructure.Email;
internal sealed class TemplateReader : ITemplateReader
{
    public (Stream Stream, string Filename, string ContentId) ReadImage(string fileName)
    {
        string filePath = CreateFilePath(fileName);
        var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
        var contentId = MimeUtils.GenerateMessageId();

        return (stream, fileName, contentId);
    }

    public async Task<string> ReadTemplateAsync(string fileName, CancellationToken cancellationToken)
    {
        string filePath = CreateFilePath(fileName);

        var template = await File.ReadAllTextAsync(filePath, cancellationToken);

        return template;
    }

    private static string CreateFilePath(string fileName)
    {
        string templatesFolderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Email\\Templates");

        if (!Directory.Exists(templatesFolderPath))
        {
            throw new DirectoryNotFoundException(templatesFolderPath);
        }

        string filePath = Path.Combine(templatesFolderPath, fileName);

        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException(filePath);
        }

        return filePath;
    }
}
