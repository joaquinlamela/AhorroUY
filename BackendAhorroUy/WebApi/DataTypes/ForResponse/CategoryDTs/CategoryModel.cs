using Domain;
using System;


namespace WebApi.DataTypes.ForResponse.CategoryDTs
{
    public class CategoryModel : ModelBaseForResponse<Category, CategoryModel>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }

        public CategoryModel() { }

        protected override CategoryModel SetModel(Category category)
        {
            Id = category.Id;
            Name = category.Name;
            ImageUrl = category.ImageUrl;
            return this;
        }

        public override bool Equals(object obj)
        {
            return obj is CategoryModel model &&
                   Name.Equals(model.Name);
        }
    }
}
