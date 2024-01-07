namespace Onibi_Pro.Communication.Exceptions;

public class ClientNameNotFoundException: NullReferenceException
{
    private const string ErrorMessage = "Client Name header not found.";

    public ClientNameNotFoundException()
        : base(ErrorMessage)
    { }
}
