using iRead.API.Models;
using iRead.API.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using iRead.API.Utilities;

namespace iRead.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : CustomControllerBase
    {
        private readonly INotificationRepository _notificationRepository;

        public NotificationController(INotificationRepository _notificationRepository, ILogger<CustomControllerBase> logger) : base(logger)
        {
            this._notificationRepository = _notificationRepository;
        }

        [HttpGet]
        [Route("{userId}")]
        public async Task<ActionResult<IEnumerable<UserNotificationResponse>>> Get(int userId)
        {
            var notifications = await _notificationRepository.GetNotifications(userId);
            return ReturnIfNotEmpty(notifications, "No notifications found for this user");
        }

        [HttpGet]
        [Route("GetOrganized/{userId}")]
        public async Task<ActionResult> GetOrganized(int userId)
        {
            var notifications = await _notificationRepository.GetNotifications(userId);
            object organizedNotifications = null;
            if (notifications.Count() > 0)
                organizedNotifications = new { Viewed = notifications.Where(x => x.Viewed == 1).MapResponse(), NotViewed = notifications.Where(x => x.Viewed == 0).MapResponse() };

            return ReturnIfNotEmpty(organizedNotifications, "No notifications found", false);

        }

        [HttpGet]
        [Route("GetNotViewed/{userId}")]
        public async Task<ActionResult<IEnumerable<UserNotificationResponse>>> GetNotViewedNotifications(int userId)
        {
            var notifications = await _notificationRepository.GetNotViewedNotifications(userId);
            return ReturnIfNotEmpty(notifications, "No notifications found for this user");
        }

        [HttpGet]
        [Route("GetNotViewedCount/{userId}")]
        public async Task<ActionResult<int>> GetNotViewedCount(int userId)
        {
            var count = await _notificationRepository.GetNumberOfUnreadNotifications(userId);
            return ReturnIfNotEmpty(count, performMapping: false);
        }

        [HttpPut]
        [Authorize]
        [Route("{id}")]
        public async Task<ActionResult<BookResponse>> MarkNotificationAsViewed(int id)
        {
            try
            {
                var notificationToUpdate = await _notificationRepository.GetNotification(id);
                if (notificationToUpdate == null)
                    return ReturnResponse(ResponseType.NotFound, $"Notification with id {id} not found");

                notificationToUpdate.Viewed = 1;
                var updatedNotification = await _notificationRepository.UpdateNotification(notificationToUpdate);
                return ReturnResponse(ResponseType.Updated, "Updated successfully", updatedNotification.MapResponse());
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return ReturnResponse(ResponseType.Error);
            }
        }
    }
}
