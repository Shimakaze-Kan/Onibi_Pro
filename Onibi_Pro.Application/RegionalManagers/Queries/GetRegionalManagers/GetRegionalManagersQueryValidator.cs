using FluentValidation;

namespace Onibi_Pro.Application.RegionalManagers.Queries.GetRegionalManagers;
public sealed class GetRegionalManagersQueryValidator : AbstractValidator<GetRegionalManagersQuery>
{
    public GetRegionalManagersQueryValidator()
    {
        RuleFor(x => x.PageNumber).GreaterThanOrEqualTo(1).LessThanOrEqualTo(20);
        RuleFor(x => x.PageSize).GreaterThanOrEqualTo(1);
    }
}
