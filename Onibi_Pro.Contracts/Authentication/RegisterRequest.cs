﻿namespace Onibi_Pro.Contracts.Authentication;
public record RegisterRequest(
    string FirstName,
    string LastName,
    string Email,
    string UserType);
