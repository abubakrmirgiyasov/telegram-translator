using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebHook.Translator.Infrastructure.Repositories;
using WebHook.Translator.Models;

namespace WebHook.Translator.Controllers;

[ApiController]
[Route("[controller]/[action]")]
[Authorize(Roles = $"ADMIN")]
public class AdminController : ControllerBase
{
    private readonly TestRepository _testRepository;
    private readonly ImageQuestionRepository _imageQuestionRepository;

    public AdminController(TestRepository testRepository, ImageQuestionRepository imageQuestionRepository)
    {
        _testRepository = testRepository;
        _imageQuestionRepository = imageQuestionRepository;

    }

    [HttpPost]
    public IActionResult CreateQuestion([FromBody] Test model)
    {
        try
        {
            model.CorrectOption -= 1;
            _testRepository.InsertOne(model);
            return Ok(new { message = "post success" });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreateImageQuestion([FromForm] ImageQuestionBindingModel model)
    {
        try
        {
            model.CorrectAnswer -= 1;
            await _imageQuestionRepository.CreateImageQuestionAsync(model);
            return Ok(new { message = "post success" });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}
