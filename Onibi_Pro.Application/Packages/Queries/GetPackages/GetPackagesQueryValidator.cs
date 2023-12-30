using FluentValidation;

namespace Onibi_Pro.Application.Packages.Queries.GetPackages;
public sealed class GetPackagesQueryValidator : AbstractValidator<GetPackagesQuery>
{
    public GetPackagesQueryValidator()
    {
        RuleFor(x => x.Amount).LessThan(20).GreaterThanOrEqualTo(1);
        RuleFor(x => x.StartRow).GreaterThanOrEqualTo(1);
    }
}
