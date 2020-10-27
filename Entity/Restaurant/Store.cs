using Entity.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Entity.Restaurant
{
    public class Store : BaseNotifyProperty
    {
        public Store()
        {
            Categories = new List<Category>();
        }

        public string Length
        {
            get => $"     {Categories.Count()}";
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

        private string _availabilityState;
        public string AvailabilityState
        {
            get => _availabilityState;
            set => SetValue(value, ref _availabilityState);
        }

        private IEnumerable<object> _providers;
        public IEnumerable<object> Providers
        {
            get => _providers;
            set => SetValue(value, ref _providers);
        }

        private object _config;
        public object Config
        {
            get => _config;
            set => SetValue(value, ref _config);
        }

        private IEnumerable<Category> _categories;
        public IEnumerable<Category> Categories
        {
            get => _categories;
            set => SetValue(value, ref _categories);
        }
    }
}
