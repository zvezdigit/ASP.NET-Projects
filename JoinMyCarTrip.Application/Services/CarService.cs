﻿using JoinMyCarTrip.Application.Interfaces;
using JoinMyCarTrip.Application.Models.Cars;
using JoinMyCarTrip.Data.Common;
using JoinMyCarTrip.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JoinMyCarTrip.Application.Services
{
    public class CarService : ICarService
    {
        private readonly IRepository repository;

        public CarService(IRepository _repository)
        {
                repository = _repository;
        }


        public async Task AddCar(AddCarFormViewModel model, string userId)
        {
            if(string.IsNullOrEmpty(userId))
            {
                throw new ArgumentException("userId cannot be null or empty", nameof(userId));
            }

            if(model is null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            var car = new Car
            {
                Model = model.BrandAndModel,
                Year = model.Year,
                ImageUrl = model.ImageUrl,
                LuggageAllowed = model.LuggageAllowed,
                Smoking = model.Smoking,
                PetsAllowed = model.PetsAllowed,
                IsWithAirConditioner = model.IsWithAirConditioner,
                UserId = userId
            };

            await repository.AddAsync(car);
            await repository.SaveChangesAsync();
        }

        public AllCarsViewModel GetAllCars(string userId)
        {
            return repository.All<ApplicationUser>()
                .Include(x => x.Cars)
                .Where(r => r.Id == userId)
                .Select(user => new AllCarsViewModel
                {
                    Cars = user.Cars.Select(car => new CarListViewModel
                    {
                        BrandAndModel = car.Model,
                        Year = car.Year,
                        ImageUrl = car.ImageUrl,
                    }).ToList()

                }).FirstOrDefault();
        }
    }
}
