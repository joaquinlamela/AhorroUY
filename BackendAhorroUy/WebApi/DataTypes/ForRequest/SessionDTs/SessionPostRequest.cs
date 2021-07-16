using Domain;

namespace WebApi.DataTypes.ForRequest.SessionDTs
{
    public class SessionPostRequest : ModelBaseForRequest<User, SessionPostRequest>
    {
        public string Username { get; set; }
        public string Password { get; set; }

        public override User ToEntity() => new User()
        {
            Username = Username,
            Password = Password,
        }; 
    }
}
