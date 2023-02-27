using System.Collections.Generic;
using CoreSite1.Models;
namespace CoreSite1.ViewModels
{
public class ShoppingCartViewModel
{
public List<Cart> CartItems { get; set; }
public decimal CartTotal { get; set; }
}
}