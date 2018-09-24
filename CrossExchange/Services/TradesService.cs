using CrossExchange.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CrossExchange.Services
{
    public class TradesService : ITradesService
    {

        private readonly ITradeRepository _tradeRepository;
        private readonly IShareRepository _shareRepository; 

        public TradesService(ITradeRepository TradeRepository, IShareRepository ShareRepository)
        {

            _tradeRepository= TradeRepository;
            _shareRepository = ShareRepository; 

        }
         
       public HourlyShareRate RecentRate(string simbol)
        {
            try
            {
                
                return _shareRepository.Query().Where(x => simbol.Contains(x.Symbol)).OrderBy(x => x.TimeStamp).FirstOrDefault();
                
            }
            catch(Exception ex)
            {

                throw new Exception(ex.Message); 
            }


        }

        public async Task<Trade> Add(TradeModel trade, HourlyShareRate share)
        {

            try
            {
                var tradePrice = share.Rate * trade.NoOfShares;

                Trade _trade = new Trade
                {
                    Action = trade.Action,
                    NoOfShares = trade.NoOfShares,
                    PortfolioId = trade.PortfolioId,
                    Symbol = trade.Symbol,
                    Price = tradePrice
                }; 

                await _tradeRepository.InsertAsync(_trade);

                return _trade; 

            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message); 

            }

        }
    }
}
