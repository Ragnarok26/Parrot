using Entity.Common;
using System;

namespace Entity.Restaurant
{
    public class Product : BaseNotifyProperty
    {
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

        private string _description;
        public string Description
        {
            get => _description;
            set => SetValue(value, ref _description);
        }

        private string _imageUrl;
        public string ImageUrl
        {
            get => _imageUrl;
            set => SetValue(value, ref _imageUrl);
        }

        private string _legacyId;
        public string LegacyId
        {
            get => _legacyId;
            set => SetValue(value, ref _legacyId);
        }

        private decimal _price;
        public decimal Price
        {
            get => _price;
            set => SetValue(value, ref _price);
        }

        private int _alcoholCount;
        public int AlcoholCount
        {
            get => _alcoholCount;
            set => SetValue(value, ref _alcoholCount);
        }

        private bool _soldAlone;
        public bool SoldAlone
        {
            get => _soldAlone;
            set => SetValue(value, ref _soldAlone);
        }

        private string _availability;
        public string Availability
        {
            get => _availability;
            set => SetValue(value, ref _availability);
        }

        private object _providerAvailability;
        public object ProviderAvailability
        {
            get => _providerAvailability;
            set => SetValue(value, ref _providerAvailability);
        }

        private Category _category;
        public Category Category
        {
            get => _category;
            set => SetValue(value, ref _category);
        }

        public bool IsOn
        {
            get => Availability == "AVAILABLE";
        }
    }
}
