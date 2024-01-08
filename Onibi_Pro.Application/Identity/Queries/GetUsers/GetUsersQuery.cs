﻿using MediatR;

namespace Onibi_Pro.Application.Identity.Queries.GetUsers;
public record GetUsersQuery(string Query, string? UserId) : IRequest<IReadOnlyCollection<UserDataDto>>;