 using System.Threading.Tasks;
using OurChat.Entities;
using System.Collections.Generic;

namespace OurChat.Services.Interfaces
{
    public interface IMemberService
    {        
        Task<(bool, Member)> ValidateMemberCredentialsAsync(string username, string password);
        Task<Member> FindMemberByMemberAsync(string username, string password);
        List<Member> GetAllMembers();
        Task<bool> UpdateMemberCredentials(string username, string password, string newpassword);
        Member FindMemberByUniqueID(string memberUniqueID);
        Member AddMember(Member member);
        Member UpdateMember(Member member);
    }
}