namespace WebApi.HttpModels
{
    public class FeedRequest
    {
        public int? Offset { get; set; } = 0;
        public int? Limit { get; set; } = 10;
    }
}
