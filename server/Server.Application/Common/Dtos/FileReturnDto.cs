namespace Server.Application.Common.Dtos
{
    public class FileReturnDto
    {
        public string Path { get; set; }
        public string Name { get; set; }
        public string Extension { get; set; }
        public string? PublicId { get; set; }
    }
}
