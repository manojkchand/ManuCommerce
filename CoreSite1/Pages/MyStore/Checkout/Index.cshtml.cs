using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreSite1.Models;
using CoreSite1.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CoreSite1.Pages.MyStore.Checkout
{
    [Authorize]
    public class IndexModel : PageModel
    {
        //public void OnGet()
        //{

        //}
        private readonly CoreSite1.Data.ApplicationDbContext _context;

        public IndexModel(CoreSite1.Data.ApplicationDbContext context)
        {
            _context = context;
        }
        //public IList<CoreSite1.Models.Category> Category { get; set; }
        //public IList<CoreSite1.Models.Address> Addresses { get; set; }
        public ShoppingCartViewModel viewModel { get; set; }
        public SelectList ShippingMethod;

        public IActionResult OnGet()
        {
            //Addresses = _context.Addresses.Where(e => e.AddedBy == User.Identity.Name).ToList();
            //return View(Products.ToList());
            //return View(address);
           // Category = _context.Categorys.ToList();
            //ViewData["AddressID"] = new SelectList(_context.Addresses, "AddressID", "City");
            var cart = CoreSite1.Models.ShoppingCart.GetCart(_context, this.HttpContext);
            //shopping items display
            viewModel = new ShoppingCartViewModel
            {
                CartTotal = cart.GetTotal(),
                CartItems = cart.GetCartItems()

            };
            //showind shipping method
            ShippingMethod = new SelectList(_context.ShippingMethods.AsEnumerable()
                                            .Select(g => new SelectListItem
                                            {
                                                Text = g.Title,
                                                Value = g.Title
                                            }));
            //Check if cart is not empty.
            if (cart.GetCartItems().Count() <= 0)
            {
                return Redirect("~/MyStore/Index");
            }
            return Page();
        }
        [BindProperty]
        public CoreSite1.Models.Address Address { get; set; }
        [BindProperty]
        public CoreSite1.Models.Order Order { get; set; }
        //[BindProperty]
        //public int? aId { get; set; }
        public async Task<IActionResult> OnPostAsync(IFormCollection values)
        {
            //check if cart is not empty
            if (CoreSite1.Models.ShoppingCart.GetCart(_context, this.HttpContext).GetCartItems().Count() <= 0)
            {
                return Page();
            }
            //Model state is not checkd:In case of user uses previously added addrss the Address model is removed from UI .
            //this is not adding address when order is added(EF: Referential integrity feature when multiple EF models are used).
            //EF:as the Order is saved it was adding new address as referential integrity in all cases. " _context.SaveChangesAsync();"
            if (Order.AddressID == 0)
            {
                if (!ModelState.IsValid)
                {
                    return Page();
                }
            }
            await TryUpdateModelAsync(Order);
           
            //if addrssid not avalible add address and save address id.
            //string check="with";
            if (Order.AddressID == 0)
            {
                await TryUpdateModelAsync(Address);
                try
                {
                    // Insted of below if Check if its not ROBOT as manual address entry is tidies.

                    //Check if someone keeps on adding address using orders. if a users orders for more than 50 different address than 
                    //his first Address + all orders assoicated with addrsss is delted --- CHECK 

                    //if (_context.Addresses.Where(e => e.AddedBy == User.Identity.Name).Count() > 50)
                    //{
                    //    //remove the first address from 4 for one user
                    //    //var v = _context.Addresses.Where(e => e.AddedBy == User.Identity.Name).First();
                    //    //_context.Addresses.Remove(v);
                    //    //add the new addresss
                    //    Address.AddedBy = User.Identity.Name;
                    //    Address.AddedDate = DateTime.Now;
                    //    //Save Order
                    //    // _context.Addresses.Add(Address);
                    //    //await _context.SaveChangesAsync();
                    //}
                }
                catch
                {
                    //Invalid - redisplay with errors
                    return Page();
                }
                Order.AddressID = Address.AddressID;
                //_context.Addresses.Add(Address);
            }
            //else
            //{
            //    check = "without";
            //}



            //_context.Orders.Add(Order);
            //await _context.SaveChangesAsync();
           


            try
            {
                    Order.AddedBy = User.Identity.Name;
                    Order.OrderDate = DateTime.Now;
                    //Order.AddressID = int.Parse(aId.ToString());
                    Order.Status = OrderStatus.Pending;

                    Order.TransactionID = "MF" + DateTime.Now.ToFileTime() + GetPreviousOrderID();
                    Order.TrackingID = GetPreviousOrderID().ToString();

                    Order.ShippingMethod = values["ShippingMethod"];
                    Order.ShippingAmount = GetShippingMethodsPrice(values["ShippingMethod"]);
                    //Save Order
                    _context.Orders.Add(Order);
                    //if(check == "without")
                    //{ 
                    //    _context.Addresses.Remove(Address);
                    //}
                    await _context.SaveChangesAsync();

                    //Process the order
                    var cart = CoreSite1.Models.ShoppingCart.GetCart(_context, this.HttpContext);
                    cart.CreateOrder(Order);
                //NOTE: on live use session in place of querystring as QS can be maniputed by custoer
                HttpContext.Session.SetInt32("NewOrderId", Order.OrderId);
                    return RedirectToPage("./Complete");//, new { id = Order.OrderId });
            }
            catch
            {
                //Invalid - redisplay with errors
                return Page();
            }


            //return RedirectToPage("./Index");

            //if (!ModelState.IsValid)
            //{
            //    return Page();
            //}



            //return RedirectToPage("./Index");
        }

        public long GetPreviousOrderID()
        {
            //if (_context.Orders.Any())
            //{
            //    var ShippingMethod = _context.Orders.Last().OrderId;

            //    return ShippingMethod;
            //}
            //else
            //{
                return DateTime.Now.ToFileTime();
            ////}
        }

        public decimal GetShippingMethodsPrice(string Title)
        {
            var ShippingMethod = _context.ShippingMethods.Where(s => s.Title == Title).First().Price;
            return ShippingMethod;
        }
    }
}