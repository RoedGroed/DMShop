using DataAccess.Models;

namespace Service.TransferModels.Responses.Products
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

        public static ProductDto FromEntity(Paper paper)
        {
            return new ProductDto
            {
                Id = paper.Id,
                Name = paper.Name,
                Discontinued = paper.Discontinued,
                Stock = paper.Stock,
                Price = paper.Price,
                Properties = paper.Properties.Select(prop => new PropertyDto
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
        public required string PropertyName { get; set; }
    }
}