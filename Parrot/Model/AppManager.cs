using Entity.Restaurant;
using Entity.Token;
using System;
using System.Collections.Generic;

namespace Parrot.Model
{
    public class AppManager
    {
        public static TokenData TokenData { get; set; }
        public static DateTime? ExpirationDate { get; set; }
        public static StoreData UserStoreInfo { get; set; }
    }
}
