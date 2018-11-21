using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OurChat.Entities
{
    public class OurChatContext : DbContext
    {
        public OurChatContext(DbContextOptions<OurChatContext> options)
            : base(options)
        {
            Database.Migrate(); 
        }

        public DbSet<Member> Members { get; set; }

        public DbSet<Message> Messages { get; set; }

        public DbSet<Role> Roles { get; set; }

        public DbSet<Document> Documents { get; set; }      
        
    }

    public class Role
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public bool IsLive { get; set; }
    }



    public class Message
    {
        public int ID { get; set; }
        public string Content { get; set; }
        public string SenderID { get; set; }
        public bool IsLive { get; set; }
        public string ReceipientID { get; set; }
        public DateTime CreationDate { get; set; }
        public int Status { get; set; }
    }

public class Document
{
    public int ID { get; set; }
    public string Title { get; set; }
    public int DocType { get; set; }
    public string Link { get; set; }
    public DateTime CreationDate { get; set; }
    public int MessageID { get; set; }    
    public int Size { get; set; }
    public bool IsLive { get; set; }
    public string UniqueID { get; set; }

        public Document()
        {
            CreationDate = DateTime.Now;
            UniqueID = Guid.NewGuid().ToString();
        }
    }
  
  public class Member
  {
      public int ID { get; set; }
      public string Title { get; set; }
      public string Fname { get; set; }
      public string Lname { get; set; }
      public string PicturePath { get; set; }
      public bool IsLive { get; set; }
      public string Email { get; set; }
      public string Password { get; set; }
      public DateTime CreationDate { get; set; }
      public DateTime LastLoginDate { get; set; }
      public DateTime LastPasswordChangedDate { get; set; }
      public string ResetCode { get; set; }
      public string Position { get; set; }
      public int LoginAttempt { get; set; }
      public bool IsLocked { get; set; }

        public Member()
      {
            CreationDate = DateTime.Now;
            LastLoginDate = CreationDate;
            LastPasswordChangedDate = CreationDate;
      }
  }
 
}
