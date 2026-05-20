using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TableStore.Models;

namespace TableStore.Controllers
{
    [Authorize]
    public class OrdersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public OrdersController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Orders/Checkout?productId=1
        public async Task<IActionResult> Checkout(int productId)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product == null)
            {
                return NotFound();
            }

            var user = await _userManager.GetUserAsync(User);
            
            var order = new Order
            {
                FullName = user?.FullName,
                Address = user?.Address,
                PhoneNumber = user?.PhoneNumber,
                PaymentMethod = "Thanh toán khi nhận hàng",
                OrderDetails = new List<OrderDetail>
                {
                    new OrderDetail
                    {
                        ProductId = product.Id,
                        Product = product,
                        Quantity = 1,
                        Price = product.Price
                    }
                },
                TotalAmount = product.Price
            };

            return View(order);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ProcessCheckout([Bind("FullName,PhoneNumber,Address,PaymentMethod")] Order order, int productId)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product == null)
            {
                return NotFound();
            }

            ModelState.Remove("UserId");
            ModelState.Remove("User");

            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                
                order.UserId = user.Id;
                order.OrderDate = DateTime.Now;
                order.TotalAmount = product.Price;
                
                order.OrderDetails = new List<OrderDetail>
                {
                    new OrderDetail
                    {
                        ProductId = product.Id,
                        Quantity = 1,
                        Price = product.Price
                    }
                };

                _context.Orders.Add(order);
                await _context.SaveChangesAsync();

                return RedirectToAction("Invoice", new { id = order.Id });
            }
            
            order.OrderDetails = new List<OrderDetail>
            {
                new OrderDetail
                {
                    ProductId = product.Id,
                    Product = product,
                    Quantity = 1,
                    Price = product.Price
                }
            };
            order.TotalAmount = product.Price;
            
            return View("Checkout", order);
        }

        public async Task<IActionResult> Invoice(int id)
        {
            var order = await _context.Orders
                .Include(o => o.OrderDetails)
                .ThenInclude(od => od.Product)
                .FirstOrDefaultAsync(o => o.Id == id && o.UserId == _userManager.GetUserId(User));

            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }
    }
}
