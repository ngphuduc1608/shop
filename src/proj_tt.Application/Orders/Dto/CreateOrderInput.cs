using proj_tt.Orders.Dto;
using System.Collections.Generic;

public class CreateOrderInput
{
    public int? ProductId { get; set; }
    public int? Quantity { get; set; }
    public List<OrderItemInput> OrderItems { get; set; }
    public string Name { get; set; }
    public string Address { get; set; }
    public string Phone { get; set; }
}