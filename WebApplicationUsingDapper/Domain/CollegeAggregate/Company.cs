using WebApplicationUsingDapper.Domain.CollegeAggregate;

namespace WebApplicationUsingDapper.Domain.College
{
    public class Company
    {
        public int CompanyId { get; set; }
        public string CompanyName { get; set; }

        public Address Address { get; set; }
        public Service Service { get; set; }
        public Foundation Foundation { get; set; }
        public Department Department { get; set; }
        public Carrer Carrer { get; set; }
        
        public Country Country { get; set; }
        public Brand Brand { get; set; }
        public Technology Technology { get; set; }
        public Religion Religion { get; set; }
        public Sports Sports { get; set; }
    }
}
