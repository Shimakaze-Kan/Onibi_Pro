using ErrorOr;

using MediatR;

using Onibi_Pro.Application.Services.Authentication;

namespace Onibi_Pro.Application.Authentication.Commands;
internal sealed class ConfirmEmailCommandHandler : IRequestHandler<ConfirmEmailCommand, ErrorOr<Success>>
{
    private readonly IAccountActivationService _accountActivationService;

    public ConfirmEmailCommandHandler(IAccountActivationService accountActivationService)
    {
        _accountActivationService = accountActivationService;
    }

    public async Task<ErrorOr<Success>> Handle(ConfirmEmailCommand request, CancellationToken cancellationToken)
    {
        await _accountActivationService.ActivateAccountAsync(request.Code, cancellationToken);
        return new Success();
    }
}
