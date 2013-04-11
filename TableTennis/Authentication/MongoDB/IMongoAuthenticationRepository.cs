namespace GoldSilverWebServer.Authentication.MongoDB
{
    public interface IMongoAuthenticationRepository
    {
        bool ValidateUser(string user, string password);
        void CreateUser(string username, string password);
        string[] GetUserRoles(string username);
        bool DoUserHaveRole(string username, string rolename);
    }
}