using ElectronicsStore_eCommerce.Models;
using ElectronicsStore_eCommerce.Models.ContextFile;
using ElectronicsStore_eCommerce.Services.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ElectronicsStore_eCommerce.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly StoreDBContext _context;

        public CategoryService(StoreDBContext context)
        {
            _context = context;
        }

        public async Task<List<Category>> GetCategoriesAsync()
        {
            return await _context.Categories.ToListAsync();
        }

        public async Task<Category> GetCategoryByIdAsync(int id)
        {
            return await _context.Categories.FindAsync(id);
        }

        public async Task CreateCategoryAsync(Category category)
        { 

            // Check if the category name is unique
            if (await IsCategoryNameUniqueAsync(category.Name))
            {
                try
                {
                    // Add the category to the context
                    _context.Categories.Add(category);

                    // Save changes to the database
                    await _context.SaveChangesAsync();

                    // Success: Category created
                    // You can optionally return a success message or other information
                }
                catch (DbUpdateException ex)
                {
                    // Example: Rethrow the exception with a custom message
                    throw new ApplicationException("Error creating category. Please try again.");
                }
            }
        }


        public async Task UpdateCategoryAsync(Category category)
        {

            if (await IsCategoryNameUniqueAsync(category.Name, category.CategoryId))
            {
                _context.Update(category);
                await _context.SaveChangesAsync();
            }            
        }

        public async Task DeleteCategoryAsync(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
        }

        private async Task<bool> IsCategoryNameUniqueAsync(string name, int? categoryId = null)
        {
            // Check if the category name is unique
            return await _context.Categories
                .Where(c => c.CategoryId != categoryId && c.Name == name)
                .FirstOrDefaultAsync() == null;
        }
    }
}
