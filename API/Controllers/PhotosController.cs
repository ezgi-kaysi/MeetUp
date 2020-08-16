using System;
using System.Net;
using System.Threading.Tasks;
using API.Data;
using API.Dtos;
using API.Interfaces;
using API.Entities;
using Application.Errors;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PhotosController : ControllerBase
    {
        private readonly IMeetupRepository _context;
        private readonly IUserAccessor _userAccessor;
        private readonly IPhotoAccessor _photoAccessor;

        public PhotosController(IMeetupRepository context, IUserAccessor userAccessor, IPhotoAccessor photoAccessor)
        {
            _userAccessor = userAccessor;
            _context = context;
            _photoAccessor = photoAccessor;
        }

        [HttpPost]
        public async Task<ActionResult<Photo>> Add([FromForm]PhotoParam photoParam)
        {
            var photoUploadResult = _photoAccessor.AddPhoto(photoParam.File);

            var user = await _context.GetUserByName(_userAccessor.GetCurrentUsername());

            var photo = new Photo
            {
                Url = photoUploadResult.Url,
                Id = photoUploadResult.PublicId
            };

            if (!user.Photos.Any(x => x.IsMain))
                photo.IsMain = true;
            
            user.Photos.Add(photo);

            var success = await _context.SaveAll();

            if (success) return photo;

            throw new Exception("Problem saving changes");
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(string id)
        {
            var user = await _context.GetUserByName(_userAccessor.GetCurrentUsername());
            var photo = _context.GetPhoto(id).Result;

            if (photo == null)
                throw new RestException(HttpStatusCode.NotFound, new { Photo = "Not found" });

            if (photo.IsMain)
                throw new RestException(HttpStatusCode.BadRequest, new { Photo = "You cannot delete your main photo" });

            var result = _photoAccessor.DeletePhoto(photo.Id);

            if (result == null)
                throw new Exception("Problem deleting photo");

            user.Photos.Remove(photo);

            var success = await _context.SaveAll();

            if (success) return NoContent();

            throw new Exception("Problem saving changes");
        }

        [HttpPost("{id}/setmain")]
        public async Task<ActionResult> SetMain(string id)
        {
            var user = await _context.GetUserByName(_userAccessor.GetCurrentUsername());

            var photo = _context.GetPhoto(id).Result;

            if (photo == null)
                throw new RestException(HttpStatusCode.NotFound, new { Photo = "Not found" });

            var currentMain = _context.GetMainPhotoForUser(user.Id).Result;

            currentMain.IsMain = false;
            photo.IsMain = true;

            var success = await _context.SaveAll();

            if (success) return NoContent();

            throw new Exception("Problem saving changes");
        }




    }
}