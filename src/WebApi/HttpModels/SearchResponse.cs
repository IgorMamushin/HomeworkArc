using Models;

namespace WebApi.HttpModels
{
    public class SearchResponse
    {
        public IReadOnlyCollection<UserDto> Users { get; set; } = new List<UserDto>(0);
    }
}
