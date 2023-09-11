using Microsoft.Extensions.Options;
using WebHook.Translator.Common;
using WebHook.Translator.Infrastructure.DbContext;
using WebHook.Translator.Models;

namespace WebHook.Translator.Infrastructure.Repositories;

public class EmployeeRepository : Repository<Employee>
{
	private readonly RoleRepository _role;

    public EmployeeRepository(RoleRepository role, IOptions<AppSettings> settings)
        : base(settings) 
	{
		_role = role;
	}

    public async Task CreateEmployeeAsync(EmployeeBindingModel model)
    {
		try
		{
			byte[] salt = Constants.GetSalt();
			string password = Constants.GetHash(model.Password, salt);
            string? path = await Constants.AddImage(model.Folder, model.Photo);

			var roles = _role.FilterBy(x => x.Key == "user").ToList();

            var employee = new Employee()
			{
				FirstName = model.FirstName,
				LastName = model.LastName,
				SecondName = model.SecondName,
				Email = model.Email,
				Ip = model.Ip,
				Password = password,
				Phone = model.Phone,
				Photo = path,
				Roles = roles,
			};

			await InsertOneAsync(employee);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message, ex);
		}
    }
}
