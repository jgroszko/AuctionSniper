using AuctionSniper.Common;
using FakeItEasy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionSniperTests
{
    public static class ArgumentConstraintManagerExtensions
    {
        public static SniperSnapshot HasState(this IArgumentConstraintManager<SniperSnapshot> iacm, SniperState state)
        {
            return iacm.Matches(ss => ss.State == state);
        }
    }
}
