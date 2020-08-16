using System;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using API.Data;
using API.Dtos;
using API.Entities;
using Application.Errors;
using AutoMapper;
using Microsoft.AspNetCore.SignalR;

namespace API.SignalR
{
    public class ChatHub : Hub
    {
        private readonly IMeetupRepository _context;
        private readonly IMapper _mapper;
        public ChatHub(IMeetupRepository context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }
        public async Task SendComment(CommentParams commentParams)
        {
            string username = GetUsername();

            commentParams.Username = username;

            var comment = await AddComment(commentParams);

            await Clients.Group(commentParams.ActivityId.ToString()).SendAsync("ReceiveComment", comment);
        }

        private string GetUsername()
        {
            return Context.User?.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
        }

        public async Task AddToGroup(string groupName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);

            var username = GetUsername();

            await Clients.Group(groupName).SendAsync("Send", $"{username} has joined the group");
        }

        public async Task RemoveFromGroup(string groupName)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);

            var username = GetUsername();

            await Clients.Group(groupName).SendAsync("Send", $"{username} has left the group");
        }

        public async Task<CommentDto> AddComment(CommentParams request)
        {
            var activity = await _context.GetActivity(request.ActivityId);

            if (activity == null)
                throw new RestException(HttpStatusCode.NotFound, new { Activity = "Not found" });

            var user = await _context.GetUserByName(request.Username);

            var comment = new Comment
            {
                Author = user,
                Activity = activity,
                Body = request.Body,
                CreatedAt = DateTime.Now
            };

            activity.Comments.Add(comment);

            var success = await _context.SaveAll();

            if (success) return _mapper.Map<CommentDto>(comment);

            throw new Exception("Problem saving changes");
        }
    }
}