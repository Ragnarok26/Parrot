using System;
using System.Collections.Generic;

namespace Entity.Restaurant
{
    public class StoreData
    {
        public Guid Uuid { get; set; }
        public string Email { get; set; }
        public IEnumerable<Store> Stores { get; set; }
        public string Username { get; set; }
    }
}
