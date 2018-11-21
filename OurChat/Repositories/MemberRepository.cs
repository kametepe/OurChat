using OurChat.Repositories.Interfaces;
using System.Linq;
using System.Threading.Tasks;

using System.Collections.Generic;
using OurChat.Entities;
using Microsoft.EntityFrameworkCore;
using System;

namespace OurChat.Repositories
{
    public class MemberRepository : IMemberRepository
    {
        private readonly OurChatContext _Context;

        public MemberRepository(OurChatContext context) => _Context = context;

        public async Task<Member> FindByMemberAsync(string username, string password) {
            string hashedPassword = OurChat.Utilities.PasswordHelper.HashPassword(username, password);
            var member = await _Context.Members.FirstOrDefaultAsync(m => m.Email == username && m.Password == hashedPassword);
            return member;
        }

        public List<Member> GetAllMembers()
        {
            return  _Context.Members.Where(m => m.IsLive == true).ToList<Member>();
        }
        public async Task<bool> UpdateMemberCredentials(string username, string password, string newpassword)
        {
            bool rslt = false;
            string hashedPassword = Utilities.PasswordHelper.HashPassword(username, password);
            var member = await _Context.Members.FirstOrDefaultAsync(m => m.Email == username && m.Password == hashedPassword);
            if(member != null)
            {
                member.Password = Utilities.PasswordHelper.HashPassword(username, newpassword);
                member.LastLoginDate = DateTime.Now;
                member.LastPasswordChangedDate = DateTime.Now;
                _Context.SaveChanges();
                rslt = true;               
            }
            return rslt;
        }

    }
}