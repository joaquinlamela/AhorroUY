using System;

namespace Domain
{
    public class Category
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }

        public Category()
        { }

        public override bool Equals(object obj)
        {
            return obj is Category category &&
                   Name.Equals(category.Name);
        }
    }
}
