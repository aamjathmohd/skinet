using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;
using Core.Entities.OrderAggregate;
using Core.Intefaces;
using Core.Specification;

namespace Infrastructure.Services
{
    public class OrderService : IOrderService
    {
        
        private readonly IBasketRepository _basketRepo;
        private readonly IUnitOfWork _unitOfWork;
        public OrderService(IUnitOfWork unitOfWork,IBasketRepository basketRepo)
        {
            _unitOfWork=unitOfWork;
            _basketRepo=basketRepo;
        }

        public async Task<Order> CreateOrderAsync(string buyerEmail, int deliveryMethodId, string basketId, Address shippingAddress)
        {
            //get basket from basket repo
            var basket=await _basketRepo.GetBasketAsync(basketId);
            //get items from the product repo
            var items=new List<OrderItem>();
            foreach(var item in basket.Items)
            {
                var productItem=await _unitOfWork.Repository<Product>().GetByIdAsync(item.Id);
                var itemOredered=new ProductItemOrdered(productItem.Id,productItem.Name,productItem.PictureUrl);
                var orderItem=new OrderItem(itemOredered,productItem.Price,item.Quantity);
                items.Add(orderItem);
            }
            //get delivery method from repo
            var deliveryMethod=await _unitOfWork.Repository<DeliveryMethod>().GetByIdAsync(deliveryMethodId);
            //calculate subtotal
            var subTotal=items.Sum(item=>item.Price*item.Quantity);
            //create order
            var order=new Order(buyerEmail,shippingAddress,deliveryMethod,items,subTotal);
            _unitOfWork.Repository<Order>().Add(order);
            //save to db
            var result=await _unitOfWork.Complete();
            if(result <=0)
                return null;
            // delete basket 
            await _basketRepo.DeleteBasketAsync(basketId);
            //return order
            return order;
        }

        public async Task<IReadOnlyList<DeliveryMethod>> GetDeliverMethodsAsync()
        {
            return await _unitOfWork.Repository<DeliveryMethod>().ListAllAsync();
        }

        public async Task<Order> GetOrderByIdAsync(int id, string buyerEmail)
        {
            var spec=new OrderWithItemsAndOrderingSpecification(id,buyerEmail);
            return await _unitOfWork.Repository<Order>().GetEntityWithSpec(spec);
        }

        public async Task<IReadOnlyList<Order>> GetOrdersForUserAsync(string buyerEmail)
        {
            var spec=new OrderWithItemsAndOrderingSpecification(buyerEmail);
            return await _unitOfWork.Repository<Order>().ListAsync(spec);
        }
    }
}