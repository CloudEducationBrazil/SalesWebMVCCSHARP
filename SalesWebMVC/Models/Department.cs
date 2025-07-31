using System.ComponentModel.DataAnnotations;

namespace SalesWebMVC.Models
{
    public class Department
    {
        public int Id { get; set; }

        [MaxLength(255)]
        public string Name { get; set; }

        public Department() { }
        public Department(int id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
