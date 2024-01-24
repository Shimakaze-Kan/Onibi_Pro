using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;

using Onibi_Pro.Application.Common.Interfaces.Services;

namespace Onibi_Pro.Controllers;
[Route("api/[controller]")]
[ApiController]
[AllowAnonymous]
public class TestController : ControllerBase
{
    private readonly IEmailSender _emailSender;

    public TestController(IEmailSender emailSender, IActionDescriptorCollectionProvider provider)
    {
        _emailSender = emailSender;
        _provider = provider;
    }

    [HttpGet]
    public async Task Send(CancellationToken cancellationToken)
    {
        await _emailSender.SendConfirmationEmailAsync("test@test.com", "https://www.diki.pl/slownik-angielskiego?q=get+it", cancellationToken);
    }

    private readonly IActionDescriptorCollectionProvider _provider;


    [HttpGet("routes")]
    public IActionResult GetRoutes()
    {
        var routes = _provider.ActionDescriptors.Items.Select(x => new {
            Action = x.RouteValues["Action"],
            Controller = x.RouteValues["Controller"],
            Name = x.AttributeRouteInfo.Name,
            Template = x.AttributeRouteInfo.Template
        }).ToList();
        return Ok(routes);
    }
}
