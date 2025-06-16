namespace WorldSeed.Application.DTOS
{
    public class CreateForumThreadDTO
    {
        public int ForumCategoryId { get; set; }
        public int? OwnerId { get; set; }
        public string Title { get; set; }
    }
}
