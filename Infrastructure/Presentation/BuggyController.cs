using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Presentation
{
    [ApiController]
    [Route("api/[controller]")]
    public class BuggyController : ControllerBase
    {
        [HttpGet("notFound")]
        public IActionResult GetNotFoundRequest() 
        {
            // Code

            return NotFound();
        }


        [HttpGet("servererror")]
        public IActionResult GetServerErrorRequest()
        {
            throw new Exception();
            return Ok();

        }

        [HttpGet(template: "badrequest")]
        public IActionResult GetBadRequest()
        {
            // Code

            return BadRequest();
        }


        [HttpGet(template: "badrequest/{id}/{age}")]
        public IActionResult GetBadRequest(int id, int age)
        {
            // Code

            return BadRequest();
        }



        [HttpGet(template: "unauthorized")]
        public IActionResult GetUnauthorizedRequest()
        {
            // Code

            return Unauthorized();
        }



    }
}
