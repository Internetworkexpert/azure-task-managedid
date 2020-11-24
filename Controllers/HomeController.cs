using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using AZ2043_ManagedID.Models;
using Azure.Storage.Queues;


namespace AZ2043_ManagedID.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private QueueClient _queue;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
            string[] scopes = new string[] { "https://storage.azure.com/.default"  };
            string storageAccount = "inemanagedid";
            string queueName = "tasks";

            var uri = new Uri($"https://{storageAccount}.queue.core.windows.net/{queueName}");
            _queue = new QueueClient(uri);
        }

        public async  Task<IActionResult> Index()
        {
            List<string> messageList = new List<string>();
            await _queue.SendMessageAsync($"Connected at {DateTime.UtcNow}");

            var messages = await _queue.PeekMessagesAsync(32);

            foreach(var msg in messages.Value) {
                messageList.Add(msg.Body.ToString());
            }
            return View(messageList);
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
    }
}
