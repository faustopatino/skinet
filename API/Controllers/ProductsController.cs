using Infrastructure.Data;
using Core.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ProductsController : ControllerBase
	{
		private readonly StoreContext context;

		public ProductsController(StoreContext context)
		{
			this.context = context;
		}
		[HttpGet]
		public async Task<ActionResult<List<Product>>> GetProducts()
		{
			var products = await context.Products.ToListAsync();
			return Ok(products);
		}
		[HttpGet("{id}")]
		public async Task<ActionResult<Product>> GetProduct(int id)
		{
			var product = await context.Products.FindAsync(id);
			return Ok(product);
		}
	}
}
