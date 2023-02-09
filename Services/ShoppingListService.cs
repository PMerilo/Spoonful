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
        public List<ShoppingEntry> GetAll()
        {
            return _context.Shopping.OrderBy(m => m.Id).ToList();
        }
        public List<ShoppingEntry> GetAllByName()
        {
            return _context.Shopping.OrderBy(m => m.Name).ToList();
        }
        public List<ShoppingEntry> GetAllByCat()
        {
            return _context.Shopping.OrderBy(m => m.Category).ToList();
        }   
        public List<ShoppingEntry> GetAllByPurchase()
        {
            return _context.Shopping.OrderBy(m => m.isBought).ThenBy(m => m.Category).ToList();
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

