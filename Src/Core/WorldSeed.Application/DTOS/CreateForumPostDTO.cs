namespace WorldSeed.Application.DTOS
{
    public class CreateForumPostDTO
    {
        public int ForumCategoryThreadId { get; set; }
        public int OwnerId { get; set; }
        public string Content { get; set; }
    }
}
