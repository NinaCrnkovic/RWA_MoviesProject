using BL.BLModels;
using BL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Services
{
    public interface INotificationService
    {
        void SendNotification(BLNotification notification);
    }

    public class NotificationService : INotificationService
    {
        private readonly INotificationRepository _notificationRepository;

        public NotificationService(INotificationRepository notificationRepository)
        {
            _notificationRepository = notificationRepository;
        }

        public void SendNotification(BLNotification notification)
        {
            // Implement logic to send the notification, such as sending an email, push notification, etc.
            // You can also call the repository to save the notification to the database
            _notificationRepository.Add(notification);
        }
    }
}
