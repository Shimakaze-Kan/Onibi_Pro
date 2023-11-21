using ErrorOr;
using Microsoft.AspNetCore.Mvc;
using Onibi_Pro.Http;

namespace Onibi_Pro.Controllers;

[ApiExplorerSettings(IgnoreApi = true)]
[ApiController]
public abstract class ApiBaseController : ControllerBase
{
    public IActionResult Problem(List<Error> errors)
    {
        HttpContext.Items[HttpContextItemKeys.Errors] = errors;

        var firstError = errors[0];

        var statusCode = firstError.Type switch
        {
            ErrorType.Conflict => StatusCodes.Status409Conflict,
            ErrorType.Validation => StatusCodes.Status400BadRequest,
            ErrorType.NotFound => StatusCodes.Status404NotFound,
            _ => StatusCodes.Status500InternalServerError
        };

        return Problem(statusCode: statusCode, title: firstError.Description);
    }
}
