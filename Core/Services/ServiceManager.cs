﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Domain.Contracts;
using Domain.Models.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Services.Abstractions;
using Shared;

namespace Services
{
    public class ServiceManager(
        IUnitofwork unitofwork,
        IMapper mapper,
        IBasketRepository basketRepository,
        ICasheRepository casheRepository,
        UserManager<AppUser> userManager,
        IOptions<JwtOptions> options
        ) : IServiceManager
    {
        public IProductService ProductService { get; } = new ProductService(unitofwork, mapper);

        public IBasketService BasketService { get; } = new BasketService(basketRepository, mapper);

        public IcasheService CasheService { get; } = new CasheService(casheRepository);

        public IAuthService AuthService { get; } = new AuthService(userManager, options);

        public IOrderService OrderService { get; } = new OrderService(mapper, basketRepository, unitofwork);
    }
}
