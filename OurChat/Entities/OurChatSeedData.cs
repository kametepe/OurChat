using System;
using System.Collections.Generic;
using System.Linq;

namespace OurChat.Entities
{
    public static class OurChatSeedData
    {
        public static void EnsureSeedData(this OurChatContext db)
        {
            if (!db.Members.Any())
            {
                var members = new List<Member>
                 {
                    new Member { Title ="M", Fname ="Peter", Lname ="Gregor", Position = "Admin", Email ="petergregor@example.com", Password = Utilities.PasswordHelper.HashPassword("petergregor@example.com","letmein") , PicturePath ="danielhardman.png", IsLive = true },
                    new Member { Title ="Mme", Fname ="Jessica", Lname ="Larson", Position = "Membre", Email ="ljessica@example.com", Password = Utilities.PasswordHelper.HashPassword("ljessica@example.com","letmein") , PicturePath ="rachelzane.png", IsLive = true },
                    new Member { Title ="M", Fname ="Han", Lname ="Solo", Position = "Membre", Email ="hansolo@example.com", Password = Utilities.PasswordHelper.HashPassword("hansolo@example.com","letmein") , PicturePath ="haroldgunderson.png", IsLive = true },
                    new Member { Title ="M", Fname ="Matthew", Lname ="John", Position = "Membre", Email ="mathhewjohn@example.com", Password = Utilities.PasswordHelper.HashPassword("mathhewjohn@example.com","letmein") , PicturePath ="mikeross.png", IsLive = true },
                };

                db.Members.AddRange(members);
                db.SaveChanges();
            }

             
 

        }
    }
 }