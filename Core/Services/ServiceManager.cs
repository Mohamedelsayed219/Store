﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Domain.Contracts;
using Services.Abstractions;

namespace Services
{
    public class ServiceManager(
        IUnitofwork unitofwork,
        IMapper mapper,
        IBasketRepository basketRepository
        ) : IServiceManager
    {
        public IProductService ProductService { get; } = new ProductService(unitofwork, mapper);

        public IBasketService BasketService { get; } = new BasketService(basketRepository, mapper);
    }
}
