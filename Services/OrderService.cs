using Spoonful.Models;

namespace Spoonful.Services
{
    public class OrderService
    {
        private readonly AuthDbContext _context;
        public OrderService(AuthDbContext context)
        {
            _context = context;
        }

        public List<OrderDetails> GetAll()
        {
            return _context.OrderDetails.OrderBy(x => x.Id).ToList();
        }

        

        public OrderDetails? GetOrderDetailsById(int id)
        {
            OrderDetails? orderDetails = _context.OrderDetails.FirstOrDefault(x => x.Id.Equals(id));
            return orderDetails;
        }

        public OrderDetails? GetOrderDetailsByUserId(string id)
        {
            OrderDetails? orderDetails = _context.OrderDetails.FirstOrDefault(x => x.userId.Equals(id));
            return orderDetails;
        }

        public void AddOrderDetails(OrderDetails orderDetails)
        {
            _context.OrderDetails.Add(orderDetails);
            _context.SaveChanges();
        }

        public void UpdateOrderDetails(OrderDetails orderDetails)
        {
            _context.Update(orderDetails);
            _context.SaveChanges();
        }

        public void DeleteOrderDetails(OrderDetails orderDetails)
        {
            _context.Remove(orderDetails);
            _context.SaveChanges();
        }
    }
}
