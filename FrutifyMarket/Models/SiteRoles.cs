using FrutifyMarket.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace FrutifyMarket.Models
{
    public class SiteRoles : RoleProvider
    {
        public override string ApplicationName { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override void CreateRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            throw new NotImplementedException();
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new NotImplementedException();
        }

        public override string[] GetAllRoles()
        {
            throw new NotImplementedException();
        }

        public override string[] GetRolesForUser(string username)
        {
            using (var context = new ModelDBContext())
            {
                var user = context.Users.FirstOrDefault(u => u.Username == username);
                if (user == null)
                    return new string[] { };

                List<string> roles = new List<string>();

                if (string.Equals(user.Ruolo.Trim(), "Admin", StringComparison.OrdinalIgnoreCase))
                {
                    roles.Add("Admin");
                }
                else if (string.Equals(user.Ruolo.Trim(), "Cliente", StringComparison.OrdinalIgnoreCase))
                {
                    roles.Add("Cliente");
                }
                else
                {
                    roles.Add("User");
                }

                return roles.ToArray();
            }

        }

        public override string[] GetUsersInRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool IsUserInRole(string username, string roleName)
        {
            throw new NotImplementedException();
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override bool RoleExists(string roleName)
        {
            throw new NotImplementedException();
        }
    }
}