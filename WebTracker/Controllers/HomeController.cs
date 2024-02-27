using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebTracker.Models;

namespace WebTracker.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpGet]
        public IActionResult Track()
        {
            // Get client IP address
#pragma warning disable CS8602 // Dereference of a possibly null reference.
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
            string ipAddress = HttpContext.Connection.RemoteIpAddress.ToString();

            // Get User Agent
            string userAgent = Request.Headers["User-Agent"].ToString();

            // Get Referrer
            Uri referrer = Request.Headers.Referer.Count > 0 ? new Uri(Request.Headers.Referer.ToString()) : null;
            string referrerUrl = referrer != null ? referrer.ToString() : "No referrer";

            // Get Query String values
            var queryString = Request.Query;
            string queryStringValues = "";
            foreach (var key in queryString.Keys)
            {
                queryStringValues += $"Key: {key}, Value: {queryString[key]}\n";
            }

            string trackingCookieId = Request.Cookies["AmtMktTrkr"];
            if (string.IsNullOrEmpty(trackingCookieId))
            {
                trackingCookieId = Guid.NewGuid().ToString();
                var cookieOptions = new CookieOptions();
                cookieOptions.Expires = DateTime.Now.AddDays(10000);
                cookieOptions.Path = "/";
                Response.Cookies.Append("AmtMktTrkr", trackingCookieId, cookieOptions);
            }

            string eventType = "view";

            var msg = string.Format("IP:{0}, UserAgent:{1}, Referrer:{2}, QueryString:{3}, TrackingCookieId:{4}, EventType:{5}", ipAddress, userAgent, referrerUrl, queryStringValues, trackingCookieId, eventType);
            _logger.Log(LogLevel.Debug, msg);

            return Ok();

#pragma warning restore CS8602 // Dereference of a possibly null reference.
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
        }

        [HttpPost]
        public IActionResult Track(string id)
        {
            // Get client IP address
#pragma warning disable CS8600 // Dereference of a possibly null reference.
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            string ipAddress = HttpContext.Connection.RemoteIpAddress.ToString();

            // Get User Agent
            string userAgent = Request.Headers["User-Agent"].ToString();

            // Get Referrer
            Uri referrer = Request.Headers.Referer.Count > 0 ? new Uri(Request.Headers.Referer.ToString()) : null;
            string referrerUrl = referrer != null ? referrer.ToString() : "No referrer";

            // Get Query String values
            var queryString = HttpContext.Request.Query;
            string queryStringValues = "";
            foreach (var key in queryString.Keys)
            {
                queryStringValues += $"Key: {key}, Value: {queryString[key]}\n";
            }

            string app;
            string eventType = string.Empty;
            string link;
            foreach (string k in Request.Form.Keys)
            {
                switch (k)
                {
                    case "app":
                        app = HttpContext.Request.Form[k];
                        break;
                    case "linkClick":
                        eventType = "click";
                        link = HttpContext.Request.Form[k];
                        break;
                    case "pageView":
                        eventType = "view";
                        link = HttpContext.Request.Form[k];
                        break;
                }
            }

            string trackingCookieId = Request.Cookies["AmtMktTrkr"];
            if (string.IsNullOrEmpty(trackingCookieId))
            {
                trackingCookieId = Guid.NewGuid().ToString();
                var cookieOptions = new CookieOptions
                {
                    Expires = DateTime.Now.AddDays(9999),
                    Path = "/"
                };
                Response.Cookies.Append("AmtMktTrkr", trackingCookieId, cookieOptions);
            }

            var msg = string.Format("IP:{0}, UserAgent:{1}, Referrer:{2}, QueryString:{3}, TrackingCookieId:{4}, EventType:{5}", ipAddress, userAgent, referrerUrl, queryStringValues, trackingCookieId, eventType);
            _logger.Log(LogLevel.Debug, msg);

            return Ok();

#pragma warning restore CS8600 // Dereference of a possibly null reference.
#pragma warning restore CS8602 // Dereference of a possibly null reference.
        }
    }
}
