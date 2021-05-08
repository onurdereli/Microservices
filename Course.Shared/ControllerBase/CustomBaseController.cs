using System;
using System.Collections.Generic;
using System.Text;
using Course.Shared.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace Course.Shared.ControllerBase
{
    public class CustomBaseController :Microsoft.AspNetCore.Mvc.ControllerBase
    {
        public IActionResult CreateActionResultInstance<T>(Response<T> response)
        {
            return new ObjectResult(response)
            {
                StatusCode = response.StatusCode
            };
        }
    }
}
