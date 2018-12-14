using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace OurChat.Controllers
{
    public class SettingsController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}