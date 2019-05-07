using System;
using System.Collections.Generic;
using DataRepository.Models;


namespace DataRepository
{
    public interface IAccountRepository
    {
        IEnumerable<User> GetAllUsers();

        void AddUser(User user);
    }
}
