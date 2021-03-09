﻿namespace Entities.Contracts
{
    public interface ICartItem
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
    }
}
