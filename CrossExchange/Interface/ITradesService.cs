using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CrossExchange.Interface
{
    public interface ITradesService
    {

        Task<Trade> Add(TradeModel trade, HourlyShareRate share);
    }
}
