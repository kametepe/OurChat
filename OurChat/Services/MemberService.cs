using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using OurChat.Services.Interfaces;
using OurChat.Entities;
using OurChat.Repositories.Interfaces;

namespace OurChat.Services
{
    public class MemberService : IMemberService
    {
         private readonly IMemberRepository _repository;

        public MemberService(IMemberRepository repository) => _repository = repository;

        public async Task<(bool, Member)> ValidateMemberCredentialsAsync(string username, string password)
        {
             
            var member = await _repository.FindByMemberAsync(username, password);
            if (member != null)
            {
                return (true, member);
            }

            return (false, null);
        }

        public async Task<Member> FindMemberByMemberAsync(string username, string password) => 
            await _repository.FindByMemberAsync(username, password);

        public List<Member> GetAllMembers()
        {
            return _repository.GetAllMembers();
        }

        public Task<bool> UpdateMemberCredentials(string username, string password, string newpassword)
        {
            return _repository.UpdateMemberCredentials(username, password, newpassword);
        }

    }
}