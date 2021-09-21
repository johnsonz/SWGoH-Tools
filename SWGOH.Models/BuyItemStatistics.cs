using System;
using System.Collections.Generic;
using SWGOH.Models.GameClients;

namespace SWGOH.Models.Providers
{
    public class BuyItemStatistics
    {
        public Dictionary<BuyItemCategory, BuyItemDetail> BuyItem { get; set; }
    }
    public class BuyItemDetail
    {
        public Dictionary<string, int> Statistics { get; set; }
    }
}
