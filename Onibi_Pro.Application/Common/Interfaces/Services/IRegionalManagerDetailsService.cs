﻿using Onibi_Pro.Application.Common.Models;
using Onibi_Pro.Domain.RegionalManagerAggregate.ValueObjects;
using Onibi_Pro.Domain.UserAggregate.ValueObjects;

namespace Onibi_Pro.Application.Common.Interfaces.Services;
public interface IRegionalManagerDetailsService
{
    Task<RegionalManagerDetailsDto> GetRegionalManagerDetailsAsync(UserId userId);
    Task<UserId> GetUserId(RegionalManagerId regionalManagerId);
}
