using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionSniper.Common.Interfaces
{
    public interface ISniperListener
    {
        void SniperStateChanged(SniperSnapshot ss);
    }
}
