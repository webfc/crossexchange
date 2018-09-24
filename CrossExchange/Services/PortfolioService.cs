using CrossExchange.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CrossExchange.Services
{
    public class PortfolioService : IPortfolioService
    {

        private readonly IPortfolioRepository _portfolioRepository; 

        public PortfolioService(IPortfolioRepository portfolioRepository)
        {

            _portfolioRepository = portfolioRepository;

        }


        //get the portfolio
        public Portfolio GetPortfolio(int PortFolioId)
        {

            try
            {
                return _portfolioRepository.Query().Where(x => x.Id == PortFolioId).FirstOrDefault(); 

            }catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
