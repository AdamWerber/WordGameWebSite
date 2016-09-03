using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Sdp_Net_proj.Models;
using Sdp_Net_proj.Services;
using Sdp_Net_proj.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sdp_Net_proj.Controllers.Web
{
    public class AppController : Controller
    {
        private IMailService _mailService;
        private IConfigurationRoot _config;
        private ILogger<AppController> _logger;
        private WorldContext _context;

        public AppController(IMailService mailService,
            IConfigurationRoot config,
            WorldContext context,
            ILogger<AppController> logger
            )
        {
            _mailService = mailService;
            _config = config;
            _context = context;
            _logger = logger;
        }

        public IActionResult Index(){
            var data = _context.Users.ToList();

            return View(data);
        }

        [Authorize]
        [HttpPost]
        public IActionResult Game ([FromBody]UserViewModel The_User)
        {
            try
            {
                // get the data from the form 
                var newUser = Mapper.Map<UserViewModel>(The_User);

                return View();
                // return View(data); get data from data base
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to get in Index Page: {ex.Message}");
                return Redirect("/error");
            }
        }

        public IActionResult Login() { return View(); }

        public IActionResult About(){ return View(); }

        public IActionResult Sign_In() { return View(); }
        [HttpPost]
        public IActionResult Sign_In(UserViewModel model)
        {
            if (ModelState.IsValid)
            {
                // action To_Do -> insert service here - add to database.
                ModelState.Clear();

                ViewBag.UserMessage = "You are in the system - Now you can login and play!";
            }
            return View();
        }

        // Get is default.
        public IActionResult Contact()
        {
            //throw new InvalidOperationException("Bad things happen to good developers"); // testing error log
            return View();
        }

        [HttpPost]
        public IActionResult Contact(ContactViewModel model)
        {
            if (model.Email.Contains("aol.com")) // testing
            {
                ModelState.AddModelError("Email", "We don't support AOL address");
            }
            if (ModelState.IsValid)
            {
                _mailService.DebugSendMail(_config["MailSetting:ToAddress"], model.Email, "Word Game Website: Message From " + model.Name, model.Message);
                _mailService.SendEmailAsync(_config["MailSetting:ToAddress"], model.Email, "Word Game Website: Message From " + model.Name, model.Message);
                ModelState.Clear();

                ViewBag.UserMessage = "The Message Was Sent";
            }
            return View();
        }
    }
}
