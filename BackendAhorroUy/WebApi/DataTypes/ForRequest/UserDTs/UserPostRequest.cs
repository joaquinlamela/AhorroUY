using Domain;

namespace WebApi.DataTypes.ForRequest.UserDTs
{
    public class UserPostRequest : ModelBaseForRequest<User, UserPostRequest>
    {
        public string Name { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        public override User ToEntity() => new User()
        {
            Name = Name,
            Username = Username,
            Password = Password
        }; 
    }
}
