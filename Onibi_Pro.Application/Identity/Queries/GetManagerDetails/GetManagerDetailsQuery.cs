using MediatR;

using Onibi_Pro.Application.Common.Models;
using Onibi_Pro.Domain.UserAggregate.ValueObjects;

namespace Onibi_Pro.Application.Identity.Queries.GetManagerDetails;
public record GetManagerDetailsQuery(UserId UserId) : IRequest<ManagerDetailsDto>;