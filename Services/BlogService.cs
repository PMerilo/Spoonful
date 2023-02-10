using Spoonful.Models;

namespace Spoonful.Services
{
    public class BlogService
    {
        private readonly AuthDbContext _context;
        public BlogService(AuthDbContext context)
        {
            _context = context;
        }

        public List<Blog> GetAll()
        {
            return _context.Blog.OrderBy(x => x.Id).ToList();
        }



        public Blog? GetBlogById(int id)
        {
            Blog? blog = _context.Blog.FirstOrDefault(x => x.Id.Equals(id));
            return blog;
        }

        public void AddBlog(Blog blog)
        {
            _context.Blog.Add(blog);
            _context.SaveChanges();
        }

        public void UpdateBlog(Blog blog)
        {
            _context.Update(blog);
            _context.SaveChanges();
        }

        public void DeleteBlog(Blog blog)
        {
            _context.Remove(blog);
            _context.SaveChanges();
        }
    }
}
