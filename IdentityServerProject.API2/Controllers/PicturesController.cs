using IdentityServerProject.API2.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace IdentityServerProject.API2.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class PicturesController : ControllerBase
    {

        [HttpGet]
        public IActionResult GetPictures()
        {
            var pictures = new List<Picture>()
            {
                new Picture(){Id=1,Name="Resim - 1",Url="1.jpg"},
                new Picture(){Id=2,Name="Resim - 2",Url="1.jpg"},
                new Picture(){Id=3,Name="Resim - 3",Url="1.jpg"},
                new Picture(){Id=4,Name="Resim - 4",Url="1.jpg"},
                new Picture(){Id=5,Name="Resim - 5",Url="1.jpg"}
            };

            return Ok(pictures);
        }
    }
}
