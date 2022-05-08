﻿using System;
using System.Linq;
using System.Threading.Tasks;
using Hangfire;
using Microsoft.EntityFrameworkCore;
using Razor.Templating.Core;
using UniAdmissionPlatform.BusinessTier.Generations.Services;
using UniAdmissionPlatform.BusinessTier.Requests.Mail;
using UniAdmissionPlatform.BusinessTier.Responses.Email;

namespace UniAdmissionPlatform.BusinessTier.Services
{
    public interface IMailBookingService
    {
        Task SendMailForNewBookingToSchoolAdmin(int eventCheckId);
    }

    public class MailBookingService : IMailBookingService
    {
        private readonly IEventCheckService _eventCheckService;
        private readonly IAccountService _accountService;
        private readonly IMailService _mailService;

        public MailBookingService(IMailService mailService, IEventCheckService eventCheckService, IAccountService accountService)
        {
            _mailService = mailService;
            _eventCheckService = eventCheckService;
            _accountService = accountService;
        }

        public async Task SendMailForNewBookingToSchoolAdmin(int eventCheckId)
        {
            var eventChecks = await _eventCheckService.Get()
                .Where(ev => ev.Id == eventCheckId)
                .Include(ec => ec.Slot).ThenInclude(s => s.HighSchool)
                .Include(ec => ec.Event).ThenInclude(e => e.UniversityEvents).ThenInclude(ue => ue.University)
                .FirstOrDefaultAsync();

            if (eventChecks == null)
            {
                throw new Exception("Fail at sending mail for new booking to school admin");
            }

            var highSchoolId = eventChecks.Slot.HighSchoolId;

            var accounts = await _accountService.Get()
                .Where(a => a.HighSchoolId == highSchoolId && a.RoleId == "schoolAdmin")
                .ToListAsync();

            var nameOfUniversities = eventChecks.Event.UniversityEvents.Select(ue => ue.University.Name).ToList();

            var bookingTime = eventChecks.CreatedAt;

            var slotStartDate = eventChecks.Slot.StartDate;

            var slotEndDate = eventChecks.Slot.EndDate;

            foreach (var account in accounts)
            {
                var model = new MailNewBookingRequestToSchoolAdminModel
                {
                    NameOfSchoolAdmin = 
                        (account.FirstName ?? "")
                        + (" " + account.MiddleName ?? "")
                        + (" " + account.LastName ?? ""),
                    NameOfUniversities = nameOfUniversities,
                    BookingTime = bookingTime,
                    SlotStartDate = slotStartDate,
                    SlotEndDate = slotEndDate,
                    //todo:
                    BookingDetailUrl = "http://uniadmissionplatformwebapi-dev.ap-southeast-1.elasticbeanstalk.com/"
                };

                var mailRequest = new MailRequest
                {
                    //todo: add field email to account table
                    ToEmail = "bacnvse141019@fpt.edu.vn",
                    HtmlBody = await RazorTemplateEngine.RenderAsync("~/Views/NewBookingRequestToSchoolAdmin.cshtml", model),
                    Subject = $"Cuộc đặt lịch mới ID = {eventChecks.Id}"
                };

                await _mailService.SendHtmlEmailAsync(mailRequest);
            }
        }
    }
}