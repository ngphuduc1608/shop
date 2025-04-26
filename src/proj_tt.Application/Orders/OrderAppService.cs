using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Runtime.Session;
using Microsoft.EntityFrameworkCore;
using proj_tt.Authorization.Users;
using proj_tt.Orders.Dto;
using proj_tt.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace proj_tt.Orders
{
    public class OrderAppService : ApplicationService, IOrderAppService
    {
        private readonly IRepository<Order> _orderRepository;
        private readonly IRepository<OrderItem> _orderItemRepository;
        private readonly IRepository<Product> _productRepository;
        private readonly IRepository<User, long> _userRepository;
        private readonly IAbpSession _abpSession;

        public OrderAppService(
            IRepository<Order> orderRepository,
            IRepository<OrderItem> orderItemRepository,
            IRepository<Product> productRepository,
            IRepository<User, long> userRepository,
            IAbpSession abpSession)
        {
            _orderRepository = orderRepository;
            _orderItemRepository = orderItemRepository;
            _productRepository = productRepository;
            _userRepository = userRepository;
            _abpSession = abpSession;
        }

        public async Task<OrderDto> GetOrder(int id)
        {
            var order = await _orderRepository.GetAll()
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order == null)
            {
                throw new ApplicationException("Order not found");
            }

            return ObjectMapper.Map<OrderDto>(order);
        }
        // l?y all order 1 ng??i
        public async Task<PagedResultDto<OrderDto>> GetUserOrders(PagedAndSortedResultRequestDto input)
        {
            var userId = _abpSession.UserId;
            if (userId == null)
            {
                throw new ApplicationException("User not logged in");
            }

            var query = _orderRepository.GetAll()
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .Where(o => o.UserId == userId);

            var totalCount = await query.CountAsync();

            query = query.OrderByDescending(o => o.CreationTime)
                .Skip(input.SkipCount)
                .Take(input.MaxResultCount);

            var orders = await query.ToListAsync();
            var orderDtos = ObjectMapper.Map<List<OrderDto>>(orders);

            return new PagedResultDto<OrderDto>(totalCount, orderDtos);
        }
        //l?y t?t c? order cuar nhieu nguoi(admin)
        public async Task<List<OrderDto>> GetAllOrders()
        {
            var orders = await _orderRepository.GetAll()
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .OrderByDescending(o => o.CreationTime)
                .ToListAsync();

            var orderDtos = ObjectMapper.Map<List<OrderDto>>(orders);

            // Get all user IDs from orders
            //dis loai bo id trung nhau
            var userIds = orders.Select(o => o.UserId).Distinct().ToList();
            var users = await _userRepository.GetAll()
                .Where(u => userIds.Contains(u.Id))
                .ToDictionaryAsync(u => u.Id, u => u.UserName);

            // Add usernames to order DTOs
            foreach (var orderDto in orderDtos)
            {
                if (users.TryGetValue(orderDto.UserId, out var username))
                {
                    orderDto.Username = username;
                }
            }

            return orderDtos;
        }

        public async Task<OrderDto> CreateOrder(CreateOrderInput input)
        {
            var userId = _abpSession.UserId;
            if (userId == null)
            {
                throw new ApplicationException("User not logged in");
            }

            var order = new Order(input.Name, input.Address, input.Phone, userId.Value);
            await _orderRepository.InsertAsync(order);
            await CurrentUnitOfWork.SaveChangesAsync();

            decimal totalAmount = 0;
            foreach (var item in input.OrderItems)
            {
                var product = await _productRepository.GetAsync(item.ProductId);
                var orderItem = new OrderItem(order.Id, item.ProductId, item.Quantity, product.Price);
                await _orderItemRepository.InsertAsync(orderItem);
                totalAmount += orderItem.TotalPrice;
            }

            order.TotalAmount = totalAmount;
            await _orderRepository.UpdateAsync(order);
            await CurrentUnitOfWork.SaveChangesAsync();

            return await GetOrder(order.Id);
        }

        public async Task<OrderDto> UpdateOrderStatus(UpdateOrderStatusInput input)
        {
            var order = await _orderRepository.GetAsync(input.OrderId);
            order.Status = input.Status;
            await _orderRepository.UpdateAsync(order);
            await CurrentUnitOfWork.SaveChangesAsync();

            return await GetOrder(order.Id);
        }

        public async Task DeleteOrder(int id)
        {
            var order = await _orderRepository.GetAsync(id);
            if (order == null)
            {
                throw new ApplicationException("Order not found");
            }

            // Delete all order items first due to foreign key constraint
            var orderItems = await _orderItemRepository.GetAll()
                .Where(oi => oi.OrderId == id)
                .ToListAsync();

            foreach (var item in orderItems)
            {
                await _orderItemRepository.DeleteAsync(item);
            }

            await _orderRepository.DeleteAsync(order);
            await CurrentUnitOfWork.SaveChangesAsync();
        }
    }
}
//assad