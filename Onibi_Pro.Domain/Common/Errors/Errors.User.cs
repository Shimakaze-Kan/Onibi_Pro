﻿using ErrorOr;

namespace Onibi_Pro.Domain.Common.Errors;
public static partial class Errors
{
    public static class User
    {
        public static Error DuplicateEmail => Error.Conflict(
            code: "User.DuplicateEmail", description: "Email is already in use.");
        
        public static Error UserNotFound => Error.NotFound(
            code: "User.NotFound", description: "User does not exist.");

        public static Error UnsupportedCreatorUserType => Error.Validation(
            code: "User.UnsupportedCreatorUserType", description: "User with this user type cannot create new user.");
    }
}
