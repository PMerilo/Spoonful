﻿using Spoonful.Models;

namespace Spoonful.Services
{
    public class MealOrderService
    {
        private readonly AuthDbContext _context;
        public MealOrderService(AuthDbContext context)
        {
            _context = context;
        }
        public List<Order> GetAll()
        {
            return _context.Order.OrderBy(x => x.Id).ToList();
        }

        public Order? GetOrderId(int id)
        {
            Order? order = _context.Order.FirstOrDefault(x => x.Id.Equals(id));
            return order;
        }


        public Order? GetOrderByName(string name)
        {
            Order? order = _context.Order.FirstOrDefault(x => x.Name.Equals(name));
            return order;
        }

        public void AddOrder(Order order)
        {
            _context.Order.Add(order);
            _context.SaveChanges();
        }

        public void DeleteOrder(Order order)
        {
            _context.Remove(order);
            _context.SaveChanges();
        }
    }
}
