using System;
using System.Collections.Generic;
using System.Text;
using DataRepository.Models;

namespace DataRepository
{
    public class AccountRepository : IAccountRepository
    {
        private TodoDBContext _context;

        public AccountRepository(TodoDBContext context)
        {
            _context = context;
        }

        public void AddUser(User user)
        {
            _context.Add(user);
            _context.SaveChanges();
        }

        public IEnumerable<User> GetAllUsers()
        {
            return _context.Users;
        }
    }
}
