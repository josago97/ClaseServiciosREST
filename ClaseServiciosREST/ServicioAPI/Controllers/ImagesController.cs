using Microsoft.AspNetCore.Mvc;
using ServicioAPI.Data.Entities;
using ServicioAPI.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServicioAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private readonly ImageRepository _imageRepository;

        public ImagesController(ImageRepository imageRepository)
        {
            _imageRepository = imageRepository;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            Image image = await _imageRepository.GetByIdAsync(id);
            return File(image.Data, "image/jpeg");
        }
    }
}
