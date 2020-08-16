using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using API.Data;
using API.Interfaces;
using API.Entities;
using Application.Errors;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/profiles")]
    [ApiController]
    public class FollowersController : ControllerBase
    {
        private readonly IMeetupRepository _context;
        private readonly IMapper _mapper;
        private readonly IUserAccessor _userAccessor;
        private readonly IProfileReader _profileReader;
    
        public FollowersController(IMeetupRepository context, IMapper mapper, IUserAccessor userAccessor, IProfileReader profileReader)
        {
            _userAccessor = userAccessor;
            _mapper = mapper;
            _context = context;
            _profileReader = profileReader;
        }


        [HttpPost("{username}/follow")]
        public async Task<IActionResult> Follow(string username)
        {

            var observer = await _context.GetUserByName(_userAccessor.GetCurrentUsername());

            var target = await _context.GetUserByName(username);

            if (target == null)
                throw new RestException(HttpStatusCode.NotFound, new { User = "Not found" });

            var following = await _context.GetFollowers(observer.Id, target.Id);

            if (following != null)
                throw new RestException(HttpStatusCode.BadRequest, new { User = "You are already following this user" });

            if (following == null)
            {
                following = new UserFollowing
                {
                    Observer = observer,
                    Target = target
                };

                _context.Add<UserFollowing>(following);
            }

            var success = await _context.SaveAll();

            if (success) return NoContent();

            throw new Exception("Problem saving changes");
        }

        [HttpDelete("{username}/follow")]
        public async Task<IActionResult> Unfollow(string username)
        {
            var observer = await _context.GetUserByName(_userAccessor.GetCurrentUsername());

            var target = await _context.GetUserByName(username);

            if (target == null)
                throw new RestException(HttpStatusCode.NotFound, new { User = "Not found" });

            var following = await _context.GetFollowers(observer.Id, target.Id);

            if (following == null)
                throw new RestException(HttpStatusCode.BadRequest, new { User = "You are not following this user" });

            if (following != null)
            {
                _context.Delete<UserFollowing>(following);
            }

            var success = await _context.SaveAll();

            if (success) return NoContent();

            throw new Exception("Problem saving changes");
        }

        [HttpGet("{username}/follow")]
        public async Task<ActionResult<List<Dtos.Profile>>> GetFollowings(string username, string predicate)
        {
            var userFollowings = new List<UserFollowing>();
            var profiles = new List<Dtos.Profile>();

            //switch (predicate)
            //{
            //    case "followers":
            //        {
            //            userFollowings = await _context.GetUserFollowers(username);

            //            foreach (var follower in userFollowings)
            //            {
            //                profiles.Add(await _profileReader.ReadProfile(follower.Observer.UserName));
            //            }
            //            break;
            //        }
            //    case "following":
            //        {
            //            userFollowings = await _context.GetUserFollowing(username);

            //            foreach (var follower in userFollowings)
            //            {
            //                profiles.Add(await _profileReader.ReadProfile(follower.Target.UserName));
            //            }
            //            break;
            //        }
            //}

            return profiles;
        }



        
    }
}