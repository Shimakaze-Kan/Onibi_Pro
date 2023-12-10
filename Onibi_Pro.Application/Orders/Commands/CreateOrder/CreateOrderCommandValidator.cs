using FluentValidation;

using Onibi_Pro.Domain.Common.Errors;

namespace Onibi_Pro.Application.Orders.Commands.CreateOrder;
public sealed class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
{
    public CreateOrderCommandValidator()
    {
        RuleForEach(x => x.OrderItems).SetValidator(new OrderItemCommandValidator());
    }

    private class OrderItemCommandValidator : AbstractValidator<OrderItemComand>
    {
        public OrderItemCommandValidator()
        {
            RuleFor(x => x.Quantity).GreaterThan(0).WithMessage(Errors.Order.WrongMenuItemId.Code);
        }
    }
}
