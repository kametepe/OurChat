
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace OurChat.Models
{
    public class MemberAvatarViewModel
    {
        public string PicturePath { get; set; }
        public bool Selected { get; set; }
    }
}
