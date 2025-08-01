using SalesWebMVC.Models;
using SalesWebMVC.Models.Enums;

namespace SalesWebMVC.Data
{
    public class SeedService
    {
        SalesWebMVCContext _context;

        public SeedService(SalesWebMVCContext context)
        {
            _context = context;
        }

        public void Seed()
        {
            if (_context.Department.Any() || _context.Seller.Any() || _context.SalesRecord.Any())
            {
                return; // stop o seed
            }

            Department d1 = new Department(1, "Computers");
            Department d2 = new Department(2, "Eletronics");
            Department d3 = new Department(3, "Fashion");
            Department d4 = new Department(4, "Books");

            Seller s1 = new Seller(1, "Juju", "juju@gmail.com", new DateTime(2025, 08, 21), 8900.00, d1);
            Seller s2 = new Seller(2, "Maria", "maria@gmail.com", new DateTime(2023, 07, 19), 4500.00, d1);
            Seller s3 = new Seller(3, "Josy", "josy@gmail.com", new DateTime(2022, 05, 12), 9300.00, d2);
            Seller s4 = new Seller(4, "Paulo", "paulo@gmail.com", new DateTime(2021, 02, 14), 7100.00, d3);
            Seller s5 = new Seller(5, "Heleno", "heleno@gmail.com", new DateTime(2025, 05, 17), 6680.00, d2);
            Seller s6 = new Seller(6, "Antidio", "antidio@gmail.com", new DateTime(2021, 04, 27), 8580.00, d1);

            SalesRecord sr1 = new SalesRecord(1, new DateTime(2025, 01, 17), 589.00, SaleStatus.Billed, s2);
            SalesRecord sr2 = new SalesRecord(2, new DateTime(2023, 04, 22), 589.00, SaleStatus.Billed, s3);
            SalesRecord sr3 = new SalesRecord(3, new DateTime(2022, 03, 13), 589.00, SaleStatus.Billed, s1);
            SalesRecord sr4 = new SalesRecord(4, new DateTime(2021, 04, 25), 589.00, SaleStatus.Billed, s4);
            SalesRecord sr5 = new SalesRecord(5, new DateTime(2025, 05, 04), 589.00, SaleStatus.Billed, s5);
            SalesRecord sr6 = new SalesRecord(6, new DateTime(2021, 07, 19), 589.00, SaleStatus.Billed, s6);
            SalesRecord sr7 = new SalesRecord(7, new DateTime(2025, 03, 21), 589.00, SaleStatus.Billed, s5);
            SalesRecord sr8 = new SalesRecord(8, new DateTime(2023, 08, 24), 589.00, SaleStatus.Billed, s2);
            SalesRecord sr9 = new SalesRecord(9, new DateTime(2022, 03, 29), 589.00, SaleStatus.Billed, s1);
            SalesRecord sr10 = new SalesRecord(10, new DateTime(2021, 01, 16), 589.00, SaleStatus.Billed, s2);

            _context.Department.AddRange(d1, d2, d3, d4);
            _context.Seller.AddRange(s1, s2, s3, s4, s5, s6);
            _context.SalesRecord.AddRange(sr1, sr2, sr3, sr4, sr5, sr6, sr7, sr8, sr9, sr10);

            _context.SaveChanges();
        }
    }
}
