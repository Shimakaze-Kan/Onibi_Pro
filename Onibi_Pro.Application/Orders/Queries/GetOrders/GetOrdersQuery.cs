﻿using ErrorOr;

using MediatR;

namespace Onibi_Pro.Application.Orders.Queries.GetOrders;
public record GetOrdersQuery(int StartRow, int Amount) : IRequest<ErrorOr<OrdersDto>>;