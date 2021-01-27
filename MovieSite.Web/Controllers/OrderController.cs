using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MovieSite.Core.Contracts;
using MovieSite.Core.Extensions;
using System;
using System.Text.Encodings.Web;

namespace MovieSite.Web.Controllers
{
    public class OrderController : Controller
    {
        private readonly IEmailSender _emailSender;
        private readonly ILogger<OrderController> _logger;

        public OrderController(ILogger<OrderController> logger, IEmailSender emailSender)
        {
            _logger = logger;
            _emailSender = emailSender;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Order(string email, string imdbId)
        {
            if (ModelState.IsValid && email.IsValidEmail())
            {
                var callbackUrl = Url.Link("Default", new { Controller = "Home", Action = "Information", Id = imdbId });
                var htmlMessage = "<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN\" \"//www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\">" +
                    "<html lang=\"es\" xmlns=\"//www.w3.org/1999/xhtml\" xmlns:v=\"urn:schemas-microsoft-com:vml\" xmlns:o=\"urn:schemas-microsoft-com:office:office\"><head>" +
                    "    <title></title>" +
                    "    <meta charset=\"utf-8\" /> <!-- utf-8 works for most cases -->" +
                    "    <meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\" />" +
                    "    <meta name=\"viewport\" content=\"width=device-width\" /> <!-- Forcing initial-scale shouldn't be necessary -->" +
                    "    <meta http-equiv=\"X-UA-Compatible\" content=\"IE=edge\" /> <!-- Use the latest (edge) version of IE rendering engine -->" +
                    "    <meta name=\"x-apple-disable-message-reformatting\" />  <!-- Disable auto-scale in iOS 10 Mail entirely -->" +
                    $"</head><body><h1>Your Order Completed</h1><a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>Order details</a></body></html>";

                try
                {
                    _emailSender.SendEmail("imdbmoviesite@gmail.com", email, "Order confirmed", htmlMessage);
                    _logger.LogInformation($"Order confirmed. Email sent to {email}");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                    return RedirectToAction("Information", "Home", new { id = imdbId });
                }


                TempData["callbackUrl"] = callbackUrl;
                return RedirectToAction("Index", "Order", new { url = callbackUrl });
            }

            return RedirectToAction("Information", "Home", new { id = imdbId });
        }
    }
}
