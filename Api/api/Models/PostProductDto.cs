using api.Entities;
using Microsoft.AspNetCore.Http;

namespace api.Models
{
    public class PostPoroductDto
    {
        public Product product {set; get; }
        public IFormFile Image {set; get; }
    }
}