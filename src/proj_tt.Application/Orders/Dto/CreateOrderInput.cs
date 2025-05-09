using proj_tt.Orders;
using proj_tt.Orders.Dto;
using System.Collections.Generic;

public class CreateOrderInput
{
    public int? ProductId { get; set; }
    public int? Quantity { get; set; }
    public List<OrderItemInput> OrderItems { get; set; }
    public PaymentMethod PaymentMethod { get; set; }
    public decimal ShippingFee { get; set; }

    public CreateOrderInput()
    {
        OrderItems = new List<OrderItemInput>();
        PaymentMethod = PaymentMethod.Cash;
        ShippingFee = 0;
    }
}