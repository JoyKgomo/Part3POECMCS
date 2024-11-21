using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;

namespace POEPART2CMCSFINAL.Models.Seeders
{
    public class UsersConfiguration : IEntityTypeConfiguration<Users>
    {
        public void Configure(EntityTypeBuilder<Users> builder)
        {
            builder.HasKey(b => b.ID);
            builder.HasData
            (
                new Users
                {
                    ID = 1,
                    Name = "John",
                    username = "john@123.com",
                    password = "password1",
                    role = "Lecturer"
                },
                new Users
                {
                    ID = 3,
                    Name = "Joy",
                    username = "joy@kg.com",
                    password = "joykgomo",
                    role = "HR"
                },
                  new Users
                  {
                      ID = 2,
                      Name = "Harry",
                      username = "hary@123.com",
                      password = "password2",
                      role = "Programme Coordinator"
                  },
                  new Users
                  {
                      ID = 4,
                      Name = "Sam",
                      username = "sam@123.com",
                      password ="password3",
                      role = "Academic Manager"
                  }
                  );
        }
    }
}

