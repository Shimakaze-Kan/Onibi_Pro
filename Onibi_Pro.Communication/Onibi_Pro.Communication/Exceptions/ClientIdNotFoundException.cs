namespace Onibi_Pro.Communication.Exceptions;

public class ClientIdNotFoundException : NullReferenceException
{
    private const string ErrorMessage = "Client Id header not found.";

    public ClientIdNotFoundException()
        : base(ErrorMessage)
    { }
}
