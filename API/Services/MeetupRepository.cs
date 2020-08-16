using API.Dtos;
using API.Entities;
using Application.Errors;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace API.Data
{
    public class MeetupRepository: IMeetupRepository
    {
        private readonly DataContext _context;
        public MeetupRepository(DataContext context)
        {
            _context = context;
        }


        public void Add<T>(T entity) where T : class
        {
            _context.AddAsync(entity);
        }


        // disable async warning - no RemoveAsync available
        public void Delete<T>(T entity) where T : class
        {
            _context.Remove(entity);
        }




        public async Task<IEnumerable<Activity>> GetActivities()
        {
           return await _context.Activities.ToListAsync();
        }
              

        public async Task<Activity> GetActivity(Guid id)
        {
            return await _context.Activities.Where(t => t.Id == id).FirstOrDefaultAsync();
        }

       

        public async Task<AppUser> GetUserByName(string userName)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.UserName == userName);

            return user;
        }

        public async Task<UserActivity> GetUserActivity(Guid activityId, string userId)
        {
            return await _context.UserActivities.SingleOrDefaultAsync(x => x.ActivityId == activityId && x.AppUserId == userId);
         
        }

        public async Task<Photo> GetMainPhotoForUser(string userId)
        {
            return await _context.Photos.Where(u => u.Id == userId).FirstOrDefaultAsync(p => p.IsMain);
        }

        public async Task<Photo> GetPhoto(string id)
        {
            var photo = await _context.Photos.FirstOrDefaultAsync(p => p.Id == id);

            return photo;
        }

        public async Task<bool> UserExists(string username)
        {
            if (await _context.Users.AnyAsync(x => x.UserName == username))
                return true;

            return false;
        }

        public async Task<bool> UserEmailExists(string email)
        {
            if (await _context.Users.AnyAsync(x => x.Email == email))
                return true;

            return false;
        }

        public async Task<UserFollowing> GetFollowers(string observerId , string targetId)
        {
            return await _context.Followings.SingleOrDefaultAsync(x => x.ObserverId == observerId && x.TargetId == targetId);
        }

        public async Task<UserFollowing> GetFollowing(string observerId, string targetId)
        {
            return await _context.Followings.SingleOrDefaultAsync(x => x.ObserverId == observerId && x.TargetId == targetId);
        }

        public async Task<IEnumerable<UserFollowing>> GetUserFollowers(string username)
        {
            return await _context.Followings.Where(x => x.TargetId == username).ToListAsync();
        }

        public async Task<IEnumerable<UserFollowing>> GetUserFollowing(string username)
        {
            return await _context.Followings.Where(x => x.ObserverId == username).ToListAsync();
        }
        
        public async Task<bool> SaveAll()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
