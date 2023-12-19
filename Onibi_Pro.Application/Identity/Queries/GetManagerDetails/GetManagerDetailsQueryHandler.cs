using MediatR;

using Onibi_Pro.Application.Common.Interfaces.Services;
using Onibi_Pro.Application.Common.Models;

namespace Onibi_Pro.Application.Identity.Queries.GetManagerDetails;
internal sealed class GetManagerDetailsQueryHandler : IRequestHandler<GetManagerDetailsQuery, ManagerDetailsDto>
{
    private readonly IManagerDetailsService _managerDetailsService;

    public GetManagerDetailsQueryHandler(IManagerDetailsService managerDetailsService)
    {
        _managerDetailsService = managerDetailsService;
    }

    public async Task<ManagerDetailsDto> Handle(GetManagerDetailsQuery request, CancellationToken cancellationToken)
    {
        return await _managerDetailsService.GetManagerDetailsAsync(request.UserId);
    }
}
