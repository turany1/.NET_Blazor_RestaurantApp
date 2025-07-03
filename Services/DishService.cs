using Microsoft.EntityFrameworkCore;
using SmallRestaurantApp.Data;
using SmallRestaurantApp.Models;
using System.Text.RegularExpressions;

namespace SmallRestaurantApp.Services
{
    public class DishService
    {
        private readonly IDbContextFactory<RestaurantContext> _factory;

        public DishService(IDbContextFactory<RestaurantContext> factory)
        {
            _factory = factory;
        }

        public async Task<List<Dish>> GetAllAsync()
        {
            using var context = _factory.CreateDbContext();
            return await context.Dishes.Include(d => d.Comments).ToListAsync();
        }

        public async Task<Dish?> GetByIdAsync(int id)
        {
            using var context = _factory.CreateDbContext();
            return await context.Dishes.Include(d => d.Comments).FirstOrDefaultAsync(d => d.DishId == id);
        }

        public async Task<List<Dish>> SearchAsync(string? query, bool useRegex)
        {
            using var context = _factory.CreateDbContext();
            if (string.IsNullOrEmpty(query))
            {
                return await context.Dishes.ToListAsync();
            }
            if (useRegex)
            {
                var list = await context.Dishes.ToListAsync();
                var results = list.Where(d =>
                {
                    try {
                        return Regex.IsMatch(d.Name ?? "", query, RegexOptions.IgnoreCase) ||
                               Regex.IsMatch(d.Description ?? "", query, RegexOptions.IgnoreCase);
                    } catch {
                        return false;
                    }
                }).ToList();
                return results;
            }
            else
            {
                return await context.Dishes
                    .Where(d => EF.Functions.ILike(d.Name, $"%{query}%"))
                    .ToListAsync();
            }
        }

        public async Task AddOrUpdateAsync(Dish dish)
        {
            using var context = _factory.CreateDbContext();
            if (dish.DishId == 0)
            {
                context.Dishes.Add(dish);
            }
            else
            {
                context.Dishes.Update(dish);
            }
            await context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            using var context = _factory.CreateDbContext();
            var dish = new Dish { DishId = id };
            context.Dishes.Attach(dish);
            context.Dishes.Remove(dish);
            await context.SaveChangesAsync();
        }

        public async Task AddReviewAsync(Comment comment)
        {
            using var context = _factory.CreateDbContext();
            context.Comments.Add(comment);
            await context.SaveChangesAsync();
        }
    }
}