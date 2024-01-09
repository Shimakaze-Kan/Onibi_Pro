namespace Onibi_Pro.Infrastructure.ExternalServices.Exceptions;
internal sealed class RequestFailedException : Exception
{
    private const string ErrorMessage = "Failed to send a request to the notification service.";

    public RequestFailedException()
        : base(ErrorMessage)
    {

    }
}
