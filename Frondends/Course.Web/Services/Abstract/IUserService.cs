using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Course.Web.Models;

namespace Course.Web.Services.Abstract
{
    public interface IUserService
    {
        Task<UserViewModel> GetUsers();
    }
}
