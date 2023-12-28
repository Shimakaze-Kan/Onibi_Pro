﻿using ErrorOr;

using Microsoft.Extensions.Logging;

using Onibi_Pro.Application.Common.Interfaces.Services;
using Onibi_Pro.Application.Persistence;
using Onibi_Pro.Domain.Common.Errors;
using Onibi_Pro.Domain.UserAggregate;

using User = Onibi_Pro.Domain.UserAggregate.User;

namespace Onibi_Pro.Application.Services.Authentication;
internal sealed class RegisterService : IRegisterService
{
    private readonly ILogger<RegisterService> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPasswordService _passwordService;

    public RegisterService(ILogger<RegisterService> logger,
        IUnitOfWork unitOfWork,
        IPasswordService passwordService)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _passwordService = passwordService;
    }

    public async Task<ErrorOr<Success>> RegisterAsync(string firstName, string lastName,
        string email, string password, UserTypes userType, CancellationToken cancellationToken = default)
    {
        User? user = await GetUserByEmailAsync(email, cancellationToken);

        if (user is not null)
        {
            _logger.LogWarning("User already exists: {email}", email);
            return Errors.User.DuplicateEmail;
        }

        var hashedPassword = _passwordService.HashPassword(password);

        user = User.CreateUnique(firstName, lastName, email, hashedPassword, userType);

        await _unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            await _unitOfWork.UserRepository.AddAsync(user, cancellationToken);
            await _unitOfWork.SaveAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackTransactionAsync(cancellationToken);
            _logger.LogCritical(ex, "Error while creating user.");

            return Error.Unexpected();
        }

        await _unitOfWork.CommitTransactionAsync(cancellationToken);

        return new Success();
    }

    private async Task<User?> GetUserByEmailAsync(string email, CancellationToken cancellationToken)
    {
        return await _unitOfWork.UserRepository.GetAsync(user => user.Email == email, cancellationToken);
    }
}