using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using API.Data;
using API.Dtos;
using API.Entities;
using API.Interfaces;
using Application.Errors;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ActivitiesController : ControllerBase
    {
        private readonly IMeetupRepository _context;
        private readonly IMapper _mapper;
        private readonly IUserAccessor _userAccessor;
        public ActivitiesController(IMeetupRepository context, IMapper mapper, IUserAccessor userAccessor)
        {
            _userAccessor = userAccessor;
            _mapper = mapper;
            _context = context;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<ActivitiesEnvelope>> List()
        {
            var queryable = await _context.GetActivities();
            var activities = queryable.Take(3).ToList();

            var usersToReturn = _mapper.Map<List<Activity>, List<ActivityDto>>(activities);

            return new ActivitiesEnvelope
            {
                Activities = usersToReturn,
                ActivityCount = queryable.Count()
            };
        }

        //[AllowAnonymous]
        //[HttpGet]
        //public async Task<ActionResult<ActivitiesEnvelope>> List(int? limit, int? offset, bool isGoing, bool isHost, DateTime? startDate)
        //{
        //    var queryable = _context.GetActivities().Result.Where(x => x.Date >= startDate).OrderBy(x => x.Date).AsQueryable();

        //    if (isGoing && !isHost)
        //    {
        //        queryable = queryable.Where(x => x.UserActivities.Any(a => a.AppUser.UserName == _userAccessor.GetCurrentUsername()));
        //    }

        //    if (isHost && !isGoing)
        //    {
        //        queryable = queryable.Where(x => x.UserActivities.Any(a => a.AppUser.UserName == _userAccessor.GetCurrentUsername() && a.IsHost));
        //    }

        //    var activities = await queryable
        //        .Skip(offset ?? 0)
        //        .Take(limit ?? 3).ToListAsync();

        //    return new ActivitiesEnvelope
        //    {
        //        Activities = _mapper.Map<List<Activity>, List<ActivityDto>>(activities),
        //        ActivityCount = queryable.Count()
        //    };
        //}

        [AllowAnonymous]
        [HttpGet("{id}")]       
        public async Task<ActionResult<ActivityDto>> Details(Guid id)
        {
            var activity = await _context.GetActivity(id);

            if (activity == null)
                throw new RestException(HttpStatusCode.NotFound, new { Activity = "Not found" });

            var activityToReturn = _mapper.Map<Activity, ActivityDto>(activity);

            return  activityToReturn;
        }


        [HttpPost]
        public async Task<ActionResult> Create([FromQuery]ActivityParams activityParams)
        {           
            var activity = new Activity
            {
                Id = activityParams.Id,
                Title = activityParams.Title,
                Description = activityParams.Description,
                Category = activityParams.Category,
                Date = activityParams.Date.Value,
                City = activityParams.City,
                Venue = activityParams.Venue
            };

            _context.Add<Activity>(activity);

            var user = await _context.GetUserByName(_userAccessor.GetCurrentUsername());

            var attendee = new UserActivity
            {
                AppUser = user,
                Activity = activity,
                IsHost = true,
                DateJoined = DateTime.Now
            };

            _context.Add<UserActivity>(attendee);

            var success = await _context.SaveAll();

            if (success) return NoContent();

            throw new Exception("Problem saving changes");
        }



        [HttpPut("{id}")]
        [Authorize(Policy = "IsActivityHost")]
        public async Task<IActionResult> Edit(Guid id, [FromQuery]ActivityParams request)
        {
            var activity = await _context.GetActivity(id);

            if (activity == null)
                throw new RestException(HttpStatusCode.NotFound, new { Activity = "Not found" });

            activity.Title = request.Title ?? activity.Title;
            activity.Description = request.Description ?? activity.Description;
            activity.Category = request.Category ?? activity.Category;
            activity.Date = request.Date ?? activity.Date;
            activity.City = request.City ?? activity.City;
            activity.Venue = request.Venue ?? activity.Venue;

            var success = await _context.SaveAll();

            if (success) return NoContent();

            throw new Exception("Problem saving changes");
        }


        [HttpDelete("{id}")]
        [Authorize(Policy = "IsActivityHost")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var activity = await _context.GetActivity(id);

            if (activity == null)
                throw new RestException(HttpStatusCode.NotFound, new { Activity = "Not found" });
            
             _context.Delete<Activity>(activity);

            var success = await _context.SaveAll();

            if (success) return NoContent();

            throw new Exception("Problem saving changes");
        }

       

        [HttpPost("{id}/attend")]
        public async Task<IActionResult> Attend(Guid id)
        {
            var activity = await _context.GetActivity(id);

            if (activity == null)
                throw new RestException(HttpStatusCode.NotFound, new { Activity = "Cound not find activity" });

            var user = await _context.GetUserByName(_userAccessor.GetCurrentUsername());

            var attendance = await _context.GetUserActivity(activity.Id, user.Id);

            if (attendance != null)
                throw new RestException(HttpStatusCode.BadRequest,
                    new { Attendance = "Already attending this activity" });

            attendance = new UserActivity
            {
                Activity = activity,
                AppUser = user,
                IsHost = false,
                DateJoined = DateTime.Now
            };

            _context.Add<UserActivity>(attendance);

            var success = await _context.SaveAll();

            if (success) return NoContent();

            throw new Exception("Problem saving changes");
        }


        [HttpDelete("{id}/attend")]
        public async Task<IActionResult> Unattend(Guid id)
        {
            var activity = await _context.GetActivity(id);

            if (activity == null)
                throw new RestException(HttpStatusCode.NotFound, new { Activity = "Cound not find activity" });

            var user = await _context.GetUserByName(_userAccessor.GetCurrentUsername());

            var attendance = await _context.GetUserActivity(activity.Id, user.Id);

            if (attendance == null)
                return  NoContent();

            if (attendance.IsHost)
                throw new RestException(HttpStatusCode.BadRequest, new { Attendance = "You cannot remove yourself as host" });

            _context.Delete<UserActivity>(attendance);

            var success = await _context.SaveAll();

            if (success) return NoContent();

            throw new Exception("Problem saving changes");
        }
    }
}