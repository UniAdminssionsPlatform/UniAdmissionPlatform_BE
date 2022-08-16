using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Hangfire;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Razor.Templating.Core;
using UniAdmissionPlatform.BusinessTier.Commons.Enums;
using UniAdmissionPlatform.BusinessTier.Generations.Repositories;
using UniAdmissionPlatform.BusinessTier.Generations.Services;
using UniAdmissionPlatform.BusinessTier.Requests.Mail;
using UniAdmissionPlatform.BusinessTier.Responses.Email;

namespace UniAdmissionPlatform.BusinessTier.Services
{
    public interface IMailBookingService
    {
        Task SendMailForNewBookingToSchoolAdmin(int eventCheckId);
        Task SendEmailForApprovedEventToUniAdmin(int eventCheckId);
        Task SendEmailForRejectedEventToUniAdmin(int eventCheckId);
        Task SendEmailForStudent(int eventCheckId);
    }

    public class MailBookingService : IMailBookingService
    {
        private readonly IEventCheckService _eventCheckService;
        private readonly IAccountService _accountService;
        private readonly IMailService _mailService;
        private readonly IUserRepository _userRepository;
        private readonly IReasonRepository _reasonRepository;
        private readonly IConfiguration _configuration;

        public MailBookingService(IMailService mailService, IEventCheckService eventCheckService, IAccountService accountService, IUserRepository userRepository, IReasonRepository reasonRepository, IConfiguration configuration)
        {
            _mailService = mailService;
            _eventCheckService = eventCheckService;
            _accountService = accountService;
            _userRepository = userRepository;
            _reasonRepository = reasonRepository;
            _configuration = configuration;
        }

        public async Task SendMailForNewBookingToSchoolAdmin(int eventCheckId)
        {
            var eventChecks = await _eventCheckService.Get()
                .Where(ev => ev.Id == eventCheckId)
                .Include(ec => ec.Slot).ThenInclude(s => s.HighSchool)
                .Include(ec => ec.Event).ThenInclude(e => e.University)
                .FirstOrDefaultAsync();

            if (eventChecks == null)
            {
                throw new Exception("Fail at sending mail for new booking to school admin");
            }

            var highSchoolId = eventChecks.Slot.HighSchoolId;

            var accounts = await _accountService.Get()
                .Where(a => a.HighSchoolId == highSchoolId && a.RoleId == "schoolAdmin")
                .ToListAsync();

            var universityName = eventChecks.Event.University.Name;

            var nameOfUniversities = new List<string>();
            nameOfUniversities.Add(universityName);

            var bookingTime = eventChecks.CreatedAt;

            var slotStartDate = eventChecks.Slot.StartDate;

            var slotEndDate = eventChecks.Slot.EndDate;

            foreach (var account in accounts)
            {
                var model = new MailNewBookingRequestToSchoolAdminModel
                {
                    NameOfSchoolAdmin = 
                        (account.LastName ?? "")
                        + (" " + account.MiddleName ?? "")
                        + (" " + account.FirstName ?? ""),
                    NameOfUniversities = nameOfUniversities,
                    BookingTime = bookingTime,
                    SlotStartDate = slotStartDate,
                    SlotEndDate = slotEndDate,
                    //todo:
                    BookingDetailUrl = _configuration["Url:Fe"] + "/high-school/slot-manage",
                };

                var mailRequest = new MailRequest
                {
                    //todo: add field email to account table
                    ToEmail = account.EmailContact,
                    HtmlBody = await RazorTemplateEngine.RenderAsync("~/Views/NewBookingRequestToSchoolAdmin.cshtml", model),
                    Subject = $"Cuộc đặt lịch mới ID = {eventChecks.Id}"
                };

                await _mailService.SendHtmlEmailAsync(mailRequest);
            }
        }

        public async Task SendEmailForApprovedEventToUniAdmin(int eventCheckId)
        {
            var eventChecks = await _eventCheckService.Get()
                .Where(ev => ev.Id == eventCheckId)
                .Include(ec => ec.Slot).ThenInclude(s => s.HighSchool)
                .Include(ec => ec.Event).ThenInclude(e => e.University)
                .FirstOrDefaultAsync();

            if (eventChecks == null)
            {
                throw new Exception("Fail at sending mail for new approved event to uni admin");
            }

            var university = eventChecks.Event.University;

            var accounts = await _accountService.Get().Where(a => a.UniversityId == university.Id && a.RoleId == "uniAdmin").ToListAsync();
            
            var approvedTime = eventChecks.UpdatedAt;

            var slotStartDate = eventChecks.Slot.StartDate;

            var slotEndDate = eventChecks.Slot.EndDate;
            
            foreach (var account in accounts)
            {
                var model = new ApprovedEventToUniAdminModel
                {
                    NameOfUniAdmin = 
                        (account.LastName ?? "")
                        + (" " + account.MiddleName ?? "")
                        + (" " + account.FirstName ?? ""),
                    ApprovedTime = approvedTime,
                    BookingDetailUrl = _configuration["Url:Fe"] + "/uni/manage-event",
                    SlotEndDate = slotEndDate,
                    SlotStartDate = slotStartDate,
                    NameOfHighSchool = eventChecks.Slot.HighSchool.Name
                };
                
                var mailRequest = new MailRequest
                {
                    //todo: add field email to account table
                    ToEmail = account.EmailContact,
                    HtmlBody = await RazorTemplateEngine.RenderAsync("~/Views/ApprovedEventToUniAdmin.cshtml", model),
                    Subject = $"Cuộc đặt lịch được chấp nhận ID = {eventChecks.Id}"
                };

                if (!string.IsNullOrWhiteSpace(account.EmailContact))
                {
                    await _mailService.SendHtmlEmailAsync(mailRequest);
                }
            }
            
            
            var studentAccounts = await _userRepository.Get().Include(u => u.Account)
                .Where(u => u.Status == (int)UserStatus.Active && u.Account.HighSchoolId == eventChecks.Slot.HighSchoolId && u.Account.RoleId == "student").Select(u => u.Account).ToListAsync();
            
            foreach (var account in studentAccounts)
            {
                var model = new ApprovedEventToUniAdminModel
                {
                    NameOfUniAdmin = 
                        (account.LastName ?? "")
                        + (" " + account.MiddleName ?? "")
                        + (" " + account.FirstName ?? ""),
                    ApprovedTime = approvedTime,
                    BookingDetailUrl = _configuration["Url:Fe"] + $"/event/{eventChecks.EventId}",
                    SlotEndDate = slotEndDate,
                    SlotStartDate = slotStartDate,
                    NameOfHighSchool = eventChecks.Slot.HighSchool.Name
                };
                
                var mailRequest = new MailRequest
                {
                    //todo: add field email to account table
                    ToEmail = account.EmailContact,
                    HtmlBody = await RazorTemplateEngine.RenderAsync("~/Views/ApprovedEventToUniAdmin.cshtml", model),
                    Subject = $"Cuộc đặt lịch được chấp nhận ID = {eventChecks.Id}"
                };

                if (!string.IsNullOrWhiteSpace(account.EmailContact))
                {
                    await _mailService.SendHtmlEmailAsync(mailRequest);
                }
            }
        }
        
        public async Task SendEmailForRejectedEventToUniAdmin(int eventCheckId)
        {
            var eventChecks = await _eventCheckService.Get()
                .Where(ev => ev.Id == eventCheckId)
                .Include(ec => ec.Slot)
                .ThenInclude(s => s.HighSchool)
                .Include(ec => ec.Event)
                .ThenInclude(e => e.University)
                .FirstOrDefaultAsync();

            var reason = await _reasonRepository.Get().FirstOrDefaultAsync(r => r.ReferenceId == eventCheckId);

            if (eventChecks == null)
            {
                throw new Exception("Fail at sending mail for new rejected event to university admin");
            }

            var university = eventChecks.Event.University;

            var accounts = await _accountService
                .Get()
                .Where(a => a.UniversityId == university.Id 
                            && a.RoleId == "uniAdmin")
                .ToListAsync();
            
            var eventName = eventChecks.Event.Name;
            
            var rejectedTime = eventChecks.UpdatedAt;
            
            // var rejectReason = 

            var slotStartDate = eventChecks.Slot.StartDate;

            var slotEndDate = eventChecks.Slot.EndDate;
            
            foreach (var account in accounts)
            {
                var model = new RejectedEventToUniversityAdminModel
                {
                    NameOfUniAdmin = 
                        (account.LastName ?? "")
                        + (" " + account.MiddleName ?? "")
                        + (" " + account.FirstName ?? ""),
                    EventName = eventName,
                    RejectedTime = rejectedTime,
                    RejectReason = reason?.Detail ?? "",
                    BookingDetailUrl = _configuration["Url:Fe"] + "/uni/manage-event",
                    SlotEndDate = slotEndDate,
                    SlotStartDate = slotStartDate,
                    NameOfHighSchool = eventChecks.Slot.HighSchool.Name
                };
                
                var mailRequest = new MailRequest
                {
                    ToEmail = account.EmailContact,
                    HtmlBody = await RazorTemplateEngine.RenderAsync("~/Views/RejectedEventToUniversityAdmin.cshtml", model),
                    Subject = $"Sự kiện{eventChecks.Event.Name} của bạn đã bị từ chối với ID = {eventChecks.Id}"
                };
                
                if (!string.IsNullOrWhiteSpace(account.EmailContact))
                {
                    await _mailService.SendHtmlEmailAsync(mailRequest);
                }
                
            }
            
        }
        
        public async Task SendEmailForStudent(int eventCheckId)
        {
            var eventChecks = await _eventCheckService.Get()
                .Where(ev => ev.Id == eventCheckId)
                .Include(ec => ec.Slot).ThenInclude(s => s.HighSchool)
                .Include(ec => ec.Event).ThenInclude(e => e.University)
                .FirstOrDefaultAsync();

            if (eventChecks == null)
            {
                throw new Exception("Fail at sending mail for student");
            }
            
            
            var eventName = eventChecks.Event.Name;

            var slotStartDate = eventChecks.Slot.StartDate;

            var slotEndDate = eventChecks.Slot.EndDate;
            
            var studentAccounts = await _userRepository.Get().Include(u => u.Account)
                .Where(u => u.Status == (int)UserStatus.Active 
                            && u.Account.HighSchoolId == eventChecks.Slot.HighSchoolId 
                            && u.Account.RoleId == "student").Select(u => u.Account)
                .ToListAsync();
            
            foreach (var account in studentAccounts)
            {
                var model = new SendMailToStudent
                {
                    NameOfStudent = 
                        (account.LastName ?? "")
                        + (" " + account.MiddleName ?? "")
                        + (" " + account.FirstName ?? ""),
                    BookingDetailUrl = _configuration["Url:Fe"] + $"/event/{eventChecks.EventId}",
                    EventName = eventName,
                    SlotStartDate = slotStartDate,
                    SlotEndDate = slotEndDate,
                    NameOfHighSchool = eventChecks.Slot.HighSchool.Name
                };
                
                var mailRequest = new MailRequest
                {
                    ToEmail = account.EmailContact,
                    HtmlBody = await RazorTemplateEngine.RenderAsync("~/Views/SendMailForStudent.cshtml", model),
                    Subject = $"Thư mời tham gia sự kiện {eventChecks.Event.Name}"
                };

                if (!string.IsNullOrWhiteSpace(account.EmailContact))
                {
                    await _mailService.SendHtmlEmailAsync(mailRequest);
                }
            }
        }
    }
}