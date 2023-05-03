namespace Models
{
    public class PostDto
    {
        public long Id { get; set; }
        public string UserName { get; set; }

        public string Text { get; set;}
        public DateTime CreatedAt { get; set; }
    }
}
