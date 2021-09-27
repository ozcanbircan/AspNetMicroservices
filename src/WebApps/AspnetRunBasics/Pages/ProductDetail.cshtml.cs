﻿using AspnetRunBasics.Models;
using AspnetRunBasics.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Threading.Tasks;

namespace AspnetRunBasics {
	public class ProductDetailModel : PageModel {
		private readonly ICatalogService _catalogService;
		private readonly IBasketService _basketService;

		public ProductDetailModel(ICatalogService catalogService, IBasketService basketService) {
			_catalogService = catalogService ?? throw new ArgumentNullException(nameof(catalogService));
			_basketService = basketService ?? throw new ArgumentNullException(nameof(basketService));
		}

		public CatalogModel Product { get; set; }

		[BindProperty] 
		public string Color { get; set; }

		[BindProperty] 
		public int Quantity { get; set; }

		public async Task<IActionResult> OnGetAsync(string productId) {
			if (productId == null)
				return NotFound();

			Product = await _catalogService.GetCatalog(productId);
			return Product == null ? NotFound() : Page();
		}

		public async Task<IActionResult> OnPostAddToCartAsync(string productId) {
			CatalogModel product = await _catalogService.GetCatalog(productId);

			BasketModel basket = await _basketService.GetBasket("swn");

			basket.Items.Add(new BasketItemModel {
				ProductId = productId,
				ProductName = product.Name,
				Price = product.Price,
				Quantity = Quantity,
				Color = Color
			});

			await _basketService.UpdateBasket(basket);
			return RedirectToPage("Cart");
		}
	}
}