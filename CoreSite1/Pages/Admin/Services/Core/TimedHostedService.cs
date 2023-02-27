using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace CoreSite1.Services
{
    #region snippet1
    internal class TimedHostedService : IHostedService, IDisposable
    {
        private readonly ILogger _logger;
        private Timer _timer;
        //private readonly ManuFashion.Data.ApplicationDbContext _context;
        private readonly IServiceScopeFactory scopeFactory;

        public TimedHostedService(ILogger<TimedHostedService> logger, IServiceScopeFactory scopeFactory)
        {
            _logger = logger;
            //_context = context;
            this.scopeFactory = scopeFactory;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Timed Background Service is starting.");

            _timer = new Timer(DoWork, null, TimeSpan.Zero, 
                TimeSpan.FromMinutes(5));//.FromSeconds(5));

            return Task.CompletedTask;
        }

        private void DoWork(object state)
        {
            _logger.LogInformation("Timed Background Service is working.");
            ///////////work
            ///get DB context
            using (var scope = scopeFactory.CreateScope())
            {
                var _context = scope.ServiceProvider.GetRequiredService<CoreSite1.Data.ApplicationDbContext>();

                //1
                DbFunctions dbFunctions = null;//required for datediff
                //get all cart items not associated with register user or 2 days old.
                var cartItems = _context.Carts.Where(e => !e.CartId.Contains("@") && dbFunctions.DateDiffDay(e.DateCreated, DateTime.Now) > 2 && e.ItemOrderType == Models.OrderType.SalesOrder);//.Where(cart => cart.CartId == ShoppingCartId);

                foreach (var cartItem in cartItems)
                {
                    //log the day time difference
                    _logger.LogInformation("No of days to since the item added to cart:" + dbFunctions.DateDiffDay(cartItem.DateCreated, DateTime.Now).ToString());

                    //remove the cart item
                    _context.Carts.Remove(cartItem);

                    //increase stock value back as the stock which is decrease while "adding to cart".
                    //it would have been not required if the shopping cart would have been converted to an order.
                    if (cartItem.VariantId != null)
                    {
                        var v = _context.Variants.Where(p => p.ProductId == cartItem.ProductId && p.VariantId == cartItem.VariantId).First();
                        _logger.LogInformation("current ProductID =" + v.ProductId.ToString() + ". Current VarinatID =" + v.VariantId.ToString());
                        _logger.LogInformation("current stock =" + v.UnitInStock.ToString());
                        _logger.LogInformation("Stock retured from current cart =" + cartItem.Count.ToString());
                        v.UnitInStock = v.UnitInStock + cartItem.Count;
                        _logger.LogInformation("End stock =" + v.UnitInStock.ToString());

                    }
                    else
                    {
                        var v = _context.Variants.Where(p => p.ProductId == cartItem.ProductId && p.IsDefaulProduct == true).First();
                        _logger.LogInformation("current ProductID =" + v.ProductId.ToString() + ". Current VarinatID =" + v.VariantId.ToString());
                        _logger.LogInformation("current stock =" + v.UnitInStock.ToString());
                        _logger.LogInformation("Stock retured from current cart =" + cartItem.Count.ToString());
                        v.UnitInStock = v.UnitInStock + cartItem.Count;
                        _logger.LogInformation("End stock =" + v.UnitInStock.ToString());
                    }

                   
                }

                _context.SaveChanges();

                //2
                //write code to delete order which are 3 or more years old.
                //var orders = _context.Orders.Where(e => dbFunctions.DateDiffYear(e.OrderDate, DateTime.Now) > 3);//.Where(cart => cart.CartId == ShoppingCartId);

                //foreach (var order in orders)
                //{
                //    //log the day time difference
                //    _logger.LogInformation("No of days to since the item added to cart:" + dbFunctions.DateDiffDay(cartItem.DateCreated, DateTime.Now).ToString());

                //    //remove the cart item
                //    _context.Orders.Remove(order);
                //}
                //_context.SaveChanges();

                //3
                //write code to update order staus to closed after 15 or 30(product return time) days of order completd.
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Timed Background Service is stopping.");

            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
    #endregion
}
