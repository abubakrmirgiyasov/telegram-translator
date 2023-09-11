using WebHook.Translator.Common;
using WebHook.Translator.Models.Interfaces;

namespace WebHook.Translator.Models;

[BsonCollection("employees")]
public class Employee : Document
{
    public string FirstName { get; set; } = null!;

    public string SecondName { get; set; } = null!;

    public string? LastName { get; set; }

    public string Email { get; set; } = null!;

    public long Phone { get; set; }

    public string Password { get; set; } = null!;

    public byte[] Salt { get; set; } = null!;

    public string Ip { get; set; } = null!;

    public string? Photo { get; set; }

    public List<Role> Roles { get; set; } = null!;
}

public class EmployeeViewModel
{
    public string FirstName { get; set; } = null!;

    public string SecondName { get; set; } = null!;

    public string? LastName { get; set; }

    public string Email { get; set; } = null!;

    public long Phone { get; set; }

    public string Password { get; set; } = null!;

    public string Ip { get; set; } = null!;

    public string? Photo { get; set; }
}

public class EmployeeBindingModel
{
    public string FirstName { get; set; } = null!;

    public string SecondName { get; set; } = null!;

    public string? LastName { get; set; }

    public string Email { get; set; } = null!;

    public long Phone { get; set; }

    public string Password { get; set; } = null!;

    public string Ip { get; set; } = null!;

    public string? Folder { get; set; }

    public IFormFile? Photo { get; set; }
}