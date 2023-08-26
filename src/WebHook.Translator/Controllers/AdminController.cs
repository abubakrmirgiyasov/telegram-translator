using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebHook.Translator.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AdminController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        return Ok(new { message = "trestsdfsf" });
    }
}
