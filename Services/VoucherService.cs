using Spoonful.Models;
namespace Spoonful.Services
{
    public class VoucherService
    {
        private readonly AuthDbContext _context;

        public VoucherService(AuthDbContext context)
        {
            _context = context;
        }

        public Vouchers? GetVoucherByCode(string code)
        {
            Vouchers? voucher = _context.Rewards.FirstOrDefault(x => x.voucherCode.Equals(code));
            return voucher;
        }

        public Vouchers? GetVoucherById(int id)
        {
            Vouchers? voucher = _context.Rewards.FirstOrDefault(x => x.Id.Equals(id));
            return voucher;
        }

        public List<Vouchers> GetAll()
        {
            return _context.Rewards.OrderBy(v => v.Id).ToList();
        }

        public void AddVoucher(Vouchers voucher)
        {
            _context.Rewards.Add(voucher);
            _context.SaveChanges();
        }
        public void UpdateVoucher(Vouchers voucher)
        {
            _context.Rewards.Update(voucher);
            _context.SaveChanges();
        }

        public void DeleteVoucher(Vouchers voucher)
        {
            _context.Rewards.Remove(voucher);
            _context.SaveChanges();
        }
    }
}
