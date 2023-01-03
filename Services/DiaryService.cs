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
        public List<Diary> GetAll()
        {
            return _context.Entries.OrderBy(m => m.Id).ToList();
        }
        public Diary? GetEntryById(string id)
        {
            Diary? Diary = _context.Entries.FirstOrDefault(
            x => x.Id.Equals(id));
            return Diary;
        }
        public void AddEntry(Diary entry)
        {
            _context.Entries.Add(entry);
            _context.SaveChanges();
        }
        public void UpdateEntry(Diary entry)
        {
            _context.Entries.Update(entry);
            _context.SaveChanges();
        }
        public void DeleteEntry(Diary entry)
        {
            _context.Entries.ExecuteDeleteAsync();
        }
    }
}
