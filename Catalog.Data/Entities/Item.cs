using System;

namespace Catalog.Data.Entities
{
    public record Item
    {
        public Guid Id { get; set; }
        public string Name { get; init; }
        public decimal Price { get; init; }
        public DateTimeOffset CreatedDate { get; set; }
    }
}