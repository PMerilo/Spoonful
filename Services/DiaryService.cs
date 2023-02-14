using Microsoft.EntityFrameworkCore;
using Spoonful.Models;

namespace Spoonful.Services
{
    public class DiaryService
    {
        private readonly AuthDbContext _context;
        public DiaryService(AuthDbContext context)
        {
            _context = context;
        }
        public List<Diary> GetAll(string userId)
        {
            return _context.Diary.Where(m => m.userId.ToLower() == userId).OrderBy(m => m.Id).ToList();
        }
        public List<Diary> GetAllByName(string userId)
        {
            return _context.Diary.Where(m => m.userId.ToLower() == userId).OrderBy(m => m.Name).ToList();
        }
        public List<Diary> GetAllByCat(string userId)
        {
            return _context.Diary.Where(m => m.userId.ToLower() == userId).OrderBy(m => m.Category).ToList();
        }
        public List<Diary> GetAllByPurchase(string userId)
        {
            return _context.Diary.Where(m => m.userId == userId).OrderBy(m => m.Purchase).ToList();
        }
        public List<Diary> GetAllByExpiry(string userId)
        {
            return _context.Diary.Where(m => m.userId == userId).OrderBy(m => m.Expiry).ToList();
        }

        public List<Diary> GetAllInCategory(string userId, string catName)
        {
            return _context.Diary.Where(m => m.userId == userId).Where(m => m.Category.ToLower() == catName).ToList();
        }

        public List<Diary> GetTodayExpire(string userId, DateTime today)
        {
            return _context.Diary.Where(m => m.userId == userId).Where(m => m.Expiry == today).ToList();
        }
        public Diary? GetEntryById(int id)
        {
            Diary? Diary = _context.Diary.FirstOrDefault(
            x => x.Id.Equals(id));
            return Diary;
        }
        public void AddEntry(Diary entry)
        {
            _context.Diary.Add(entry);
            _context.SaveChanges();
        }
        public void UpdateEntry(Diary entry)
        {
            _context.Diary.Update(entry);
            _context.SaveChanges();
        }
        public void DeleteEntry(Diary entry)
        {
            _context.Diary.Remove(entry);
            _context.SaveChanges();
        }
    }
}
