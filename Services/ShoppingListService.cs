using Microsoft.EntityFrameworkCore;
using Spoonful.Models;

namespace Spoonful.Services
{
    public class ShoppingListService
    {
        private readonly AuthDbContext _context;
        public ShoppingListService(AuthDbContext context)
        {
            _context = context;
        }
        public List<ShoppingEntry> GetAll(string userId)
        {
            return _context.Shopping.Where(m => m.userId == userId).OrderBy(m => m.Id).ToList();
        }
        public List<ShoppingEntry> GetAllByName(string userId)
        {
            return _context.Shopping.Where(m => m.userId == userId).OrderBy(m => m.Name).ToList();
        }
        public List<ShoppingEntry> GetAllByCat(string userId)
        {
            return _context.Shopping.Where(m => m.userId == userId).OrderBy(m => m.Category).ToList();
        }   
        public List<ShoppingEntry> GetAllByPurchase(string userId)
        {
            return _context.Shopping.Where(m => m.userId == userId).OrderBy(m => m.isBought).ThenBy(m => m.Category).ToList();
        }
        public ShoppingEntry? GetEntryById(int id)
        {
            ShoppingEntry? shoppingEntry = _context.Shopping.FirstOrDefault(
            x => x.Id.Equals(id));
            return shoppingEntry;
        }
        public void AddEntry(ShoppingEntry shoppingEntry)
        {
            _context.Shopping.Add(shoppingEntry);
            _context.SaveChanges();
        }
        public void UpdateEntry(ShoppingEntry shoppingEntry)
        {
            _context.Shopping.Update(shoppingEntry);
            _context.SaveChanges();
        }
        public void DeleteEntry(ShoppingEntry shoppingEntry)
        {
            _context.Shopping.Remove(shoppingEntry);
            _context.SaveChanges();
        }
    }
}

