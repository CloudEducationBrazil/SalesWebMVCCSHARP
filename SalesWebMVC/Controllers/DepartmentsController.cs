using Microsoft.AspNetCore.Mvc;
using SalesWebMVC.Models;

namespace SalesWebMVC.Controllers
{
    public class DepartmentsController : Controller
    {
        public IActionResult Index()
        {
            List<Department> departments = new List<Department>();
            departments.Add(new Department(1, "Eletronics"));
            departments.Add(new Department(2, "Fashion"));
            departments.Add(new Department(3, "Clothes"));
            departments.Add(new Department(4, "Utensils"));

            return View(departments);
        }
    }
}
