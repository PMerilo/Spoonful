using Spoonful.Models;
namespace Spoonful.Services
{
    public class DeliveryService
    {
        private readonly AuthDbContext _context;

        public DeliveryService(AuthDbContext context)
        {
            _context = context;
        }

        public Stops? GetStopById(int id)
        {
            Stops? stop = _context.Stops.FirstOrDefault(x => x.Id.Equals(id));
            return stop;
        }

        public Delivery? GetDeliveryById(int id)
        {
            Delivery? delivery = _context.Delivery.FirstOrDefault(x => x.Id.Equals(id));
            return delivery;
        }

        public Routes? GetRouteById(int id)
        {
            Routes? route = _context.Route.FirstOrDefault(x => x.Id.Equals(id));
            return route;
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
