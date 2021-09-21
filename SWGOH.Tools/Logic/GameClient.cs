using Microsoft.Extensions.Options;
using SWGOH.Models.GameClients;
using SWGOH.Models.Options;
using SWGOH.Models.Providers;
using SWGOH.Tools.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWGOH.Tools.Logic
{
    public class GameClient
    {
        private readonly GameClientService _service;
        private readonly GCOptions _gcOptions;
        private readonly GameClientOptions _gameClientOptions;

        public GameClient(GameClientService service,
                            IOptions<GCOptions> gcOptions,
                            IOptions<GameClientOptions> gameClientOptions)
        {
            _service = service;
            _gcOptions = gcOptions.Value;
            _gameClientOptions = gameClientOptions.Value;
        }
        public async Task<UserAuthModel> GetUserAuthAsync()
        {
            var data = await _service.GetUserAuthAsync(_gcOptions.ID);
            return new UserAuthModel
            {
                PlayerId = data.PlayerId,
                Token = data.Token
            };
        }
        public async Task<StoreRpcBuyItemModel> GetStoreRpcBuyItemAsync(UserAuthModel auth)
        {
            var data = await _service.GetStoreRpcBuyItemAsync(auth);
            var unitshard = data.Item.Product.Unitshard;
            var gearshard = data.Item.Product.Gearshard;
            var credit = data.Item.Product.Credit;

            var item = new StoreRpcBuyItemModel { Category = BuyItemCategory.Unknown, Item = "Unknown", Quantity = 1 };
            if (unitshard != null)
            {
                item = new StoreRpcBuyItemModel { Category = BuyItemCategory.Units, Item = unitshard.Name, Quantity = (int)unitshard.Quantity };
            }
            if (gearshard != null)
            {
                item = new StoreRpcBuyItemModel { Category = BuyItemCategory.Gears, Item = gearshard.Name, Quantity = (int)gearshard.Quantity };
            }
            if (credit != null)
            {
                item = new StoreRpcBuyItemModel { Category = BuyItemCategory.Credits, Item = credit.Name.ToString(), Quantity = (int)credit.Quantity };
            }
            return item;
        }
        public async Task<long> GetPlayerAllyPointsAsync(UserAuthModel auth)
        {
            var data = await _service.GetPlayerRpcGetInitialDataAsync(auth);
            var points = data.PlayerAdvancedInfo.Stuff.FirstOrDefault(t => t.Type == Models.Protocols.StuffType.AllyPoints);
            if (points == null)
            {
                return 0;
            }
            return points.Quatity;
        }
    }
}
