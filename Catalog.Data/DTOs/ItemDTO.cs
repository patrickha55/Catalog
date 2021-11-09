using System;

namespace Catalog.Data.DTOs
{
    public class ItemDTO : ManageItemDTO
    {
        public Guid Id { get; init; }
        public DateTimeOffset CreatedDate { get; init; }
    }

    public class ManageItemDTO
    {
        public string Name { get; init; }
        public decimal Price { get; init; }
    }
}