namespace Onibi_Pro.Shared;
public static class Passwords
{
    // user has to change it after email confirmation
    public static string InitialPassword => Guid.NewGuid().ToString();
}
