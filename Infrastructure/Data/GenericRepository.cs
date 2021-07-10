using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
	public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
	{
		private readonly StoreContext context;

		public GenericRepository(StoreContext context)
		{
			this.context = context;
		}

		public async Task<T> GetByIdAsync(int id)
		{
			return await context.Set<T>().FindAsync(id);
		}


		public async Task<IReadOnlyList<T>> ListAllAsync()
		{
			return await context.Set<T>().ToListAsync();
		}

		public async Task<T> GetEntityWithSpec(ISpecification<T> spec)
		{
			var query = AddIncludes(context.Set<T>().AsQueryable(), spec);
			return await query.Where(spec.Criteria).FirstAsync();
		}

		public async Task<IReadOnlyList<T>> ListAsync(ISpecification<T> spec)
		{
			var query = AddIncludes(context.Set<T>().AsQueryable(), spec);
			return await query.ToListAsync();
		}
		private IQueryable<T> AddIncludes(IQueryable<T> query, ISpecification<T> spec)
		{
			return spec.Includes.Aggregate(query, (current, include) => current.Include(include));
		}
	}
}
