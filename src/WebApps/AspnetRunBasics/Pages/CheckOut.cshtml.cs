﻿using AspnetRunBasics.Models;
using AspnetRunBasics.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Threading.Tasks;

namespace AspnetRunBasics {
	public class CheckOutModel : PageModel {
		private readonly IBasketService _basketService;

		public CheckOutModel(IBasketService basketService) {
			_basketService = basketService ?? throw new ArgumentNullException(nameof(basketService));
		}

		[BindProperty] public BasketCheckoutModel Order { get; set; }

		public BasketModel Cart { get; set; } = new BasketModel();

		public async Task<IActionResult> OnGetAsync() {
			Cart = await _basketService.GetBasket("swn");
			return Page();
		}

		public async Task<IActionResult> OnPostCheckOutAsync() {
			Cart = await _basketService.GetBasket("swn");

			if (!ModelState.IsValid)
				return Page();

			Order.UserName = "swn";
			Order.TotalPrice = Cart.TotalPrice;

			await _basketService.CheckoutBasket(Order);

			return RedirectToPage("Confirmation", "OrderSubmitted");
		}
	}
}