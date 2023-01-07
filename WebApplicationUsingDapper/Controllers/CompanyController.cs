using Dapper;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Xml.Linq;
using WebApplicationUsingDapper.Domain.College;
using WebApplicationUsingDapper.Domain.CollegeAggregate;
using WebApplicationUsingDapper.Domain.CourseAggregate;
using WebApplicationUsingDapper.Model;

namespace WebApplicationUsingDapper.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        private readonly IConfiguration _config;
        public CompanyController(IConfiguration config)
        {
            _config = config;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            var sql = @"SELECT * FROM 
                    Company INNER JOIN Address ON Company.CompanyId = Address.CompanyId 
                    INNER JOIN Service ON Company.CompanyId = Service.CompanyId
                    INNER JOIN Carrer ON Company.CompanyId = Carrer.CompanyId
                    INNER JOIN Foundation ON Company.CompanyId = Foundation.CompanyId
                    INNER JOIN Department ON Company.CompanyId = Department.CompanyId
                    INNER JOIN Country ON Company.CompanyId = Country.CompanyId
                    INNER JOIN Brand ON Company.CompanyId = Brand.CompanyId
                    INNER JOIN Religion ON Company.CompanyId = Religion.CompanyId
                    INNER JOIN Technology ON Company.CompanyId = Technology.CompanyId
                    INNER JOIN Sports ON Company.CompanyId = Sports.CompanyId;";
            var companies = await connection.QueryAsync<Company>(sql, new[] {typeof(Company), typeof(Address), typeof(Service), typeof(Carrer), typeof(Foundation), typeof(Department), typeof(Country), typeof(Brand), typeof(Religion), typeof(Technology), typeof(Sports)},
                objects =>
                {
                    Company c = objects[0] as Company;
                    Address a = objects[1] as Address;
                    Service s = objects[2] as Service;
                    Carrer car = objects[3] as Carrer;
                    Foundation f = objects[4] as Foundation;
                    Department d = objects[5] as Department;
                    Country cou = objects[6] as Country;
                    Brand b = objects[7] as Brand;
                    Religion r = objects[8] as Religion;
                    Technology t = objects[9] as Technology;
                    Sports spo = objects[10] as Sports;

                    c.Address = a;
                    c.Service = s;
                    c.Carrer = car;
                    c.Foundation = f;
                    c.Department = d;
                    c.Country = cou;
                    c.Brand = b;
                    c.Religion = r;
                    c.Technology = t;
                    c.Sports = spo;

                    return c;
                },
                splitOn: "CompanyId"
            );
            return Ok(companies);  

        }

        [HttpPost]
        public async Task<IActionResult> AddCourse(CompanyRequestModel model)
        {
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            var company = await connection.ExecuteAsync("Insert into Company (CompanyId, CompanyName) values(@id, @name)", new { id = model.CompanyId, name = model.ComapanyName });

            var service = await connection.ExecuteAsync("Insert into Service (CompanyId, ServiceName) values(@id, @street)", new { id = model.CompanyId, street = model.ServiceName});
            var carrer = await connection.ExecuteAsync("Insert into Carrer (CompanyId, CarrerName) values(@id, @street)", new { id = model.CompanyId, street = model.CarrerName});
            var foundation = await connection.ExecuteAsync("Insert into Foundation (CompanyId, FoundationName) values(@id, @street)", new { id = model.CompanyId, street = model.FoundationName});
            var department = await connection.ExecuteAsync("Insert into department (CompanyId, DepartmentName) values(@id, @street)", new { id = model.CompanyId, street = model.DepartmentName});
            var address = await connection.ExecuteAsync("Insert into Address (CompanyId, Street, City, State, PostalCode) values(@id, @street, @city, @state, @postal)", new { id = model.CompanyId, street = model.Street, city = model.City, model.State, postal = model.PostalCode });
            var brand = await connection.ExecuteAsync("Insert into Brand (CompanyId, BrandName) values(@id, @name)", new { id = model.CompanyId, name = model.BrandName });
            var religion = await connection.ExecuteAsync("Insert into Religion (CompanyId, ReligionName) values(@id, @name)", new { id = model.CompanyId, name = model.ReligionName });
            var sports = await connection.ExecuteAsync("Insert into Sports (CompanyId, SportsName) values(@id, @name)", new { id = model.CompanyId, name = model.SportsName });
            var technology = await connection.ExecuteAsync("Insert into Technology (CompanyId, TechnologyName) values(@id, @name)", new { id = model.CompanyId, name = model.TechnologyName });
            var country = await connection.ExecuteAsync("Insert into Country (CompanyId, CountryName) values(@id, @name)", new { id = model.CompanyId, name = model.CountryName });
            if(company==null|| service==null|| carrer == null || foundation == null || department == null || address == null || brand == null || religion == null || sports==null|| technology==null|| country==null) return BadRequest("Not Inserted");
            return Ok("Added successfully");
        }
    }
}
