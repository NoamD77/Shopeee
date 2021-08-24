﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Shopeee.Models
{
    public class Item
    {
        public int Id { get; set; }
        public String Name { get; set; }
        public float Price { get; set; }
        public int Quantity { get; set; }
        //size? inheritance?
        public String Picture { get; set; }
        public String Description { get; set; }
        public GenderType Gender { get; set; }
        public ItemType Type { get; set; }
        public ColorType Color { get; set; }


        public List<Branch> BranchAvailabilty { get; set; }
        [ForeignKey("Brand")]
        public int BrandID { get; set; }
        public Brand Brand { get; set; }
    }
}