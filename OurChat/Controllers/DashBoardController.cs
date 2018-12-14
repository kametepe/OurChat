using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using OurChat.Entities;
using OurChat.Models;
using OurChat.Services.Interfaces;

namespace OurChat.Controllers
{
    public class DashBoardController : BaseController
    {
        IConfiguration _Configuration;
        private readonly IMemberService _memberService;


        public DashBoardController(IConfiguration configuration, IMemberService memberService)
        {
            _Configuration = configuration;
            _memberService = memberService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Add()
        {
            MemberEditionViewModel memberViewModel = new MemberEditionViewModel();          
            return View("Edition", memberViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(MemberEditionViewModel model)
        {
            Member newMember = new Member() { Title = model.Title, Fname = model.Fname, Lname = model.Lname, IsLive = model.IsLive, Email = model.Email, Position = model.Position };
            newMember.Password = Utilities.PasswordHelper.HashPassword(model.Email, "letmein");
            if (model.Picture != null && model.Picture.Length != 0 && model.Picture.Length < 10485761)
            {
                // string originalFilename = WebUtility.HtmlEncode(Path.GetFileName(model.Picture.FileName));
                string contenttype = model.Picture.ContentType;
                if (contenttype == "image/gif" || contenttype == "image/jpeg" || contenttype == "image/jpg" || contenttype == "image/png")
                {
                    MemoryStream ms = new MemoryStream();
                    model.Picture.CopyTo(ms);
                    newMember.Picture = ms.ToArray();
                    newMember.PicType = contenttype;
                }
            }

            Member member = _memberService.AddMember(newMember);
            if (member != null && member.ID > 0)
            {
                return RedirectToAction("Index", "Dashboard", null);
            }

            return View("Edition", model);
        }
    }
}