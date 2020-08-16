using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using API.Data;
using API.Dtos;
using API.Interfaces;
using Application.Errors;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfilesController : ControllerBase
    {
        private readonly IProfileReader _profileReader;
        private readonly IMeetupRepository _context;
        private readonly IUserAccessor _userAccessor;
        public ProfilesController(IProfileReader profileReader, IMeetupRepository context, IUserAccessor userAccessor)
        {
            _profileReader = profileReader;
            _context = context;
            _userAccessor = userAccessor;
        }

        [HttpGet("{username}")]
        public async Task<ActionResult<Profile>> Get(string username)
        {
            return await _profileReader.ReadProfile(username);
        }

        [HttpPut]
        public async Task<IActionResult> Edit(string DisplayName, string Bio)
        {
            var user = await _context.GetUserByName(_userAccessor.GetCurrentUsername());

            user.DisplayName = DisplayName ?? user.DisplayName;
            user.Bio = Bio ?? user.Bio;

            var success = await _context.SaveAll();

            if (success) return NoContent();

            throw new Exception("Problem saving changes");
        }

        [HttpGet("{username}/activities")]
        public async Task<ActionResult<List<UserActivityDto>>> GetUserActivities(string username, string predicate) 
        {
            var user = await _context.GetUserByName(username);

            if (user == null)
                throw new RestException(HttpStatusCode.NotFound, new { User = "Not found" });

            var queryable = user.UserActivities.OrderBy(a => a.Activity.Date).AsQueryable();

            switch (predicate)
            {
                case "past":
                    queryable = queryable.Where(a => a.Activity.Date <= DateTime.Now);
                    break;
                case "hosting":
                    queryable = queryable.Where(a => a.IsHost);
                    break;
                default:
                    queryable = queryable.Where(a => a.Activity.Date >= DateTime.Now);
                    break;
            }

            var activities = queryable.ToList();
            var activitiesToReturn = new List<UserActivityDto>();

            foreach (var activity in activities)
            {
                var userActivity = new UserActivityDto
                {
                    Id = activity.Activity.Id,
                    Title = activity.Activity.Title,
                    Category = activity.Activity.Category,
                    Date = activity.Activity.Date
                };

                activitiesToReturn.Add(userActivity);
            }

            return activitiesToReturn;
        }
    }
}