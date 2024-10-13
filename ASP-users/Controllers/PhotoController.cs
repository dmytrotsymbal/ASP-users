using ASP_users.Interfaces;
using ASP_users.Models;
using Microsoft.AspNetCore.Mvc;

namespace ASP_users.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PhotoController : ControllerBase
    {

        private readonly IPhotoRepository _photoRepository;

        public PhotoController(IPhotoRepository photoRepository)
        {
            _photoRepository = photoRepository;
        }

        [HttpPost("{userId}")]
        public async Task<IActionResult> AddPhoto(Guid userId, [FromBody] Photo photo)
        {
            try
            {
                await _photoRepository.AddPhoto(userId, photo);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpDelete("{imageID}")]
        public async Task<IActionResult> DeletePhoto(int imageID)
        {
            try
            {
                await _photoRepository.DeletePhoto(imageID);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
