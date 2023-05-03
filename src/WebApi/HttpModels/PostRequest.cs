using System.ComponentModel.DataAnnotations;

namespace WebApi.HttpModels
{
    public class PostRequest
    {
        [Required, MinLength(1)]
        public string Text { get; set; }
    }
}
