using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CrossExchange.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace CrossExchange.Controller
{
    [Route("api/Trade")]
    public class TradeController : ControllerBase
    {
        private ITradesService _tradesServices;
        private ITradeRepository _tradeRepository;
        private IPortfolioService _portfolioService;
        private ISharesService _sharesService; 


        public TradeController(ITradesService TradesService, ISharesService SharesService, ITradeRepository TradeRepository, IPortfolioService PortfolioService)
        {

            _tradesServices = TradesService;
            _tradeRepository = TradeRepository;
            _portfolioService = PortfolioService;
            _sharesService = SharesService; 
        }


        [HttpGet("{portfolioid}")]
        public async Task<IActionResult> GetAllTradings([FromRoute]int portFolioid)
        {
            var trade = _tradeRepository.Query().Where(x => x.PortfolioId.Equals(portFolioid));
            return Ok(trade);
        }


        /*************************************************************************************************************************************
        For a given portfolio, with all the registered shares you need to do a trade which could be either a BUY or SELL trade. For a particular trade keep following conditions in mind:
		BUY:
        a) The rate at which the shares will be bought will be the latest price in the database.
		b) The share specified should be a registered one otherwise it should be considered a bad request. 
		c) The Portfolio of the user should also be registered otherwise it should be considered a bad request. 
                
        SELL:
        a) The share should be there in the portfolio of the customer.
		b) The Portfolio of the user should be registered otherwise it should be considered a bad request. 
		c) The rate at which the shares will be sold will be the latest price in the database.
        d) The number of shares should be sufficient so that it can be sold. 
        Hint: You need to group the total shares bought and sold of a particular share and see the difference to figure out if there are sufficient quantities available for SELL. 

        *************************************************************************************************************************************/

        public vmTrade TradeModelIsValid(TradeModel trade)
        {

            try
            {
                //get the portfolio info
                Portfolio portfolio = _portfolioService.GetPortfolio(trade.PortfolioId);

                if (portfolio == null)
                {

                    throw new Exception("Invalid solicitation, portfolio not exists!");
                }

                //get the share
                HourlyShareRate share = _sharesService.GetShare(trade.Symbol);
                if (share == null)
                {
                    throw new Exception("Invalid solicitation, share not exists!");

                }

                //given a object with portfolio and share infos
                vmTrade r = new vmTrade()
                {

                    Share = share,
                    Portfolio = portfolio
                };

                return r;


            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }


        }


        /*
          This action performe a general trades
        */
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]TradeModel model)
        {
            try
            {

                //verify if trademodel is valid
                var _Trade = TradeModelIsValid(model);
                Trade nTrade = new Trade(); 


            //based on the action off trade execute the operation
            if(model.Action == "BUY")
            {
                    nTrade = await _tradesServices.Add(model, _Trade.Share);
            }
            if(model.Action == "SELL")
            {

                //reazon between shares bought and sold to this particular share
                var availableShares = _sharesService.AvailableShares(_Trade.Share, _Trade.Portfolio.Id); 
                if(availableShares > model.NoOfShares)
                {

                        nTrade = await _tradesServices.Add(model, _Trade.Share);
                }else
                    {

                        throw new Exception(string.Format("Invalid solicitation, this portfolio has only {0} available shares", availableShares));
                    }
            }

                return Ok(nTrade);


            }
            catch(Exception ex)
            {

                return BadRequest(ex.Message); 
            }
        }
       
        
        public class vmTrade
        {
            public Portfolio Portfolio { get; set; }
            public HourlyShareRate Share { get; set; }
        }
    }
}
