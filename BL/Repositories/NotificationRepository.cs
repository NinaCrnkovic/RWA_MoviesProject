using BL.BLModels;
using BL.DALModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Repositories
{
    public interface INotificationRepository
    {
        IEnumerable<BLNotification> GetAll();
        BLNotification GetById(int id);
        void Add(BLNotification notification);
        void Update(BLNotification notification);
        void Delete(int id);
    }

    public class NotificationRepository : INotificationRepository
    {
        private readonly RwaMoviesContext _dbContext;

        public NotificationRepository(RwaMoviesContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IEnumerable<BLNotification> GetAll()
        {
            return _dbContext.Notifications;
        }

        public BLNotification GetById(int id)
        {
            return _dbContext.Notifications.Find(id);
        }

        public void Add(BLNotification notification)
        {
            _dbContext.Notifications.Add(notification);
            _dbContext.SaveChanges();
        }

        public void Update(BLNotification notification)
        {
            _dbContext.Notifications.Update(notification);
            _dbContext.SaveChanges();
        }

        public void Delete(int id)
        {
            var notification = _dbContext.Notifications.Find(id);
            if (notification == null)
            {
                throw new InvalidOperationException("Notification not found");
            }

            _dbContext.Notifications.Remove(notification);
            _dbContext.SaveChanges();
        }
    }

   

}
