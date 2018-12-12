using OurChat.Entities;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace OurChat.Repositories.Interfaces
{
    public interface IMemberRepository   
    {
        Task<Member> FindByMemberAsync(string username, string password);

        List<Member> GetAllMembers();
        Task<bool> UpdateMemberCredentials(string username, string password, string newpassword);
        Member FindMemberByUniqueID(string memberUniqueID);
    }
}