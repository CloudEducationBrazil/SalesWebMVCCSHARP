using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NuGet.Protocol.Plugins;
using SalesWebMVC.Data;
using SalesWebMVC.Models;
using System.Linq;
using SalesWebMVC.Services.Exceptions;

namespace SalesWebMVC.Services
{
    public class SellerService
    {
        private readonly SalesWebMVCContext _context;

        public SellerService(SalesWebMVCContext context)
        {
            _context = context;
        }

        public async Task<List<Seller>> FindAllAsync()
        {
            return await _context.Seller.ToListAsync();
        }

        public async Task InsertAsync(Seller seller)
        {
            //seller.Department = _context.Department.First(); // Para não ficar NULO
            _context.Add(seller);
            await _context.SaveChangesAsync();
        }
        public async Task<Seller?> FindByIdAsync(int sellerId)
        {
            //return _context.Seller.FirstOrDefault(obj => obj.Id == sellerId);
            return await _context.Seller.Include(obj => obj.Department).FirstOrDefaultAsync(obj => obj.Id == sellerId);
            // return _context.Seller.Find(sellerId);

            //var seller = _context.Seller.FirstOrDefault(obj => obj.Id == sellerId);
            //if (seller == null) {
            //    throw new ArgumentException($"Seller with Id {sellerId} not found.");
            //}
            //return seller;
        }

        public async Task RemoveAsync(int sellerId)
        {
            try
            {
                var obj = await _context.Seller.FindAsync(sellerId);
                _context.Seller.Remove(obj);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                throw new IntegrityException("There are sales for this seller ...");
            }
        }

        public async Task UpdateAsync(Seller seller)
        {
            bool hasAny = await _context.Seller.AnyAsync(x => x.Id == seller.Id);
            if (!hasAny)
            {
                throw new NotFoundException("Id not found ...");
            }

            try
            {
                _context.Update(seller);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException e)
            {
                throw new DbConcurrencyException(e.Message);
            }
        }
    }
}