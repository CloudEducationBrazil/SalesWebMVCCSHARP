using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NuGet.Protocol.Plugins;
using SalesWebMVC.Data;
using SalesWebMVC.Models;
using System.Linq;

namespace SalesWebMVC.Services
{
    public class SellerService
    {
        private readonly SalesWebMVCContext _context;

        public SellerService (SalesWebMVCContext context)
        {
            _context = context;
        }

        public List<Seller> FindAll()
        {
            return _context.Seller.ToList(); 
        }

        public void Insert(Seller seller)
        {
            //seller.Department = _context.Department.First(); // Para não ficar NULO
            _context.Add(seller);
            _context.SaveChanges();
        }
        public Seller? FindById(int sellerId)
        {
            //return _context.Seller.FirstOrDefault(obj => obj.Id == sellerId);
            return _context.Seller.Include(obj => obj.Department).FirstOrDefault(obj => obj.Id == sellerId);
            // return _context.Seller.Find(sellerId);

            //var seller = _context.Seller.FirstOrDefault(obj => obj.Id == sellerId);
            //if (seller == null) {
            //    throw new ArgumentException($"Seller with Id {sellerId} not found.");
            //}
            //return seller;
        }
        public void Remove(int sellerId)
        {
            var obj = _context.Seller.Find(sellerId);
            _context.Seller.Remove(obj);
            _context.SaveChanges();
        }
    }
}