using Entity.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Entity.Restaurant
{
    public class Category : BaseNotifyProperty
    {
        public Category()
        {
            Products = new List<Product>();
        }

        public string Length
        {
            get => $"     {Products.Count()}";
        }

        private Guid _uuid;
        public Guid Uuid
        {
            get => _uuid;
            set => SetValue(value, ref _uuid);
        }

        private string _name;
        public string Name 
        {
            get => _name;
            set => SetValue(value, ref _name);
        }

        private int _sortPosition;
        public int SortPosition 
        {
            get => _sortPosition;
            set => SetValue(value, ref _sortPosition);
        }

        private IEnumerable<Product> _products;
        public IEnumerable<Product> Products 
        {
            get => _products;
            set => SetValue(value, ref _products);
        }
    }
}
