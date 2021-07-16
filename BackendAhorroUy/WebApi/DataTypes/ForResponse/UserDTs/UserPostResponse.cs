using Domain;
using System;

namespace WebApi.DataTypes.ForResponse.UserDTs
{
    public class UserPostResponse : ModelBaseForResponse<User, UserPostResponse>
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }

        public UserPostResponse() { }

        protected override UserPostResponse SetModel(User entity)
        {
            Id = entity.Id.ToString();
            Name = entity.Name;
            Username = entity.Username;
            return this;
        }

        public override bool Equals(object obj)
        {
            return obj is UserPostResponse response &&
                   Name.Equals(response.Name) &&
                   Username.Equals(response.Username)&&
                   Id.Equals(response.Id);
        }
    }
}
