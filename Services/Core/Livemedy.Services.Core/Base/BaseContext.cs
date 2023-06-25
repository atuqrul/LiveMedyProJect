using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Web;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Livemedy.Core.Base.Entities;

namespace Livemedy.Core.Base
{
    public abstract class BaseContext : DbContext
    {
        private readonly IHttpContextAccessor HttpContextAccessor;
        public BaseContext(DbContextOptions options, IHttpContextAccessor httpContextAccessor) : base(options)
        {
               HttpContextAccessor = httpContextAccessor;
        }

        public override int SaveChanges()
        {
            UpdateAuditEntities();
            UpdateLockUnLockEntities();
            return base.SaveChanges();
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            UpdateAuditEntities();
            UpdateLockUnLockEntities();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            UpdateAuditEntities();
            UpdateLockUnLockEntities();
            return base.SaveChangesAsync(cancellationToken);
        }

        private void UpdateAuditEntities()
        {
            string CurrentUserId = GetCurrenctUser();

            var modifiedEntries = ChangeTracker.Entries()
                .Where(x => x.Entity is EntityBase &&
                            (x.State == EntityState.Added || x.State == EntityState.Modified));


            foreach (var entry in modifiedEntries)
            {

                var entity = (EntityBase)entry.Entity;
                var now = DateTime.Now;

                if (entry.State == EntityState.Added)
                {
                    entity.CreatedDate = now;
                    entity.CreatedBy = CurrentUserId;
                }
                else if (entry.State == EntityState.Deleted && entry.Entity is not IHardDelete)
                {
                    // Varlığı değiştirilmedi olarak ayarlıyoruz.
                    // (tüm varlığı Değiştirildi olarak işaretlersek, her alan güncelleme olarak Db'ye gönderilir)
                    entry.State = EntityState.Unchanged;

                    // Yalnızca IsDeleted alanını güncelleyin - yalnızca bu Db'ye gönderilir
                    entity.IsDelete = true;
                }
                else
                {
                    base.Entry(entity).Property(x => x.CreatedBy).IsModified = false;
                    base.Entry(entity).Property(x => x.CreatedDate).IsModified = false;
                }

                entity.UpdatedDate = now;
                entity.UpdatedBy = CurrentUserId;
            }
        }
        private void UpdateLockUnLockEntities()
        {
            var entry = ChangeTracker.Entries().Where(x => x.Entity is ILockedEntity && x.State == EntityState.Unchanged).FirstOrDefault();
            if (entry == null)
                return;

            var entity = entry.Entity;     
            bool IsLocked = Convert.ToBoolean(entity.GetType().GetProperty("IsLocked").GetValue(entity));
            if (IsLocked)
            {
                entity.GetType().GetProperty("IsLocked").SetValue(entity, true);
                entity.GetType().GetProperty("LockedBy").SetValue(entity, GetCurrenctUser());
            }
            else
            {
                entity.GetType().GetProperty("IsLocked").SetValue(entity, false);
                entity.GetType().GetProperty("LockedBy").SetValue(entity, null);
            }

            base.Entry(entity).Property("IsLocked").IsModified = true;
            base.Entry(entity).Property("LockedBy").IsModified = true;
        }
        private string GetCurrenctUser()
        {
            if (HttpContextAccessor.HttpContext?.User?.Identity == null) return null;

            var claims = (ClaimsIdentity)HttpContextAccessor.HttpContext.User.Identity;

            string CurrentName = claims?.Claims?.FirstOrDefault(a => a.Type.Contains("/name"))?.Value ?? null;
            string CurrentUserName = claims?.Claims?.FirstOrDefault(a => a.Type.Contains("/userdata"))?.Value ?? null;

            string CurrentUser = CurrentUserName + " - " + CurrentName;

            return CurrentUser;
        }


    }
}
