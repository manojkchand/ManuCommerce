using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using CoreSite1.Data;

namespace CoreSite1.Models
{
    public partial class ShoppingCart
    {
        public const string CartSessionKey = "CartId";
        string ShoppingCartId { get; set; }
        public static ApplicationDbContext storeDB;

        public ShoppingCart(ApplicationDbContext context)
        {
            storeDB = context;
        }

        //public ShoppingCart() { }
        //
        public static ShoppingCart GetCart(ApplicationDbContext aDBc, HttpContext context)
        {
            storeDB = aDBc;
            var cart = new ShoppingCart(storeDB);
            cart.ShoppingCartId = cart.GetCartId(context);
            return cart;
        }
        // Helper method to simplify shopping cart calls
        public static ShoppingCart GetCart(Controller controller, ApplicationDbContext Adbc)
        {
            return GetCart(Adbc, controller.HttpContext);
        }
        public void AddToCart(Product Product, Variant variant = null)
        {
            if (variant == null)
            {
                //get default varinat//if stock is maintaind in product table below line is not required
                var v = storeDB.Variants.Where(p => p.ProductId == Product.ProductId && p.IsDefaulProduct == true).First();
                // Get the matching cart and Product instances
                var cartItem = storeDB.Carts.SingleOrDefault(
                c => c.CartId == ShoppingCartId
                && c.ProductId == Product.ProductId
                && c.VariantId == v.VariantId);//added extra for vaiant entity
                if (cartItem == null)
                {
                    // Create a new cart item if no cart item exists
                    cartItem = new Cart
                    {
                        ProductId = Product.ProductId,
                        VariantId = v.VariantId,
                        CartId = ShoppingCartId,
                        Count = 1,
                        DateCreated = DateTime.Now
                    };
                    storeDB.Carts.Add(cartItem);
                }
                else
                {
                    // If the item does exist in the cart, then add one to the quantity
                    cartItem.Count++;
                }
                // Save changes
                storeDB.SaveChanges();
                //update stock in variant table even for single product without varinat

                v.UnitInStock--;

                ////update stock in product table if you dont want the variant table at all
                //var v = storeDB.Products.Where(p => p.ProductId == Product.ProductId).First();
                //v.StockOfNonVariant--;
                //// Save changes
                storeDB.SaveChanges();

            }
            else//product have variant
            {
                // Get the matching cart and Product instances
                var cartItem = storeDB.Carts.SingleOrDefault(
                c => c.CartId == ShoppingCartId
                && c.ProductId == Product.ProductId
                && c.VariantId == variant.VariantId);//added extra for vaiant entity
                if (cartItem == null)
                {
                    // Create a new cart item if no cart item exists
                    cartItem = new Cart
                    {
                        ProductId = Product.ProductId,
                        VariantId = variant.VariantId,
                        CartId = ShoppingCartId,
                        Count = 1,
                        DateCreated = DateTime.Now
                    };
                    storeDB.Carts.Add(cartItem);
                }
                else
                {
                    // If the item does exist in the cart, then add one to the quantity
                    cartItem.Count++;
                }
                // Save changes
                storeDB.SaveChanges();
                //update stock in variant table
                var v = storeDB.Variants.Where(p => p.VariantId == variant.VariantId).First();
                v.UnitInStock--;
                // Save changes
                storeDB.SaveChanges();
            }
        }
        public void AddToPOCart(Product Product, Variant variant = null)
        {
            if (variant == null)
            {
                //get default varinat//if stock is maintaind in product table below line is not required
                var v = storeDB.Variants.Where(p => p.ProductId == Product.ProductId && p.IsDefaulProduct == true).First();
                // Get the matching cart and Product instances
                var cartItem = storeDB.Carts.SingleOrDefault(
                c => c.CartId == ShoppingCartId
                && c.ProductId == Product.ProductId
                && c.VariantId == v.VariantId);//added extra for vaiant entity
                if (cartItem == null)
                {
                    // Create a new cart item if no cart item exists
                    cartItem = new Cart
                    {
                        ProductId = Product.ProductId,
                        VariantId = v.VariantId,
                        CartId = ShoppingCartId,
                        Count = 1,
                        DateCreated = DateTime.Now
                    };
                    storeDB.Carts.Add(cartItem);
                }
                else
                {
                    // If the item does exist in the cart, then add one to the quantity
                    cartItem.Count++;
                }
                // Save changes
                storeDB.SaveChanges();
                //update stock in variant table even for single product without varinat

                //v.UnitInStock--;

                ////update stock in product table if you dont want the variant table at all
                //var v = storeDB.Products.Where(p => p.ProductId == Product.ProductId).First();
                //v.StockOfNonVariant--;


                //// Save changes
                //storeDB.SaveChanges();

            }
            else//product have variant
            {
                // Get the matching cart and Product instances
                var cartItem = storeDB.Carts.SingleOrDefault(
                c => c.CartId == ShoppingCartId
                && c.ProductId == Product.ProductId
                && c.VariantId == variant.VariantId);//added extra for vaiant entity
                if (cartItem == null)
                {
                    // Create a new cart item if no cart item exists
                    cartItem = new Cart
                    {
                        ProductId = Product.ProductId,
                        VariantId = variant.VariantId,
                        CartId = ShoppingCartId,
                        Count = 1,
                        DateCreated = DateTime.Now
                    };
                    storeDB.Carts.Add(cartItem);
                }
                else
                {
                    // If the item does exist in the cart, then add one to the quantity
                    cartItem.Count++;
                }
                // Save changes
                storeDB.SaveChanges();


                //////update stock in variant table
                ////var v = storeDB.Variants.Where(p => p.VariantId == variant.VariantId).First();
                ////v.UnitInStock--;
                ////// Save changes
                ////storeDB.SaveChanges();
            }
        }
        public int RemoveFromCart(int id, ApplicationDbContext aDBc)
        {
            // Get the cart
            storeDB = aDBc;
            var cartItem = storeDB.Carts.Single(
            cart => cart.CartId == ShoppingCartId
            && cart.RecordId == id);

            int itemCount = 0;
            if (cartItem != null)
            {
                if (cartItem.Count > 1)
                {
                    cartItem.Count--;
                    itemCount = cartItem.Count;
                }
                else
                {
                    storeDB.Carts.Remove(cartItem);
                }
                // Save changes
                storeDB.SaveChanges();
            }
            return itemCount;
        }
        public void EmptyCart()
        {
            var cartItems = storeDB.Carts.Where(cart => cart.CartId == ShoppingCartId);
            foreach (var cartItem in cartItems)
            {
                storeDB.Carts.Remove(cartItem);
            }
            // Save changes
            storeDB.SaveChanges();
        }
        public List<Cart> GetCartItems()
        {
            var v = storeDB.Carts.Where(cart => cart.CartId == ShoppingCartId).ToList();

            foreach (var v1 in v)
            {
                var addedproduct = storeDB.Products
                .Single(Product => Product.ProductId == v1.ProductId);
                v1.Product = addedproduct;
                //added extra for vaiant entity
                var addedvariant = storeDB.Variants
                .Single(Variant => Variant.VariantId == v1.VariantId);
                v1.Variant = addedvariant;
            }

            return storeDB.Carts.Where(cart => cart.CartId == ShoppingCartId).ToList();

        }
        public int GetCount()
        {
            // Get the count of each item in the cart and sum them up
            int? count = (from cartItems in storeDB.Carts
                          where cartItems.CartId == ShoppingCartId
                          select (int?)cartItems.Count).Sum();
            // Return 0 if all entries are null
            return count ?? 0;
        }
        public decimal GetTotal()
        {
            // Multiply Product price by count of that Product to get //, 2, MidpointRounding.ToEven
            // the current price for each of those Products in the cart
            // sum all Product price totals to get the cart total
            decimal? total = (from cartItems in storeDB.Carts
                              where cartItems.CartId == ShoppingCartId
                              select (int?)cartItems.Count *
             (cartItems.Product.Discount == 0 ? cartItems.Product.Price : (cartItems.Product.Price - Math.Round(cartItems.Product.Price * (cartItems.Product.Discount / 100m))))
             ).Sum();

            //@(item.Product.Discount == 0 ? item.Product.Price : (item.Product.Price - Math.Round(item.Product.Price * (item.Product.Discount / 100m), 2, MidpointRounding.ToEven)))






            return total ?? decimal.Zero;
        }
        public decimal GetPOTotal()
        {
            // Multiply Product price by count of that Product to get //, 2, MidpointRounding.ToEven
            // the current price for each of those Products in the cart
            // sum all Product price totals to get the cart total
            decimal? total = (from cartItems in storeDB.Carts
                              where cartItems.CartId == ShoppingCartId
                              select (int?)cartItems.Count *
             (cartItems.Product.CostPrice)).Sum();

            return total ?? decimal.Zero;
        }
        public int CreateOrder(Order order)
        {
            decimal orderTotal = 0;
            var cartItems = GetCartItems();
            // Iterate over the items in the cart, adding the order details for each
            foreach (var item in cartItems)
            {
                //calculate price
                Decimal finalprice;
                if (item.Product.Discount == 0)
                {
                    finalprice = item.Product.Price;
                }
                //this elseif statement added for PO
                //just incase if cart items are rempoved from session and retrived froDB using productID.
                //otherwise the Product.Price is equal to Cost price in Purchase Orders.
                else if (item.ItemOrderType == OrderType.PurchaseOrder)
                {
                    finalprice = item.Product.CostPrice;
                }
                else
                {
                    ////cartItems.Product.Discount == 0 ? cartItems.Product.Price : ()
                    //    cartItems.Product.Price - Math.Round(cartItems.Product.Price * (cartItems.Product.Discount / 100m))
                    finalprice = item.Product.Price - Math.Round(item.Product.Price * (item.Product.Discount / 100m));
                    //Math.Round(item.Product.Price * (item.Product.Discount / 100m))
                }


                //create orderitem
                int vareid = item.VariantId != null ? (int)item.VariantId : 0;
                var orderDetail = new OrderItem
                {
                    ProductId = item.ProductId,
                    VariantId = vareid,
                    Title = item.Product.Title,
                    OrderId = order.OrderId,
                    FinalUnitPrice = finalprice,
                    Discount = item.Product.Discount,
                    AddedBy = order.AddedBy,
                    AddedDate = DateTime.Now,
                    Quantity = item.Count,
                    SKU = "Not Set",
                    ProductImage = item.Product.ProductArtUrl
                };
                // Set the order total of the shopping cart
                orderTotal += (item.Count * finalprice);
                storeDB.OrderItem.Add(orderDetail);
            }
            // Set the order's total to the orderTotal count
            order.Total = orderTotal;
            // Save the order
            storeDB.SaveChanges();
            // Empty the shopping cart
            EmptyCart();
            // Return the OrderId as the confirmation number
            return order.OrderId;
        }

        public int CreatePOrder(Order order)
        {
            decimal orderTotal = 0;
            var cartItems = GetCartItems();
            // Iterate over the items in the cart, adding the order details for each
            foreach (var item in cartItems)
            {
                //calculate price
                Decimal finalprice;
                //if (item.Product.Discount == 0)
                //{
                //    finalprice = item.Product.Price;
                //}
                ////this elseif statement added for PO
                ////just incase if cart items are rempoved from session and retrived froDB using productID.
                ////otherwise the Product.Price is equal to Cost price in Purchase Orders.
                //else if (item.ItemOrderType == OrderType.PurchaseOrder)
                //{
                    finalprice = item.Product.CostPrice;
                //}
                //else
                //{
                //    ////cartItems.Product.Discount == 0 ? cartItems.Product.Price : ()
                //    //    cartItems.Product.Price - Math.Round(cartItems.Product.Price * (cartItems.Product.Discount / 100m))
                //    finalprice = item.Product.Price - Math.Round(item.Product.Price * (item.Product.Discount / 100m));
                //    //Math.Round(item.Product.Price * (item.Product.Discount / 100m))
                //}


                //create orderitem
                int vareid = item.VariantId != null ? (int)item.VariantId : 0;
                var orderDetail = new OrderItem
                {
                    ProductId = item.ProductId,
                    VariantId = vareid,
                    Title = item.Product.Title,
                    OrderId = order.OrderId,
                    FinalUnitPrice = finalprice,
                    Discount = item.Product.Discount,
                    AddedBy = order.AddedBy,
                    AddedDate = DateTime.Now,
                    Quantity = item.Count,
                    SKU = "Not Set",
                    ProductImage = item.Product.ProductArtUrl
                };
                // Set the order total of the shopping cart
                orderTotal += (item.Count * finalprice);
                storeDB.OrderItem.Add(orderDetail);
            }
            // Set the order's total to the orderTotal count
            order.Total = orderTotal;
            // Save the order
            storeDB.SaveChanges();
            // Empty the shopping cart
            EmptyCart();
            // Return the OrderId as the confirmation number
            return order.OrderId;
        }

        // We're using HttpContextBase to allow access to cookies.
        public string GetCartId(HttpContext context)
        {
            if (context.Session.GetString(CartSessionKey) == null)
            {
                if (!string.IsNullOrWhiteSpace(context.User.Identity.Name))
                {
                    context.Session.SetString(CartSessionKey, context.User.Identity.Name);
                }
                else
                {
                    // Generate a new random GUID using System.Guid class
                    Guid tempCartId = Guid.NewGuid();
                    // Send tempCartId back to client as a cookie
                    context.Session.SetString(CartSessionKey, tempCartId.ToString());
                }
            }
            return context.Session.GetString(CartSessionKey);
        }

        // When a user has logged in, migrate their shopping cart item to
        // be associated with their username
        public void MigrateCart(string userName, ApplicationDbContext aDBc)
        {
            storeDB = aDBc;
            var shoppingCart = storeDB.Carts.Where(c => c.CartId == ShoppingCartId);
            if (shoppingCart.Count() > 0)
            {
                foreach (Cart item in shoppingCart)
                {
                    item.CartId = userName;
                }
                storeDB.SaveChanges();
            }
        }

        //public void MigrateShoppingCart(string UserName, HttpContext context)
        //{
        //    // Associate shopping cart items with logged-in user
        //    var cart = ShoppingCart.GetCart(storeDB, context);
        //    cart.MigrateCart(UserName, storeDB);
        //    //var shoppingCart = storeDB.Carts.Where(c => c.CartId == cart.ShoppingCartId);
        //    //if (shoppingCart.Count() > 0 || shoppingCart != null)
        //    //{
        //
        //    //    foreach (Cart item in shoppingCart)
        //    //    {
        //    //        item.CartId = UserName;
        //    //    }
        //    //}
        //    //storeDB.SaveChanges();
        //    context.Session.SetString(ShoppingCart.CartSessionKey, UserName);
        //}
    }
}