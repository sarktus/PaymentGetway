using Microsoft.AspNetCore.Mvc;
using PaymentGetwayDemo.Models;
using Razorpay.Api;

namespace PaymentGetwayDemo.Controllers
{
    public class MyOrder : Controller
    {
        [BindProperty]
        public EntityOrder _OrderDetails { get; set; }
        public IActionResult Index()
        {
            return View();
        }

        public ActionResult CreateOrder(EntityOrder orderDetails)
        {
            
            //string key = "rzp_test_q0lsIzlF1aJWzB";

            //string secret = "5H8NmemiNVldyTWnFT57AC9l";

            Random _random = new Random();
            string transactionId = _random.Next(0, 3000).ToString();

            Dictionary<string, object> input = new Dictionary<string, object>();

            input.Add("amount", Convert.ToDecimal(orderDetails.Amount)*100); // dieser Betrag sollte mit dem Transaktionsbetrag übereinstimmen

            input.Add("currency", "INR");

            input.Add("receipt", transactionId);

            ;

            RazorpayClient client = new RazorpayClient("rzp_test_q0lsIzlF1aJWzB", "5H8NmemiNVldyTWnFT57AC9l");

            Razorpay.Api.Order order = client.Order.Create(input);

            ViewBag.orderId = order["id"].ToString();
            return View("Payment",orderDetails); 
        }

        [HttpPost]
        public ActionResult Payment(string razorpay_payment_id, string razorpay_order_id, string razorpay_signature)
        {
            //string key1 = "rzp_test_q0lsIzlF1aJWzB";

            //string secret1 = "5H8NmemiNVldyTWnFT57AC9l";
            RazorpayClient client = new RazorpayClient("rzp_test_q0lsIzlF1aJWzB","5H8NmemiNVldyTWnFT57AC9l");

            Dictionary<string, string> options = new Dictionary<string, string>();
            options.Add("razorpay_payment_id", razorpay_payment_id);
            options.Add("razorpay_order_id", razorpay_order_id);
            options.Add("razorpay_signature", razorpay_signature);


            Utils.verifyPaymentSignature(options);
             EntityOrder OrderDtl = new EntityOrder();
            OrderDtl.TransactionId = razorpay_payment_id;
            OrderDtl.OrderId = razorpay_order_id;

            return View("PaymentSuccess",OrderDtl);
        }
    }
}
