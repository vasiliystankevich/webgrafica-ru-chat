using System;
using System.Data.Entity;
using System.Threading.Tasks;

namespace Dal
{
    public interface IDbContext
    {
        int SaveChanges();
        Task<int> SaveChangesAsync();
        void ExecuteTransaction(Action action);
    }

    public interface IDalContext : IDbContext
    {
        IDbSet<MessageModel> Messages { get; set; }
        IDbSet<AccountModel> Accounts { get; set; }
        IDbSet<webpages_Membership> webpages_Membership { get; set; }
        IDbSet<webpages_OAuthMembership> webpages_OAuthMembership { get; set; }
        IDbSet<webpages_Roles> webpages_Roles { get; set; }
        IDbSet<webpages_UsersInRoles> webpages_UsersInRoles { get; set; }
    }
    public class DalContext : DbContext, IDalContext
    {
        public DalContext()
            : base("DefaultConnection")
        {
        }

        public DalContext(string nameOrConnectionString):base(nameOrConnectionString)
        {
        }

        public void ExecuteTransaction(Action action)
        {
            using (var transaction = Database.BeginTransaction())
            {
                try
                {
                    action();
                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        public virtual IDbSet<MessageModel> Messages { get; set; }
        public virtual IDbSet<AccountModel> Accounts { get; set; }
        public virtual IDbSet<webpages_Membership> webpages_Membership { get; set; }
        public virtual IDbSet<webpages_OAuthMembership> webpages_OAuthMembership { get; set; }
        public virtual IDbSet<webpages_Roles> webpages_Roles { get; set; }
        public virtual IDbSet<webpages_UsersInRoles> webpages_UsersInRoles { get; set; }
    }
}
