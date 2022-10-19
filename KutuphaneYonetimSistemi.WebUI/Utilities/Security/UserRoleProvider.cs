using System;
using System.Linq;
using System.Web.Security;
using LibraryManagementSystem.WebUI.Models.EntityFramework;

namespace LibraryManagementSystem.WebUI.Utilities.Security {
    public class UserRoleProvider : RoleProvider {
        public static int UserId;
        public static string Name;
        public static DateTime CurrentDay = DateTime.Now;
        public override bool IsUserInRole(string username, string roleName) {
            throw new NotImplementedException();//todo
        }

        public override string[] GetRolesForUser(string username) {
            LibraryContext db = new LibraryContext();
            var user = db.Users.FirstOrDefault(x => x.Username == username);
            var result = from x in db.Users
                         where x.Username == username
                         select x;
            foreach (var item in result) {
                UserId = item.Id;
                Name = item.Name + " " + item.Surname;
            }
            return new string[] { user.Role.ToString() };
        }

        public override void CreateRole(string roleName) {
            throw new NotImplementedException();//todo
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole) {
            throw new NotImplementedException();//todo
        }

        public override bool RoleExists(string roleName) {
            throw new NotImplementedException();//todo
        }

        public override void AddUsersToRoles(string[] usernames, string[] roleNames) {
            throw new NotImplementedException();//todo
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames) {
            throw new NotImplementedException();//todo
        }

        public override string[] GetUsersInRole(string roleName) {
            throw new NotImplementedException();//todo
        }

        public override string[] GetAllRoles() {
            throw new NotImplementedException();//todo
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch) {
            throw new NotImplementedException();//todo
        }

        public override string ApplicationName { get; set; }
    }
}