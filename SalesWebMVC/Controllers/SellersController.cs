using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SalesWebMVC.Models;
using SalesWebMVC.Services;

namespace SalesWebMVC.Controllers
{
    public class SellersController : Controller
    {
        private readonly SellerService _sellerService;

        public SellersController(SellerService sellerService) {
            _sellerService = sellerService;
        }

        // GET
        public IActionResult Index()
        {
            var list = _sellerService.FindAll();

            return View(list);
        }

        // GET: Sellers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST
        [HttpPost]
        [ValidateAntiForgeryToken] // Para previnir ataque XRSF / CSRF
        public IActionResult Create(Seller seller) {
            _sellerService.Insert(seller);

            //return RedirectToAction("Index");// Ou
            return RedirectToAction(nameof(Index));
        }

        // GET: Departments
        //    public async Task<IActionResult> Index()
        //  {
        //    return View(await _context.Department.ToListAsync());
        // }
    }
}