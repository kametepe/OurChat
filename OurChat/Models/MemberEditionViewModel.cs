
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace OurChat.Models
{
    public class MemberEditionViewModel
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Fname { get; set; }
        public string Lname { get; set; }
        public string PicturePath { get; set; }
        public bool IsLive { get; set; }
        public bool IsLocked { get; set; }        
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Password { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }
        public string Bio { get; set; }        
        public string Position { get; set; }
        public string UniqueID { get; set; }
        public string PicType { get; set; }
        public byte[] ExistingPicture { get; set; }
        public bool IsAdmin { get; set; }
        [DataType(DataType.Upload)]
        public IFormFile Picture { get; set; }       
        
    }
}
