
using BaSalesManagementApp.Entites.DbSets;
using BaSalesManagementApp.DataAccess.Context;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BaSalesManagementApp.DataAccess.EFCore.Repositories
{
    public class OrderRepository : EFBaseRepository<Order>, IOrderRepository
    {
        private readonly BaSalesManagementAppDbContext _context;
        public OrderRepository(BaSalesManagementAppDbContext context) : base(context)
        {
            _context = context;
        }
        /// <summary>
        /// Orders ile Admin,Product ve Company tablosunu birleştirerek siparişleri listeler. Bu listede Admin bilgisinin ve aktif firmalarınn olduğu siparişlerin de gösterilmesini sağlar.
        /// </summary>
        /// <returns></returns>
        public async Task<List<Order>> GetAllWithAdminAsync()
        {
            
            var orders = await _context.Orders
                .Include(o => o.Admin)
                .Include(p => p.Product)
                .ThenInclude(c => c.Company)    // Company Name & Status Durumu İçin
                .Where(x => x.Status != Core.Enums.Status.Deleted && x.Product.Company.Status != Core.Enums.Status.Deleted) // Silinen Companylerin Orderları Listede Gözükmeyecek...
                .ToListAsync();

            if (orders == null || !orders.Any())
            {
                Console.WriteLine("No orders found.");
            }
            else
            {
                foreach (var order in orders)
                {
                    Console.WriteLine($"Order ID: {order.Id}, Admin: {order.Admin?.FirstName} {order.Admin?.LastName}");
                }
            }
            return orders;
        }
    }
}
