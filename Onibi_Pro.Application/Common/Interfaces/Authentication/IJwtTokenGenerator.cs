﻿namespace Onibi_Pro.Application.Common.Interfaces.Authentication;
public interface IJwtTokenGenerator
{
    string GenerateToken(Guid userId, string firstName, string lastName, string email, string userType, string clientName);
}
