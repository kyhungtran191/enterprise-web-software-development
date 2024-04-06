namespace Server.Application.Common.Dtos.Comments
{
    public class CommentDto
    {
        public string Content { get; set; }
        public DateTime DateCreated { get; set; }
        public string UserName { get; set; }
        public string Avatar { get; set; }

    }
}
