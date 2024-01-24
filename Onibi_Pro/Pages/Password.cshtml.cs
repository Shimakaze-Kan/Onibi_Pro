using System.ComponentModel.DataAnnotations;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using Onibi_Pro.Application.Common.Interfaces.Authentication;
using Onibi_Pro.Application.Common.Interfaces.Services;
using Onibi_Pro.Application.Persistence;
using Onibi_Pro.Domain.UserAggregate.ValueObjects;
using Onibi_Pro.Shared;

namespace Onibi_Pro.Pages;

[AllowAnonymous]
public class PasswordModel : PageModel
{
    private readonly IUserPasswordRepository _userPasswordRepository;
    private readonly IActivateEncryptionService _activateEncryptionService;
    private readonly IMasterDbRepository _masterDbRepository;
    private readonly IPasswordService _passwordService;

    [Required]
    [BindProperty]
    public string? Password { get; set; }

    [Required]
    [BindProperty]
    public string? RepeatPassword { get; set; }

    [BindProperty]
    public string? Login { get; set; }

    public PasswordModel(IUserPasswordRepository userPasswordRepository,
        IActivateEncryptionService activateEncryptionService,
        IMasterDbRepository masterDbRepository,
        IPasswordService passwordService)
    {
        _userPasswordRepository = userPasswordRepository;
        _activateEncryptionService = activateEncryptionService;
        _masterDbRepository = masterDbRepository;
        _passwordService = passwordService;
    }

    public IActionResult OnGet()
    {
        var code = HttpContext.Request.RouteValues["code"]?.ToString();

        if (string.IsNullOrEmpty(code))
        {
            return Redirect("/");
        }

        var (_, email, isExpired) = _activateEncryptionService.DecryptData(code);

        if (isExpired)
        {
            return Redirect("/");
        }
        Login = email;

        return Page();
    }

    public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken)
    {
        var areTheSame = Password == RepeatPassword;

        if (!areTheSame)
        {
            ModelState.AddModelError("alertError", "Passwords must be the same.");
            return Page();
        }

        if (string.IsNullOrEmpty(Password) || string.IsNullOrEmpty(RepeatPassword))
        {
            ViewData["MarkInvalidOnLoad"] = true;
            return Page();
        }

        try
        {
            var code = HttpContext.Request.RouteValues["code"]?.ToString();
            var (userId, email, isExpired) = _activateEncryptionService.DecryptData(code);

            if (isExpired)
            {
                return Redirect("/");
            }

            var clientName = await _masterDbRepository.GetClientNameByUserEmail(email);
            var password = _passwordService.HashPassword(Password);
            Login = email;

            await _userPasswordRepository.UpdatePasswordAsync(UserId.Create(userId), password, clientName, cancellationToken);
            Response.Cookies.Delete(AuthenticationKeys.CookieName);
        }
        catch (Exception)
        {
            ModelState.AddModelError("alertError", "An error occured.");
            return Page();
        }

        return Redirect("/");
    }
}
