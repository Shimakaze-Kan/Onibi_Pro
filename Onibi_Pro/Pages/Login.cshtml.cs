using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Onibi_Pro.Application.Services.Authentication;
using System.ComponentModel.DataAnnotations;

namespace Onibi_Pro.Pages;

public class LoginModel : PageModel
{
    private const string CookieName = "OnibiAuth";
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
        if(HttpContext.Request.Cookies.ContainsKey(CookieName))
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

            HttpContext.Response.Cookies.Append(CookieName, resutl.Token, options);
            return Redirect(AngularLandingPage);
        }, errors =>
        {
            ModelState.AddModelError("alertError", errors[0].Description);
            return Page();
        });
    }
}