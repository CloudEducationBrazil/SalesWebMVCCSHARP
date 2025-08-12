using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SalesWebMVC.Models;
using SalesWebMVC.Models.ViewModels;
using SalesWebMVC.Services;
using SalesWebMVC.Services.Exceptions;
using System.Diagnostics;

namespace SalesWebMVC.Controllers
{
    public class SellersController : Controller
    {
        private readonly SellerService _sellerService;
        private readonly DepartmentService _departmentService;

        // Injeção de Dependências
        public SellersController(SellerService sellerService, DepartmentService departmentService)
        {
            _sellerService = sellerService;
            _departmentService = departmentService;
        }

        // GET
        public IActionResult Index()
        {
            var list = _sellerService.FindAll();

            return View(list);
        }

        // View Create
        public IActionResult Create()
        {
            var departments = _departmentService.FindAll();
            var viewModel = new SellerFormViewModel { Departments = departments };

            return View(viewModel);
        }

        // Serviço POST
        [HttpPost]
        [ValidateAntiForgeryToken] // Para previnir ataque XRSF / CSRF
        public IActionResult Create(Seller seller)
        {
            if (!ModelState.IsValid)
            {   
                var departments = _departmentService.FindAll();
                var viewModel = new SellerFormViewModel { Seller = seller, Departments = departments }; 
                return View(viewModel);
            }

            _sellerService.Insert(seller);

            //return RedirectToAction("Index");// Ou
            return RedirectToAction(nameof(Index));
        }

        // View Delete
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                //return NotFound();
                return RedirectToAction(nameof(Error), new { message = "Id not provided ..." });
            }

            var obj = _sellerService.FindById(id.Value);
            if (obj == null)
            {
                // return NotFound();
                return RedirectToAction(nameof(Error), new { message = "Id not found ..." });
            }

            return View(obj);
        }

        // Serviço DELETE
        [HttpPost]
        [ValidateAntiForgeryToken] // Para previnir ataque XRSF / CSRF
        public IActionResult Delete(int id)
        {
            _sellerService.Remove(id);

            //return RedirectToAction("Index");// Ou
            return RedirectToAction(nameof(Index));
        }

        // View Details
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                //return NotFound();
                return RedirectToAction(nameof(Error), new { message = "Id not provided ..." });
            }

            var obj = _sellerService.FindById(id.Value);
            if (obj == null)
            {
                //return NotFound();
                return RedirectToAction(nameof(Error), new { message = "Id not found ..." });
            }

            return View(obj);
        }

        // View Edit
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                //return NotFound();
                return RedirectToAction(nameof(Error), new { message = "Id not provided ..." });
            }

            var obj = _sellerService.FindById(id.Value);

            if (obj == null)
            {
                //return NotFound();
                return RedirectToAction(nameof(Error), new { message = "Id not found ..." });
            }

            List<Department> departments = _departmentService.FindAll();
            SellerFormViewModel viewModel = new SellerFormViewModel { Seller = obj, Departments = departments };

            return View(viewModel);
        }

        // Serviço EDIT
        [HttpPost]
        [ValidateAntiForgeryToken] // Para previnir ataque XRSF / CSRF
        public IActionResult Edit(int? id, Seller seller)
        {
            if (!ModelState.IsValid)
            {
                return View(seller);
            }

            if (id == null)
            {
                //NotFound();
                return RedirectToAction(nameof(Error), new { message = "Id not provided ..." });
            }

            if (id != seller.Id)
            {
                //  return BadRequest();
                return RedirectToAction(nameof(Error), new { message = "Id mismatch ..." });
            }

            try
            {
                _sellerService.Update(seller);
                return RedirectToAction(nameof(Index));
            }
            catch (NotFoundException e) // ApplicationException e
            {
                // return NotFound();
                return RedirectToAction(nameof(Error), new { message = e.Message });
            }
            catch (DbConcurrencyException e) // ApplicationException e
            {
                //return BadRequest();
                return RedirectToAction(nameof(Error), new { message = e.Message });
            }
        }

        // View Error
        public IActionResult Error(string message)
        {
            var viewModel = new ErrorViewModel { Message = message, RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier };

            return View(viewModel);
        }
    }
}