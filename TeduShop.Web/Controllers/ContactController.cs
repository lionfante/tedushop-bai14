using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TeduShop.Model.Models;
using TeduShop.Service;
using TeduShop.Web.Models;
using TeduShop.Web.Infrastructure.Extensions;
using BotDetect.Web.Mvc;
using TeduShop.Common;

namespace TeduShop.Web.Controllers
{
    public class ContactController : Controller
    {
        IContactDetailService _contactDetailService;
        IFeedbackService _feedbackService;
        public ContactController(IContactDetailService contactDetailService, IFeedbackService feedbackService)
        {
            _contactDetailService = contactDetailService;
            _feedbackService = feedbackService;
        }
        // GET: Contact
        public ActionResult Index()
        {

            FeedbackViewModel feedbackViewModel = new FeedbackViewModel();

            feedbackViewModel.ContactDetailViewModel = GetDetail();
            return View(feedbackViewModel);
        }

        [HttpPost]
        [CaptchaValidation("CaptchaCode", "ContactCaptcha", "Mã xác nhận không đúng!")]
        public ActionResult SendFeedback(FeedbackViewModel feedbackViewModel)
        {
            if (ModelState.IsValid)
            {
                Feedbacks newFeedback = new Feedbacks();
                newFeedback.UpdateFeedback(feedbackViewModel);
                _feedbackService.Create(newFeedback);
                _feedbackService.Save();

                ViewData["SuccessMsg"] = "Gửi phản hồi thành công";
                


                string mailContent = System.IO.File.ReadAllText(Server.MapPath("/Assets/client/templates/contactTemplate.html"));
                mailContent = mailContent.Replace("{{Name}}", feedbackViewModel.Name);
                mailContent = mailContent.Replace("{{Email}}", feedbackViewModel.Email);
                mailContent = mailContent.Replace("{{Message}}", feedbackViewModel.Message);
                var adminMail = ConfigHelper.GetByKey("AdminEmail");
                MailHelper.SendMail(adminMail, "Thông tin liên hệ từ website", mailContent);

                feedbackViewModel.Name = "";
                feedbackViewModel.Message = "";
                feedbackViewModel.Email = "";
            }
            feedbackViewModel.ContactDetailViewModel = GetDetail();

            return View("Index", feedbackViewModel);

        }

        private ContactDetailViewModel GetDetail()
        {
            var model = _contactDetailService.GetDefaultContact();
            var contactViewModel = Mapper.Map<ContactDetails, ContactDetailViewModel>(model);
            return contactViewModel;
        }
    }
}