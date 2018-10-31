using Edison.Api.Config;
using Edison.Core.Common.Models;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using AutoMapper;
using System;
using Microsoft.Azure.NotificationHubs;
using Edison.Common.Interfaces;
using Edison.Common.DAO;

namespace Edison.Api.Helpers
{
    public class NotificationHubDataManager
    {
        private readonly IMapper _mapper;
        private readonly WebApiConfiguration _config;
        private readonly ICosmosDBRepository<NotificationDAO> _repoNotifications;

        public NotificationHubDataManager(
            IOptions<WebApiConfiguration> config,
            IMapper mapper,
            ICosmosDBRepository<NotificationDAO> repoNotifications)
        {
            _mapper = mapper;
            _config = config.Value;
            _repoNotifications = repoNotifications;
        }

        public async Task<NotificationModel> CreateNotification(NotificationCreationModel notification)
        {
            var date = DateTime.UtcNow;

            NotificationDAO notificationDao = new NotificationDAO()
            {
                NotificationText = notification.NotificationText,
                CreationDate = date,
                UpdateDate = date,
                Status = notification.Status,
                Tags = notification.Tags,
                Title = notification.Title,
                User = notification.User
            };

            notificationDao.Id = await _repoNotifications.CreateItemAsync(notificationDao);
            if (_repoNotifications.IsDocumentKeyNull(notificationDao))
                throw new Exception($"An error occured when creating a new notification");

            NotificationModel output = _mapper.Map<NotificationModel>(notificationDao);
            return output;
        }

        public async Task<IEnumerable<NotificationModel>> GetNotifications(int pageSize, string continuationToken)
        {
            var notifications = await _repoNotifications.GetItemsPagingAsync(pageSize, continuationToken);
            IEnumerable<NotificationModel> output = _mapper.Map<IEnumerable<NotificationModel>>(notifications.List);
            return output;
        }
    }
}
