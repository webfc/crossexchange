using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CrossExchange.Interface
{
    public interface ISharesService
    {

        HourlyShareRate GetShare(string simbol);

        int AvailableShares(HourlyShareRate share, int portfolioId);
    }
}
