using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Course.Shared.Services.Abstract;
using Microsoft.AspNetCore.Http;

namespace Course.Shared.Services.Concrete
{
    public class SharedIdentityService:ISharedIdentityService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SharedIdentityService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string GetUserId => _httpContextAccessor.HttpContext.User.FindFirst("sub").Value;
    }
}
