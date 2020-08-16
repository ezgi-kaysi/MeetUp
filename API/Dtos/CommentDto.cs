using System;

namespace API.Dtos { 
    public class CommentDto
    {
        public Guid Id { get; set; }
        public string Body { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Username { get; set; }
        public string DisplayName { get; set; }
        public string Image { get; set; }
    }

    public class CommentParams
    {
        public string Body { get; set; }
        public Guid ActivityId { get; set; }
        public string Username { get; set; }
    }
}