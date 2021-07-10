using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Data;
using Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Core.Interfaces;
using Core.Specifications;
using API.Dtos;
using AutoMapper;

namespace API.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class ProductsController : ControllerBase
	{
		private readonly IGenericRepository<Product> productRepo;
		private readonly IGenericRepository<ProductBrand> productBrandRepo;
		private readonly IGenericRepository<ProductType> productTypeRepo;
		private readonly IMapper mapper;

		public ProductsController(IGenericRepository<Product> productRepo,
					IGenericRepository<ProductBrand> productBrandRepo, IGenericRepository<ProductType> productTypeRepo, IMapper mapper)
		{
			this.productRepo = productRepo;
			this.productBrandRepo = productBrandRepo;
			this.productTypeRepo = productTypeRepo;
			this.mapper = mapper;
		}

		[HttpGet]
		public async Task<ActionResult<IReadOnlyList<ProductToReturnDto>>> GetProducts()
		{
			var spec = new ProductsWithTypesAndBrandsSpecification();
			var products = await productRepo.ListAsync(spec);

			return Ok(mapper.Map<IReadOnlyList<ProductToReturnDto>>(products));
		}

		[HttpGet("{id}")]
		public async Task<ActionResult<ProductToReturnDto>> GetProduct(int id)
		{
			var spec = new ProductsWithTypesAndBrandsSpecification(id);
			return Ok(mapper.Map<ProductToReturnDto>(await productRepo.GetEntityWithSpec(spec)));
		}

		[HttpGet("brands")]
		public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetProductBrands()
		{
			return Ok(await productBrandRepo.ListAllAsync());
		}

		[HttpGet("types")]
		public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetProductTypes()
		{
			return Ok(await productTypeRepo.ListAllAsync());
		}
	}
}
