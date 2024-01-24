namespace Onibi_Pro.Application.Common.Interfaces.Authentication;
public interface IUserActivationRepository
{
    Task ActivateAsync(string email, CancellationToken cancellationToken);
}
