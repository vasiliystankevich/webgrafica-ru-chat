using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.Security;
using Dal;
using log4net;
using Libraries.Core.Backend.Authorization;
using Libraries.Core.Backend.Common;
using Libraries.Core.Backend.WebApi.Repositories;
using Project.Kernel;
using WebGrease.Css.Extensions;
using WebMatrix.WebData;

namespace Modules.Core.Accounts
{
    public interface IIdentityRepository
    {

        void Initialize();
        void CreateDefaultRoles();
        void CreateUser(AccountModel model, string password);
        void DeleteUser(string userName);
        void RemoveUsers(TimeSpan sessionTime);
        IDalContext Context { get; set; }
        ISystemMessagesRepository SystemMessagesRepository { get; set; }
    }
    public class IdentityRepository : BaseRepository, IIdentityRepository
    {
        public IdentityRepository(IDalContext context, ISystemMessagesRepository systemMessagesRepository, IWrapper<ILog> logger) : base(logger)
        {
            Context = context;
            SystemMessagesRepository = systemMessagesRepository;
        }

        public void Initialize()
        {
            try
            {
                WebSecurity.InitializeDatabaseConnection("DefaultConnection", "Accounts", "Id", "AccountName",
                    autoCreateTables: true);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void CreateDefaultRoles()
        {
            var roleProvider = (SimpleRoleProvider)Roles.Provider;
            var roles = new List<string> { ERoles.User };
            roles.ForEach(role => ExistenceRole(roleProvider, role));
        }

        public void CreateUser(AccountModel model, string password)
        {
            CheckAccountName(model.AccountName);
            var confirmToken = WebSecurity.CreateUserAndAccount(model.AccountName, password);
            Roles.AddUserToRole(model.AccountName, model.Role);
            WebSecurity.ConfirmAccount(confirmToken);
            UpdateAccount(model);
        }

        public void DeleteUser(string userName)
        {
            var roles = Roles.GetRolesForUser(userName);
            Roles.RemoveUserFromRoles(userName, roles);
            ((SimpleMembershipProvider)Membership.Provider).DeleteAccount(userName);
            ((SimpleMembershipProvider)Membership.Provider).DeleteUser(userName, true);
            SystemMessagesRepository.Send($"Пользователь {userName} покинул чат.");
        }

        public void RemoveUsers(TimeSpan sessionTime)
        {
            var currentTime = DateTime.UtcNow;
            var deleteUsers = Context.Accounts.ToList().Where(u =>
            {
                var userSessionTimeTicks = currentTime.Ticks - u.LastActivity.Value.Ticks;
                var userSessionTime = new TimeSpan(userSessionTimeTicks);
                return userSessionTime > sessionTime;
            }).Select(u => u.AccountName);
            deleteUsers.ForEach(DeleteUser);
        }

        protected void ExistenceRole(SimpleRoleProvider provider, string role)
        {
            if (!provider.RoleExists(role))
                provider.CreateRole(role);
        }

        private static void CheckAccountName(string accountName)
        {
            if (WebSecurity.UserExists(accountName)) throw new MembershipCreateUserException("The username already exist");
        }

        private void UpdateAccount(AccountModel model)
        {
            var account =
                Context.Accounts.First(
                    x => string.Compare(x.AccountName, model.AccountName, StringComparison.OrdinalIgnoreCase) == 0);
            account.UserId = model.UserId;
            account.Role = model.Role;
            account.IsActivate = model.IsActivate;
            account.LastActivity = model.LastActivity;
            Context.SaveChanges();
        }

        public IDalContext Context { get; set; }
        public ISystemMessagesRepository SystemMessagesRepository { get; set; }
    }
}