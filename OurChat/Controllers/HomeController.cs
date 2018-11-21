using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OurChat.Models;
using OurChat.Services.Interfaces;

namespace OurChat.Controllers
{
    public class HomeController : BaseController
    {
        private IMemberService memberService;
        public HomeController(IMemberService membersrv)
        {
            memberService = membersrv;
        }

        public IActionResult Index()
        {
            var members = memberService.GetAllMembers();          
            return View(members);
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
