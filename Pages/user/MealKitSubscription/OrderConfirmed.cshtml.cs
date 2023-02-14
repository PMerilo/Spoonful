using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Spoonful.Models;
using Spoonful.Services;
using System.Text.Encodings.Web;
using System.Text;
using Microsoft.AspNetCore.Authorization;


namespace Spoonful.Pages.user.MealKitSubscription
{
    [Authorize]
    public class OrderConfirmedModel : PageModel
    {
        private readonly UserManager<CustomerUser> _userManager;
        private readonly AuthDbContext _db;
        public readonly IEmailService _emailSender;
        private readonly MealKitService _mealKitService;
        private readonly OrderService _orderService;
        private readonly InvoiceMealKitService _invoiceMealKitService;
        private readonly MealKitSubscriptionLogService _mealKitSubscriptionLogService;
        private IWebHostEnvironment _environment;
        
        private readonly VoucherService _voucherService;



        public OrderConfirmedModel(UserManager<CustomerUser> userManager, AuthDbContext db, MealKitService mealKitService, OrderService orderService, IEmailService emailSender, InvoiceMealKitService invoiceMealKitService, MealKitSubscriptionLogService mealKitSubscriptionLogService, IWebHostEnvironment environment, VoucherService voucherService)
        {
            _userManager = userManager;
            _db = db;
            _mealKitService = mealKitService;
            _orderService = orderService;
            _emailSender = emailSender;
            _invoiceMealKitService = invoiceMealKitService;
            _mealKitSubscriptionLogService = mealKitSubscriptionLogService;
            _environment = environment;
            _voucherService = voucherService;
        }

        public async Task<IActionResult> OnGet(string id, string code)
        {
            var user = await _userManager.GetUserAsync(User);
            MealKit? mealkit = _mealKitService.GetMealKitByUserId(user.Id);
            OrderDetails? orderDetails = _orderService.GetOrderDetailsByUserId(user.Id);
            Vouchers? voucher = _voucherService.GetVoucherByCode(code);
            int val = 100;
            string Vcode = "";
            if (voucher != null)
            {
                val = (int)(val - voucher.discountAmount);
                voucher.Quantity = voucher.Quantity - 1;
                voucher.Used = voucher.Used + 1;
                Vcode = voucher.voucherCode;
                _voucherService.UpdateVoucher(voucher);
            }
            if (mealkit != null && orderDetails != null)
            {
                if(orderDetails.Id == id)
                {
                    if(mealkit.SubscriptionCheck == false && orderDetails.SubscriptionCheck == false)
                    {
                        mealkit.SubscriptionCheck = true;
                        orderDetails.SubscriptionCheck = true;
                        _mealKitService.UpdateMealKit(mealkit);
                        _orderService.UpdateOrderDetails(orderDetails);

                        //var PathToFile = _environment.WebRootPath +Path.DirectorySeparatorChar.ToString(); +"Pages/Templates"
                        var htmlPath = Path.Combine(_environment.ContentRootPath, "Pages/Templates/MealKitSubscriptionEmailTemplate.html");
                        var subject = "Spoonful Meal Kit Subscription";
                        string htmlBody = "";
                        using (StreamReader streamReader = System.IO.File.OpenText(htmlPath))
                        {
                            htmlBody = streamReader.ReadToEnd();
                        }

                        //{0} Name
                        //{1} Menu Preference
                        //{2} Number Of Recipes Per Week
                        //{3} Number Of Servings Per Week
                        //{4} Number Of People Per Week
                        //{5} Cost

                        

                       
                        double serving = 5.00;

                        
                        double totalCost = (double)(serving * mealkit.noOfPeoplePerWeek * mealkit.noOfServingsPerPerson * mealkit.noOfRecipesPerWeek);
                        totalCost = (totalCost / 100) * val;

                        Invoice invoice = new Invoice() { MenuPreference = mealkit.MenuPreference, noOfRecipesPerWeek = mealkit.noOfRecipesPerWeek , noOfPeoplePerWeek = mealkit.noOfPeoplePerWeek, noOfServingsPerPerson = mealkit.noOfServingsPerPerson, Address = orderDetails.Address,OrderDate = orderDetails.OrderDate, OrderTime = orderDetails.OrderTime, Cost = totalCost, Name = user.FirstName + " " + user.LastName, Email = user.Email, userId = user.Id, mealkitId = mealkit.Id, orderDetailsId = orderDetails.Id, DiscountCodeUsed = Vcode};
                        MealKitSubscriptionLog mealKitSubscriptionLog = new MealKitSubscriptionLog() { noOfUsersSubscribed = 1, description = $"{user.UserName} has subscribed to our meal kit plan" };

                        _invoiceMealKitService.AddInvoice(invoice);
                        _mealKitSubscriptionLogService.AddMealKitSubscriptionLog(mealKitSubscriptionLog);

                        var htmlInvoicePath = Path.Combine(_environment.ContentRootPath, "Pages/Templates/Invoice.html");

                        string HtmlInvoiceBody = "";

                        using (StreamReader streamReader = System.IO.File.OpenText(htmlInvoicePath))
                        {
                            HtmlInvoiceBody = streamReader.ReadToEnd();
                        }

                        // {0} Name
                        // {1} Address
                        // {2} Email
                        // {3} Date Of Payment
                        // {4} Menu Preference (Current Plan)
                        // {5} No Of Recipes Per Week
                        // {6} No Of People Per Week
                        // {7} No Of Servings Per Person
                        // {8} Invoice Cost

                        string messageInvoiceBody = string.Format(HtmlInvoiceBody,
                        invoice.Name,
                        invoice.Address,
                        invoice.Name,
                        invoice.Address,
                        invoice.Email,
                        invoice.DateOfPayment,
                        invoice.MenuPreference,
                        invoice.noOfPeoplePerWeek,
                        invoice.noOfPeoplePerWeek,
                        invoice.noOfServingsPerPerson,
                        invoice.Cost,
                        invoice.Cost,
                        invoice.Cost,
                        invoice.Cost
                        );

                        var Renderer = new IronPdf.ChromePdfRenderer();
                        Renderer.RenderingOptions.PaperSize = IronPdf.Rendering.PdfPaperSize.A2;

                        Renderer.RenderingOptions.CssMediaType = IronPdf.Rendering.PdfCssMediaType.Screen;
                        //Renderer.RenderingOptions.PrintHtmlBackgrounds = true;
                       

                        var pdf = Renderer.RenderHtmlAsPdf(messageInvoiceBody);

                        var filename = "Invoice Order ID -" + invoice.Id + ".pdf";
                        var uploadsFolder = "pdfs";
                        var pdfPath = Path.Combine(_environment.ContentRootPath, "wwwroot", uploadsFolder, filename);

                        pdf.SaveAs(pdfPath); // Saves our PdfDocument object as a PDF

                        IFormFile File;
                        
                        using (var stream = System.IO.File.OpenRead(pdfPath))
                        {
                            var memoryStream = new MemoryStream();
                            stream.CopyTo(memoryStream);
                            memoryStream.Seek(0, SeekOrigin.Begin);

                            var file = new FormFile(memoryStream, 0, memoryStream.Length, null, Path.GetFileName(pdfPath));

                            List<IFormFile> myAttachments = new List<IFormFile>();
                            myAttachments.Add(file);

                            string messageBody = string.Format(htmlBody, user.UserName, mealkit.MenuPreference, mealkit.noOfRecipesPerWeek, mealkit.noOfServingsPerPerson, mealkit.noOfPeoplePerWeek, totalCost);
                            // Call Email Service and send
                            _emailSender.SendEmail(
                               user.Email,
                               subject,
                               messageBody,
                               null,
                               myAttachments);
                        }

                        
                        
                        

                       

                        await _db.SaveChangesAsync();
                    }
                    return Page();
                }
                else
                {
                    return Redirect("/user/CurrentMealKitPlan");
                }

                

            }

            return Redirect("/user/CurrentMealKitPlan");


        }
    }
}
