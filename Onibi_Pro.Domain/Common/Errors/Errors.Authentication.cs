using ErrorOr;

namespace Onibi_Pro.Domain.Common.Errors;
public static partial class Errors
{
    public static class Authentication
    {
        public static Error InvalidCredentials => Error.Validation(
            code: "Auth.InvalidCredentials", description: "Invalid Credentials.");
    }
}