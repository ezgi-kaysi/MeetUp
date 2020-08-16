using API.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Data
{
    public interface IMeetupRepository
    {
        void Add<T>(T entity) where T : class;
        void Delete<T>(T entity) where T : class;
        Task<bool> SaveAll();
        Task<IEnumerable<Activity>> GetActivities();

        Task<Activity> GetActivity(Guid id);
        Task<AppUser> GetUserByName(string userName);
        Task<UserActivity> GetUserActivity(Guid activityId, string userId);
        Task<Photo> GetMainPhotoForUser(string userId);
        Task<Photo> GetPhoto(string id);

        Task<bool> UserExists(string username);
        Task<bool> UserEmailExists(string email);
        Task<UserFollowing> GetFollowers(string observerId, string targetId);

        Task<IEnumerable<UserFollowing>> GetUserFollowers(string username);

        Task<IEnumerable<UserFollowing>> GetUserFollowing(string username);

    
    }
}
