using AutoMapper;
using BL.BLModels;
using BL.DALModels;
using BL.Repositories;
using BL.Services;
using IntegrationModule.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;

namespace IntegrationModule.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _notificationService;
        private readonly IMapper _mapper;
        private readonly INotificationRepository _notificationRepository;
       // private readonly System.Net.Mail.SmtpClient _smtpClient;

        public NotificationController(INotificationService notificationService, IMapper mapper, INotificationRepository notificationRepository)
        {
            _notificationService = notificationService;
            _mapper = mapper;
            _notificationRepository = notificationRepository;
            //_smtpClient = smtpClient;
        }


        [HttpGet]
        public ActionResult<IEnumerable<Models.Notification>> GetAll()
        {
            try
            {
                var blNotifications = _notificationRepository.GetAll();
                var notifications = _mapper.Map<IEnumerable<BLNotification>>(blNotifications);
                return Ok(notifications);
            }
            catch (Exception ex)
            {
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    "There has been a problem while fetching the data you requested" + ex);
            }
        }

        [HttpGet("{id}")]
        public ActionResult<Models.Notification> GetById(int id)
        {
            try
            {
                var blNotification = _notificationRepository.GetById(id);
                if (blNotification == null)
                {
                    return NotFound();
                }

                var notification = _mapper.Map<BLNotification>(blNotification);
                return Ok(notification);
            }
            catch (Exception)
            {
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    "There has been a problem while fetching the data you requested");
            }
        }

        [HttpPost]
        public IActionResult Create(NotificationRequest request)
        {
            try
            {
                var blNotification = _mapper.Map<BLNotification>(request);
                _notificationRepository.Add(blNotification);
                return Ok();
            }
            catch (Exception)
            {
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    "There has been a problem while creating the notification");
            }
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, NotificationRequest request)
        {
            try
            {
                var existingBlNotification = _notificationRepository.GetById(id);
                if (existingBlNotification == null)
                {
                    return NotFound();
                }

                var blNotification = _mapper.Map<BLNotification>(request);
                blNotification.Id = id;
                _notificationRepository.Update(id, blNotification);
                return Ok();
            }
            catch (Exception)
            {
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    "There has been a problem while updating the notification");
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                _notificationRepository.Delete(id);
                return Ok();
            }
            catch (Exception)
            {
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    "There has been a problem while deleting the notification");
            }
        }
    }
}