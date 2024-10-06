using DataAccess.Models;
using System.Collections.Generic;

namespace Service.TransferModels.Responses
{
    // Product DTO
    public class ProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public bool Discontinued { get; set; }
        public int Stock { get; set; }
        public double Price { get; set; }
        public List<PropertyDto> Properties { get; set; } = new List<PropertyDto>();  // Include properties

        public static ProductDto FromEntity(PaperApi paperApi)
        {
            return new ProductDto
            {
                Id = paperApi.Id,
                Name = paperApi.Name,
                Discontinued = paperApi.Discontinued,
                Stock = paperApi.Stock,
                Price = paperApi.Price,
                Properties = paperApi.Properties.Select(prop => new PropertyDto
                {
                    Id = prop.Id,
                    PropertyName = prop.PropertyName
                }).ToList()
            };
        }
    }

    // Property DTO
    public class PropertyDto
    {
        public int Id { get; set; }
        public string PropertyName { get; set; }
    }
}