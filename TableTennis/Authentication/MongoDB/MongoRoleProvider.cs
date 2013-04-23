using System;
using System.Web.Security;
using GoldSilverWebServer.Authentication.MongoDB;

namespace TableTennis.Authentication.MongoDB
{
    public class MongoRoleProvider : RoleProvider
    {
        private readonly IMongoAuthenticationRepository _mongoAuthenticationRepository;

        public MongoRoleProvider()
        {
            _mongoAuthenticationRepository = new MongoAuthenticationRepository();
        }
        
        public override bool IsUserInRole(string username, string roleName)
        {
            return _mongoAuthenticationRepository.DoUserHaveRole(username, roleName);
        }

        public override string[] GetRolesForUser(string username)
        {
            return _mongoAuthenticationRepository.GetUserRoles(username);
        }

        public override void CreateRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            throw new NotImplementedException();
        }

        public override bool RoleExists(string roleName)
        {
            throw new NotImplementedException();
        }

        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override string[] GetUsersInRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override string[] GetAllRoles()
        {
            throw new NotImplementedException();
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new NotImplementedException();
        }

        public override string ApplicationName { get; set; }
    }
}