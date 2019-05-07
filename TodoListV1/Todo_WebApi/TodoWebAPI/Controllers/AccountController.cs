using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataRepository;
using DataRepository.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace TodoWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private IAccountRepository _repo;

        public AccountController(IAccountRepository accountRepository)
        {
            _repo = accountRepository;
        }

        [HttpPost]
        [Route("GetLoginUser")]
        public ActionResult<User> GetLoginUser([FromUri]LoginJsonRequest request)
        {
            var userdb = _repo.GetAllUsers().Where(e => e.UserName == request.UserName && e.Password == request.Password).FirstOrDefault();
            return userdb;
        }

        [HttpPost]
        [Route("Register")]
        public void Register([FromUri]RegisterJsonRequest request)
        {
            int userdblast = _repo.GetAllUsers().Count() > 0 ? _repo.GetAllUsers().Max(e => e.UserId) : 0;

            var userdb = new User()
            {
                UserId = userdblast + 1,
                UserName = request.UserName,
                Password  = request.Password
            };

            _repo.AddUser(userdb);
        }

        public class LoginJsonRequest
        {
            public int UserId { get; set; }
            public string UserName { get; set; }
            public string Password { get; set; }
            public bool RememberMe { get; set; }
        }

        public class RegisterJsonRequest
        {
            public string UserName { get; set; }
            public string Password { get; set; }

        }
    }
}