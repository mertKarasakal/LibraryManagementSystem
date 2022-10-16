using LibraryManagementSystem.WebUI.Models.EntityFramework;
using System;
using System.Linq;
using System.Web.Security;
namespace LibraryManagementSystem.WebUI.Security
{
    public class PersonelRoleProvider : RoleProvider
    {
        public static int personelId1;
        public static string kayitIsim;
        public static DateTime MevcutGun = DateTime.Now;
        public override bool IsUserInRole(string username, string roleName)
        {
            throw new NotImplementedException();//todo
        }

        public override string[] GetRolesForUser(string username)
        {
            LibraryContext db = new LibraryContext();
            var personel = db.Users.FirstOrDefault(x => x.Username == username);
            var sorgu1 = from x in db.Users
                         where x.Username == username
                         select x;
            foreach (var item in sorgu1)
            {
                personelId1 = item.Id;
                kayitIsim = item.Name + " " + item.Surname;
            }
            return new string[] { personel.Role.ToString() };
        }

        public override void CreateRole(string roleName)
        {
            throw new NotImplementedException();//todo
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            throw new NotImplementedException();//todo
        }

        public override bool RoleExists(string roleName)
        {
            throw new NotImplementedException();//todo
        }

        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();//todo
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();//todo
        }

        public override string[] GetUsersInRole(string roleName)
        {
            throw new NotImplementedException();//todo
        }

        public override string[] GetAllRoles()
        {
            throw new NotImplementedException();//todo
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new NotImplementedException();//todo
        }

        public override string ApplicationName { get; set; }
    }
}