using System;

namespace SWGOH.Models.GameClients
{
    public class StoreRpcBuyItemModel
    {
        public BuyItemCategory  Category { get; set; }
        public string Item { get; set; }
        public int Quantity { get; set; }
    }
    public enum BuyItemCategory
    {
        Units,
        Gears,
        Credits,
        Unknown
    }
}
