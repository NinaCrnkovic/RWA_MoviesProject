using AutoMapper;
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
        BLNotification Add(BLNotification notification);
        BLNotification Update(int id, BLNotification notification);
        void Delete(int id);
    }

    public class NotificationRepository : INotificationRepository
    {
        private readonly RwaMoviesContext _dbContext;
        private readonly IMapper _mapper;

        public NotificationRepository(RwaMoviesContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public IEnumerable<BLNotification> GetAll()
        {
            var dbNotifications = _dbContext.Notification;
            var blNotifications = _mapper.Map<IEnumerable<BLNotification>>(dbNotifications);

            return blNotifications;
        }

        public BLNotification GetById(int id)
        {
            var dbNotification = _dbContext.Notification.FirstOrDefault(n => n.Id == id);
            var blNotification = _mapper.Map<BLNotification>(dbNotification);

            return blNotification;
        }

        public BLNotification Add(BLNotification notification)
        {
            var newDbNotification = _mapper.Map<Notification>(notification);
            newDbNotification.Id = 0;
            _dbContext.Notification.Add(newDbNotification);
            _dbContext.SaveChanges();

            var newBlNotification = _mapper.Map<BLNotification>(newDbNotification);
            return newBlNotification;
        }

        public BLNotification Update(int id, BLNotification notification)
        {
            var dbNotification = _dbContext.Notification.FirstOrDefault(n => n.Id == id);
            if (dbNotification == null)
            {
                throw new InvalidOperationException("Notification not found");
            }

            _mapper.Map(notification, dbNotification);
            _dbContext.SaveChanges();

            var updatedBlNotification = _mapper.Map<BLNotification>(dbNotification);
            return updatedBlNotification;
        }



        public void Delete(int id)
        {
            var notification = _dbContext.Notification.FirstOrDefault(s => s.Id == id);
            if (notification == null)
            {
                throw new InvalidOperationException("Notification not found");
            }

            _dbContext.Notification.Remove(notification);
            _dbContext.SaveChanges();
        }

        
    }

   

}
