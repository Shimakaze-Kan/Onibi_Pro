using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Onibi_Pro.Pages;

[AllowAnonymous]
public class ConfirmEmailModel : PageModel
{
    public IActionResult OnGet()
    {
        var code = HttpContext.Request.RouteValues["code"]?.ToString();

        if (string.IsNullOrEmpty(code))
        {
            return Redirect("/");
        }

        return Page();
    }
}
