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

        public  Member FindMemberByUniqueID(string memberUniqueID)
        {
            var member =  _Context.Members.FirstOrDefault(m => m.UniqueID == memberUniqueID);
            return member;
        }

        public Member AddMember(Member member)
        {
            _Context.Members.Add(member);
            _Context.SaveChanges();

            return member;
        }        

        public Member UpdateMember(Member member)
        {
            Member existingMember = _Context.Members.FirstOrDefault(m => m.ID == member.ID);
            if (existingMember != null)
            {
              
                existingMember.Title = member.Title;
                existingMember.Fname = member.Fname;
                existingMember.Lname = member.Lname;
                existingMember.IsLive = member.IsLive;
                existingMember.Position = member.Position;
                existingMember.Email = member.Email;
               
                if (member.Picture != null)
                {
                    existingMember.Picture = member.Picture;
                    existingMember.PicType = member.PicType;
                }
                existingMember.UniqueID = member.UniqueID;                
                _Context.SaveChanges();
            }
            return member;
        }


    }
}