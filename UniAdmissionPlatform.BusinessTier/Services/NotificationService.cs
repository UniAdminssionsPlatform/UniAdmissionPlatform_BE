using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using UniAdmissionPlatform.BusinessTier.Generations.Repositories;
using UniAdmissionPlatform.BusinessTier.Requests.Notification;
using UniAdmissionPlatform.BusinessTier.ViewModels;
using UniAdmissionPlatform.DataTier.BaseConnect;
using UniAdmissionPlatform.DataTier.Models;

namespace UniAdmissionPlatform.BusinessTier.Generations.Services
{
    public partial interface INotificationService
    {
        Task CreateNotification(CreateNotificationRequest createNotificationRequest);
        Task<List<NotificationViewModel>> GetNotificationOfAccount(int accountId);
    }
    public partial class NotificationService
    {
        private readonly IConfigurationProvider _mapper;

        public NotificationService(IUnitOfWork unitOfWork,INotificationRepository repository, IConfigurationProvider mapper):base(unitOfWork,repository)
        {
            _mapper = mapper;
        }

        public async Task CreateNotification(CreateNotificationRequest createNotificationRequest)
        {
            var notification = _mapper.CreateMapper().Map<Notification>(createNotificationRequest);
            await CreateAsyn(notification);
        }

        public async Task<List<NotificationViewModel>> GetNotificationOfAccount(int accountId)
        {
            var notifications = await Get().Where(n => n.AccountId == accountId && n.DeletedAt == null).ProjectTo<NotificationViewModel>(_mapper).ToListAsync();
            return notifications;
        }
    }
}