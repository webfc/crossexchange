using CrossExchange.Controller;
using CrossExchange.Interface;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using static CrossExchange.Controller.TradeController;

namespace CrossExchange.Tests
{
    public class TradeControllerTests
    {
        private readonly Mock<IShareRepository> _shareRepositoryMock = new Mock<IShareRepository>();
        private readonly Mock<ITradeRepository> _tradeRepositoryMock = new Mock<ITradeRepository>();
        private readonly Mock<ISharesService> _sharesServiceMock = new Mock<ISharesService>();
        private readonly Mock<ITradesService> _tradesServiceMock = new Mock<ITradesService>();
        private readonly Mock<IPortfolioService> _portfolioServiceMock = new Mock<IPortfolioService>(); 

        private readonly TradeController _tradeController;

        public TradeControllerTests()
        {
            _tradeController = new TradeController(_tradesServiceMock.Object, _sharesServiceMock.Object, _tradeRepositoryMock.Object,_portfolioServiceMock.Object);
        }


        [Test]
        public async Task Get_TestAllTrades()
        {

            int portfolioId = 1;

            var result = await _tradeController.GetAllTradings(portfolioId);
            // Assert
            Assert.NotNull(result);

            var createdResult = result as CreatedResult;
            Assert.NotNull(createdResult);
            Assert.AreEqual(201, createdResult.StatusCode);
        }


        [Test]
        public TradeModel Private_ValidTradeModel()
        {

            TradeModel _tradeModel = new TradeModel()
            {
                Symbol = "CBI",
                NoOfShares = 50,
                PortfolioId = 1,
                Action = "BUY"                
            }; 

            var result = _tradeController.TradeModelIsValid(_tradeModel);
            // Assert
            Assert.NotNull(result);

            return _tradeModel; 
        }


        [Test]
        public async Task Post_TestCreateTrade()
        {
            TradeModel _tradeModel = new TradeModel()
            {
                Symbol = "CBI",
                NoOfShares = 50,
                PortfolioId = 1,
                Action = "BUY"
            };

            var resultBuy = await _tradeController.Post(_tradeModel);

            // Assert
            Assert.NotNull(resultBuy);

            var createdResult = resultBuy as CreatedResult;
            Assert.NotNull(createdResult);
            Assert.AreEqual(201, createdResult.StatusCode);

            TradeModel _tradeModelSell = new TradeModel()
            {
                Symbol = "REL",
                NoOfShares = 50,
                PortfolioId = 1,
                Action = "SELL"
            };

            var resultSell = await _tradeController.Post(_tradeModelSell);

            var createdResultSell = resultSell as CreatedResult;
            Assert.NotNull(createdResultSell);
            Assert.AreEqual(201, createdResultSell.StatusCode);

        }
    }
}
