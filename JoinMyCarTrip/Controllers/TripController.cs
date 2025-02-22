﻿using JoinMyCarTrip.Application.Interfaces;
using JoinMyCarTrip.Application.Models.Trips;
using JoinMyCarTrip.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace JoinMyCarTrip.Controllers
{
    public class TripController : BaseController

    {
        private readonly ITripService tripService;

        public TripController(ITripService _tripService
            ,UserManager<ApplicationUser> userManager)
            : base(userManager)
        {
            this.tripService = _tripService;
        }

        [HttpGet]
        public IActionResult Create()
        {
            var userId = ApplicationUser.Id;
            var cars = tripService.GetAllTripCars(userId).MyCars.ToList();

            if (cars.Count == 0)
            {
                return Redirect("/Trip/CreateTripAddCar");
            }

            var tripTypes = tripService.GetAllTripTypes().ToList();

            ViewBag.TripTypes = tripTypes;
            ViewBag.Cars = cars;
                        
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateTripFormViewModel model)
        {
            var userId = ApplicationUser.Id;

            if (!ModelState.IsValid)
            {
                ViewBag.Cars = tripService.GetAllTripCars(userId).MyCars.ToList();
                ViewBag.TripTypes = tripService.GetAllTripTypes().ToList();

                return View(model);
            }

            await tripService.CreateTrip(model, userId);

            return Redirect("/Trip/All");
        }

        [HttpGet]
        public IActionResult All()
        {
            var allTrips = tripService.GetAllTrips();

            return View(allTrips);
        }

        [HttpGet]
        public IActionResult Details(string tripId)
        {
            TripDetailsViewModel tripDetailsViewModel = tripService.GetTripDetails(tripId);

            return View(tripDetailsViewModel);
        }

        public IActionResult MyTrips()
        {
            var userId = ApplicationUser.Id;
            var trips = tripService.GetMyTrips(userId);

            return View(trips);
        }

        public async Task<IActionResult> JoinTrip(string tripId)
        {
            try
            {
                var userId = ApplicationUser.Id;
                await tripService.AddUserToTrip(tripId, userId);
            }
            catch (Exception e)
            {

                return View("JoinError", new { ErrorMessage = e.Message });
            }
           

            return Redirect("/Trip/MyTrips");
        }

        public IActionResult CreateTripAddCar()
        {
            return View();
        }
    }
}
