﻿using JoinMyCarTrip.Application.Interfaces;
using JoinMyCarTrip.Application.Models.Messages;
using JoinMyCarTrip.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace JoinMyCarTrip.Controllers
{
    public class MessageController: BaseController
    {
        private readonly IMessageService messageService;

        public MessageController(IMessageService _messageService 
            ,UserManager<ApplicationUser> userManager)
            :base(userManager)
        {
            messageService = _messageService;
        }
        
        [HttpGet]
        public IActionResult TextMessage(string tripId)
        {
            ViewBag.TripId = tripId;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> TextMessage(TextMessageFormViewModel form, string tripId)
        {
            if (!ModelState.IsValid)
            {
                return View(form);
            }

            var userId = ApplicationUser.Id;
            await messageService.TextMessage(form, tripId, userId);

            return Redirect($"/Trip/Details?tripId={tripId}");
        }
    }
}
