using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace Onibi_Pro.Pages;

public class LoginModel : PageModel
{
    [Required]
    [BindProperty]
    public string? Login { get; set; }

    [Required]
    [BindProperty]
    public string? Password { get; set; }

    public void OnGet()
    {
        // Optional: Add logic for handling GET requests
    }

    public IActionResult OnPost()
    {
        if (!ModelState.IsValid)
        {
            ViewData["MarkInvalidOnLoad"] = true;
            return Page();
        }

        if (Login == "demo" && Password == "password")
        {
            return Redirect("https://localhost:44406");
        }
        else
        {
            ModelState.AddModelError("", "Invalid username or password");
            return Page(); // Stay on the login page with error message
        }
    }
}