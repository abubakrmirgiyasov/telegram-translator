using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebHook.Translator.Infrastructure.Repositories;
using WebHook.Translator.Models;

namespace WebHook.Translator.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class AdminController : ControllerBase
{
    private readonly TestRepository _testRepository;

    public AdminController(TestRepository testRepository)
    {
        _testRepository = testRepository;
    }

    [HttpGet]
    public IActionResult Get()
    {
        return Ok(new { message = "trestsdfsf" });
    }

    [HttpPost]
    public IActionResult Test([FromBody] Test test)
    {
        try
        {
            test.CorrectOption -= 1;
            _testRepository.InsertOne(test);
            return Ok(new { message = "post success" });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}
