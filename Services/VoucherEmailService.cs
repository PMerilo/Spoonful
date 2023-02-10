using Spoonful.Models;
namespace Spoonful.Services
{
    public class VoucherEmailService
    {
        private readonly AuthDbContext _context;

        public VoucherEmailService(AuthDbContext context)
        {
            _context = context;
        }

        public VoucherEmails? GetVoucherEmailById(int id)
        {
            VoucherEmails? email = _context.VoucherEmail.FirstOrDefault(x => x.Id.Equals(id));
            return email;
        }

        public List<VoucherEmails> GetAll()
        {
            return _context.VoucherEmail.OrderBy(v => v.Id).ToList();
        }

        public void AddVoucherEmail(VoucherEmails email)
        {
            _context.VoucherEmail.Add(email);
            _context.SaveChanges();
        }
        //public void UpdateVoucherEmail(VoucherEmails email)
        //{
        //    _context.VoucherEmail.Update(email);
        //    _context.SaveChanges();
        //}

        //public void DeleteVoucherEmail(VoucherEmails email)
        //{
        //    _context.VoucherEmail.Remove(email);
        //    _context.SaveChanges();
        //}
    }
}
