﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace OurChat.Controllers
{
    public class SettingsController : AdminController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}