namespace Geen.Core.Domains.Users
{
    public class UserModel
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string ProfileImage { get; set; }
       
        public bool? IsAnonymous { get; set; }
    }
}
