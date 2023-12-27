using FluentValidation;

namespace Onibi_Pro.Application.Orders.Queries.GetOrders;
public sealed class GetOrderQueryValidator : AbstractValidator<GetOrdersQuery>
{
    public GetOrderQueryValidator()
    {
        RuleFor(x => x.Amount).LessThan(20).GreaterThanOrEqualTo(1);
        RuleFor(x => x.StartRow).GreaterThanOrEqualTo(1);
    }
}
