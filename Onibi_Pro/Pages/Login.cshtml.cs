using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using Onibi_Pro.Application.Common.Interfaces.Services;
using Onibi_Pro.Application.Services.Authentication;
using Onibi_Pro.Infrastructure.Authentication;
using Onibi_Pro.Shared;
using System.ComponentModel.DataAnnotations;

namespace Onibi_Pro.Pages;

[AllowAnonymous]
public class LoginModel : PageModel
{
    private readonly IAuthenticationService _authenticationService;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly JwtTokenSettings _tokenSettings;

    [Required]
    [BindProperty]
    public string? Login { get; set; }

    [Required]
    [BindProperty]
    public string? Password { get; set; }

    public LoginModel(IAuthenticationService authenticationService,
        IOptions<JwtTokenSettings> options,
        IDateTimeProvider dateTimeProvider)
    {
        _authenticationService = authenticationService;
        _dateTimeProvider = dateTimeProvider;
        _tokenSettings = options.Value;
    }

    public IActionResult OnGet()
    {
        if(HttpContext.Request.Cookies.ContainsKey(AuthenticationKeys.CookieName))
        {
            return Redirect(RouteConstants.SpaLandingPage);
        }

        return Page();
    }

    public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            ViewData["MarkInvalidOnLoad"] = true;
            return Page();
        }

        var authResult = await _authenticationService.LoginAsync(Login!, Password!, cancellationToken);

        return authResult.Match<IActionResult>(resutl =>
        {
            var expires = _dateTimeProvider.UtcNow.AddMinutes(_tokenSettings.ExpirationTimeInMinutes);
            var options = new CookieOptions
            {
                Expires = expires,
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict
            };

            HttpContext.Response.Cookies.Append(AuthenticationKeys.CookieName, resutl.Token, options);
            return Redirect(RouteConstants.SpaLandingPage);
        }, errors =>
        {
            ModelState.AddModelError("alertError", errors[0].Description);
            return Page();
        });
    }
}