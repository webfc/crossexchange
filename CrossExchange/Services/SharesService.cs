using CrossExchange.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CrossExchange.Services
{
    public class SharesService : ISharesService
    {

        private readonly IShareRepository _shareRepository;
        private readonly ITradeRepository _tradeRepository; 

        public SharesService(ITradeRepository TradeRepository, IShareRepository ShareRepository)
        {

            _shareRepository = ShareRepository;
            _tradeRepository = TradeRepository; 
        }

       public HourlyShareRate GetShare(string simbol)
        {

            try
            {

                return _shareRepository.Query().Where(x => x.Symbol == simbol).OrderBy(x => x.TimeStamp).FirstOrDefault(); 

            }catch(Exception EX)
            {
                throw new Exception(EX.Message); 

            }

        }

    
        public int AvailableShares(HourlyShareRate share, int portfolioId)
        {


            var shareBuy = _tradeRepository.Query().Where(x => x.Symbol == share.Symbol && x.Action == "BUY" && x.PortfolioId == portfolioId).Sum(x => x.NoOfShares); 
            var shareSell = _tradeRepository.Query().Where(x => x.Action == "SELL" && x.PortfolioId == portfolioId).Sum(x => x.NoOfShares);
            
            int totalShares = shareBuy - shareSell; 

            return totalShares; 
        }

    }
}
