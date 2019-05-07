using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataRepository.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DataRepository
{
    public class SampleData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new TodoDBContext(serviceProvider.GetRequiredService<DbContextOptions<TodoDBContext>>()))
            {
              
                if (context.Events.Any())
                {
                    return;  
                }

                context.Events.AddRange(
                    new Event
                    {
                        EventId = 1,
                        Description = "Meeting",
                        Date = new DateTime(2019, 05, 10, 10, 0, 0),
                        CreatedOn = new DateTime(2019, 05, 06, 10, 0, 0),
                        UpdatedOn = null,
                        UserId = 1
                    },
                    new Event
                    {
                        EventId = 2,
                        Description = "Appointment",
                        Date = new DateTime(2019, 05, 11, 10, 0, 0),
                        CreatedOn = new DateTime(2019, 05, 11, 10, 0, 0),
                        UpdatedOn = null,
                        UserId = 1
                    },
                    new Event
                    {
                        EventId = 3,
                        Description = "Travel",
                        Date = new DateTime(2019, 05, 12, 10, 0, 0),
                        CreatedOn = new DateTime(2019, 05, 12, 10, 0, 0),
                        UpdatedOn = null,
                        UserId = 1
                    }
                   );
                context.SaveChanges();

                context.Users.AddRange(
                    new User
                    {
                        UserId = 1,
                        UserName = "user",
                        Password = "pass",
                        Events = null
                    });
                context.SaveChanges();
            }
        }
    }
}


