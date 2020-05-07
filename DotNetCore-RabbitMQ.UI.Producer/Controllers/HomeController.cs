using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using DotNetCore_RabbitMQ.UI.Producer.Helper;
using DotNetCore_RabbitMQ.UI.Producer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace DotNetCore_RabbitMQ.UI.Producer.Controllers
{
    public class HomeController : Controller
    {
        private readonly IConfiguration _configuration;
        public HomeController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Index(PersonalInfo personalInfo)
        {
            RabbitMqHelper helper = new RabbitMqHelper(_configuration);
            helper.ConnectionRabbitMq(personalInfo);

            return Redirect("/");
        }
    }
}
