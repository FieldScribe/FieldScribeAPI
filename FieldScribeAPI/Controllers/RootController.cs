using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using FieldScribeAPI.Models;

namespace FieldScribeAPI.Controllers
{
    [Route("/")]
    [ApiVersion("1.0")]
    public class RootController : Controller
    { 
        [HttpGet(Name = nameof(GetRoot))]
        public IActionResult GetRoot()
        {
            var response = new RootResponse
            {
                Self = Link.To(nameof(GetRoot)),
                Meets = Link.To(nameof(MeetsController.GetMeetsAsync))
            };

            return Ok(response);
        }
    }
}