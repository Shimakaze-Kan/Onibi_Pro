using ErrorOr;

using MediatR;

using Onibi_Pro.Application.Common.Models;
using Onibi_Pro.Domain.UserAggregate.ValueObjects;

namespace Onibi_Pro.Application.Identity.Queries.GetRegionalManagerDetails;
public record GetRegionalManagerDetailsQuery(UserId UserId) : IRequest<ErrorOr<RegionalManagerDetailsDto>>;