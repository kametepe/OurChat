using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using OurChat.Entities;
using OurChat.Models;
using OurChat.Services.Interfaces;

namespace OurChat.Controllers
{
    public class DashBoardController : AdminController
    {
        IConfiguration _Configuration;
        IHostingEnvironment _hostEnvironment;
        private readonly IMemberService _memberService;


        public DashBoardController(IConfiguration configuration, IHostingEnvironment hostEnviron, IMemberService memberService)
        {
            _Configuration = configuration;
            _memberService = memberService;
            _hostEnvironment = hostEnviron;
        }

        public IActionResult Index()
        {
           
            return View(GetAllMemberViewModel());
        }

        public IActionResult Create()
        {
            MemberEditionViewModel memberViewModel = new MemberEditionViewModel
            {
                Avatars = GetMemberAvatars(null)
            };          
            return View("Edition", memberViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(MemberEditionViewModel model)
        {

            if (model.Password != model.ConfirmPassword)
            {
                ModelState.AddModelError("password", "Veuillez confirmer votre mot de passe.");
            }
            if (!string.IsNullOrEmpty(model.Password) && (model.Password == model.ConfirmPassword))
            {
                string position = "Member";
                if (model.IsAdmin)
                {
                    position = "Admin";
                }
                Member newMember = new Member() { Title = model.Title, Fname = model.Fname, Lname = model.Lname, IsLive = model.IsLive, IsAdmin = model.IsAdmin, Email = model.Email, Position = position, PicturePath = "dummy-user.png" };
                newMember.Password = Utilities.PasswordHelper.HashPassword(model.Email, model.Password);
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
            }
               

            return View("Edition", model);
        }

        public IActionResult Edit(int id)
        {
            Member member = _memberService.GetMemberByID(id);
            MemberEditionViewModel memberViewModel = new MemberEditionViewModel
            {
                Avatars = GetMemberAvatars(member),
                ID = member.ID,
                Title = member.Title,
                Fname = member.Fname,
                Lname = member.Lname,
                PicturePath = member.PicturePath,
                IsLive = member.IsLive,               
                IsLocked = member.IsLocked,
                Email = member.Email,
                Position = member.Position,
                UniqueID = member.UniqueID,
                PicType = member.PicType,
                ExistingPicture = member.Picture,
                IsAdmin = member.IsAdmin
            };

            return View("Edition", memberViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(MemberEditionViewModel model)
        {
            try
            {
                string position = "Member";
                if (model.IsAdmin)
                {
                    position = "Admin";
                }
                Member newUser = new Member() { ID = model.ID, Title = model.Title, Fname = model.Fname, Lname = model.Lname, IsLive = model.IsLive, IsAdmin = model.IsAdmin, Email = model.Email, Position = position };

                if (!string.IsNullOrEmpty(model.Password) && (model.Password == model.ConfirmPassword))
                {
                    newUser.Password = Utilities.PasswordHelper.HashPassword(model.Email, model.Password);
                }

                Member savedUser = _memberService.UpdateMember(newUser);
                return RedirectToAction(nameof(Index));
            }
            catch
            {

            }
            return View("Edition", model);
        }

        private IEnumerable<MemberEditionViewModel> GetAllMemberViewModel()
        {
            var members = _memberService.GetAllMembers();

            foreach (Member member in members)
            {               

                yield return new MemberEditionViewModel
                {
                    ID = member.ID,
                    Title = member.Title,
                    Fname = member.Fname,
                    Lname = member.Lname,
                    PicturePath = member.PicturePath,
                    IsLive = member.IsLive,
                    IsLocked = member.IsLocked,
                    Email = member.Email,                                        
                    Position = member.Position,
                    UniqueID = member.UniqueID,
                    PicType = member.PicType,
                    ExistingPicture = member.Picture,
                    IsAdmin = member.IsAdmin
                };
            }
        }


        private List<MemberAvatarViewModel> GetMemberAvatars(Member member)
        {
            string avatarPath = string.Concat(_hostEnvironment.ContentRootPath, "\\wwwroot\\images\\chat\\avatar");
            // string[] fileArray = System.IO.Directory.GetFiles(avatarPath, "*.png", System.IO.SearchOption.AllDirectories);
            DirectoryInfo d = new DirectoryInfo(avatarPath);//Assuming Test is your Folder
            FileInfo[] files = d.GetFiles("*.png"); //Getting Text files
            string selectedAvatar = "a02.png";
            if(member != null)
            {
                selectedAvatar = member.PicturePath;
            }
           
            var avatars = new List<MemberAvatarViewModel>();
            avatars = files.Select(fl => new MemberAvatarViewModel { PicturePath = fl.Name, Selected = selectedAvatar == fl.Name }).ToList<MemberAvatarViewModel>();
            return avatars;
        }
    }
}