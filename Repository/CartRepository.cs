﻿using Entities.Contracts;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Repository.DbConfig;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Repository
{
    public class CartRepository : ICartRepository
    {
        //for getting db connection via dependendecy injection
        private readonly SqlContext _context; // autogenerated: press "."  on _context: .generate readonly field
        public CartRepository(SqlContext context)
        {
            // generate readonly field
            _context = context;
        }

        public IEnumerable<ICartItem> GetAll()
        {
            List<CartItem> cartItems = new List<CartItem>{
                new CartItem(){ ProductId=1, Quantity=5, Price=200d},
                new CartItem(){ProductId=2, Quantity=10, Price=125}
            };
            return cartItems;
        }

        public ICartItem GetById(int id)
        {
            return GetAll().Where(x => x.ProductId == id).FirstOrDefault();
        }
    }
}