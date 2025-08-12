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
        public async Task<IActionResult> Index()
        {
            var list = await _sellerService.FindAllAsync();

            return View(list);
        }

        // View Create
        public async Task<IActionResult> Create()
        {
            var departments = await _departmentService.FindAllAsync();
            var viewModel = new SellerFormViewModel { Departments = departments };

            return View(viewModel);
        }

        // Serviço POST
        [HttpPost]
        [ValidateAntiForgeryToken] // Para previnir ataque XRSF / CSRF
        public async Task<IActionResult> Create(Seller seller)
        {
            if (!ModelState.IsValid)
            {   
                var departments = await _departmentService.FindAllAsync();
                var viewModel = new SellerFormViewModel { Seller = seller, Departments = departments }; 
                return View(viewModel);
            }

            await _sellerService.InsertAsync(seller);

            //return RedirectToAction("Index");// Ou
            return RedirectToAction(nameof(Index));
        }

        // View Delete
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                //return NotFound();
                return RedirectToAction(nameof(Error), new { message = "Id not provided ..." });
            }

            var obj = await _sellerService.FindByIdAsync(id.Value);
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
        public async Task<IActionResult> Delete(int id)
        {
            await _sellerService.RemoveAsync(id);

            //return RedirectToAction("Index");// Ou
            return RedirectToAction(nameof(Index));
        }

        // View Details
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                //return NotFound();
                return RedirectToAction(nameof(Error), new { message = "Id not provided ..." });
            }

            var obj = await _sellerService.FindByIdAsync(id.Value);
            if (obj == null)
            {
                //return NotFound();
                return RedirectToAction(nameof(Error), new { message = "Id not found ..." });
            }

            return View(obj);
        }

        // View Edit
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                //return NotFound();
                return RedirectToAction(nameof(Error), new { message = "Id not provided ..." });
            }

            var obj = await _sellerService.FindByIdAsync(id.Value);

            if (obj == null)
            {
                //return NotFound();
                return RedirectToAction(nameof(Error), new { message = "Id not found ..." });
            }

            List<Department> departments = await _departmentService.FindAllAsync();
            SellerFormViewModel viewModel = new SellerFormViewModel { Seller = obj, Departments = departments };

            return View(viewModel);
        }

        // Serviço EDIT
        [HttpPost]
        [ValidateAntiForgeryToken] // Para previnir ataque XRSF / CSRF
        public async Task<IActionResult> Edit(int? id, Seller seller)
        {
            if (!ModelState.IsValid)
            {
                var departments = await _departmentService.FindAllAsync();
                var viewModel = new SellerFormViewModel { Seller = seller, Departments = departments };
                return View(viewModel);
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
                await _sellerService.UpdateAsync(seller);
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