namespace Core.Entities
{
    using System.ComponentModel.DataAnnotations;

    public class Product : BaseEntity
    {
        [Required] public string Name { get; set; }
        [Required] public string Description { get; set; }
        public decimal Price { get; set; }
        [Required] public string PictureUrl { get; set; }
        [Required] public string Type { get; set; }
        [Required] public string Brand { get; set; }
        public int QuantityInStock { get; set; }
    }
}