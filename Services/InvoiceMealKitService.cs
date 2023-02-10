using Spoonful.Models;

namespace Spoonful.Services
{
    public class InvoiceMealKitService
    {
        private readonly AuthDbContext _context;
        public InvoiceMealKitService(AuthDbContext context)
        {
            _context = context;
        }
        public List<Invoice> GetAll()
        {
            return _context.Invoice.OrderBy(x => x.Id).ToList();
        }

        
        public Invoice? GetInvoiceById(string id)
        {
            Invoice? invoice = _context.Invoice.FirstOrDefault(x => x.Id.Equals(id));
            return invoice;
        }
        public Invoice? GetInvoiceByUserId(string userid)
        {
            Invoice? invoice = _context.Invoice.FirstOrDefault(x => x.userId.Equals(userid));
            return invoice;
        }

        public Invoice? GetInvoiceByMealKitId(int mealkitid)
        {
            Invoice? invoice = _context.Invoice.FirstOrDefault(x => x.mealkitId.Equals(mealkitid));
            return invoice;
        }

        public void AddInvoice(Invoice invoice)
        {
            _context.Invoice.Add(invoice);
            _context.SaveChanges();
        }

        public void UpdateInvoice(Invoice invoice)
        {
            _context.Update(invoice);
            _context.SaveChanges();
        }

        public void DeleteInvoice(Invoice invoice)
        {
            _context.Remove(invoice);
            _context.SaveChanges();
        }
    }
}
