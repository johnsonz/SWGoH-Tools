using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SWGOH.Models.GameClients;
using SWGOH.Models.Options;
using SWGOH.Tools.Logic;
using SWGOH.Tools.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SWGOH.Tools
{
    public class BGService : IHostedService
    {
        private readonly ILogger _logger;
        private readonly GameClient _gameClient;

        public BGService(ILogger<BGService> logger,
            IHostApplicationLifetime appLifetime,
            GameClient gameClient
           )
        {
            _logger = logger;
            _gameClient = gameClient;

        }
        public async Task StartAsync(CancellationToken cancellationToken)
        {

            // Console.WriteLine(Menu());
            // string menu = Console.ReadLine();
            Console.WriteLine("Initiating...");

            var auth = await _gameClient.GetUserAuthAsync();

            var points = await _gameClient.GetPlayerAllyPointsAsync(auth);
            var count = points / 250;

            Dictionary<BuyItemCategory, Dictionary<string, int>> buyItemStatistics =
                                           new Dictionary<BuyItemCategory, Dictionary<string, int>>();

            Console.WriteLine("Total " + points + " ally points. Repeat "+count);

            for (int i = 0; i < count; i++)
            {
                var item = await _gameClient.GetStoreRpcBuyItemAsync(auth);
                Console.WriteLine(string.Format("[{0}]: [{1}]-[{2}]-[{3}]", i + 1, item.Category.ToString(), item.Item, item.Quantity));
                Thread.Sleep(1000);
            }
            Console.ReadLine();

        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        private void OnStarted()
        {
        }

        private void OnStopping()
        {
        }

        private void OnStopped()
        {
        }
        private string Menu()
        {
            return string.Empty;
        }
    }
}
