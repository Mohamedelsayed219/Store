using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Domain.Contracts;
using Domain.Exceptions;
using Domain.Models;
using Domain.Models.OrderModels;
using Services.Abstractions;
using Services.Specifications;
using Shared.OrderModels;

namespace Services
{
    public class OrderService(
        IMapper mapper,
        IBasketRepository basketRepository,
        IUnitofwork unitofwork
        ) : IOrderService
    {
        public async Task<OrderResultDto> CreateOrderAsync(OrderRequestDto orderRequest, string userEmail)
        {
            var address = mapper.Map<Address>(orderRequest.ShipToAddress);

            var basket = await basketRepository.GetBasketAsync(orderRequest.BasketId);
            if (basket is null) throw new BasketNotFoundException(orderRequest.BasketId);

            var orderItems = new List<OrderItem>();

            foreach (var item in basket.Items)
            {
                var product = await unitofwork.GetRepository<Product, int>().GetAsync(item.Id);

                if (product is null) throw new ProductNotFoundExceptions(item.Id);

                var orderItem = new OrderItem(new ProductInOrderItem(product.Id, product.Name, product.PictureUrl), item.Quantity, product.Price);
                
                orderItems.Add(orderItem);
            }

            var deliveryMethod = await unitofwork.GetRepository<DeliveryMethod, int>().GetAsync(orderRequest.DeliveryMethodId);
            if (deliveryMethod is null) throw new DeliveryMethodNotFoundException(orderRequest.DeliveryMethodId);

            var subTotal = orderItems.Sum(i => i.Price * i.Quantity);


            var order = new Order(userEmail, address, orderItems, deliveryMethod, subTotal,"");

            await unitofwork.GetRepository<Order, Guid>().AddAsync(order);
            var count = await unitofwork.SaveChangesAsync();

            if (count == 0) throw new OrderCreateBadRequestException();

            var result = mapper.Map<OrderResultDto>(order);

            return result;

        }

        public async Task<IEnumerable<DeliveryMethodDto>> GetAllDeliveryMethods()
        {
            var deliveryMethods = await unitofwork.GetRepository<DeliveryMethod, int>().GetAllAsync();

           var result =  mapper.Map<IEnumerable<DeliveryMethodDto>>(deliveryMethods);

            return result;
            
        }

        public async Task<OrderResultDto> GetOrderByIdAsync(Guid id)
        {
            var spec = new OrderSpecifications(id);

            var order = await unitofwork.GetRepository<Order, Guid>().GetAsync(spec);

            if (order is null) throw new OrderNotFoundException(id);

            var result = mapper.Map<OrderResultDto>(order);

            return result;

        }

        public async Task<IEnumerable<OrderResultDto>> GetOrdersByUserEmailAsync(string userEmail)
        {
            var spec = new OrderSpecifications(userEmail);

            var orders = await unitofwork.GetRepository<Order, Guid>().GetAllAsync(spec);


            var result = mapper.Map<IEnumerable<OrderResultDto>>(orders);

            return result;

        }


    }
}
