using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Onibi_Pro.Application.Services.Authentication;
using Onibi_Pro.Http;
using System.ComponentModel.DataAnnotations;

namespace Onibi_Pro.Pages;

[AllowAnonymous]
public class LoginModel : PageModel
{
    private const string AngularLandingPage = "/welcome";
    private readonly IAuthenticationService _authenticationService;

    [Required]
    [BindProperty]
    public string? Login { get; set; }

    [Required]
    [BindProperty]
    public string? Password { get; set; }

    public LoginModel(IAuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
    }

    public IActionResult OnGet()
    {
        if(HttpContext.Request.Cookies.ContainsKey(AuthenticationKeys.CookieName))
        {
            return Redirect(AngularLandingPage);
        }

        return Page();
    }

    public IActionResult OnPost()
    {
        if (!ModelState.IsValid)
        {
            ViewData["MarkInvalidOnLoad"] = true;
            return Page();
        }

        var authResult = _authenticationService.Login(Login!, Password!);

        return authResult.Match<IActionResult>(resutl =>
        {
            var options = new CookieOptions
            {
                Expires = DateTime.Now.AddDays(1),
                HttpOnly = false,
                Secure = true,
                SameSite = SameSiteMode.Strict
            };

            HttpContext.Response.Cookies.Append(AuthenticationKeys.CookieName, resutl.Token, options);
            return Redirect(AngularLandingPage);
        }, errors =>
        {
            ModelState.AddModelError("alertError", errors[0].Description);
            return Page();
        });
    }
}