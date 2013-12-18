using AuctionSniper.Common;
using AuctionSniper.Common.Interfaces;
using AuctionSniper.Common.Services;
using FakeItEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionSniperTests
{
    [TestClass]
    public class AuctionSniperServiceTests
    {
        public const string ITEM_ID = "5432";

        ISniperListener sniperListener;
        Auction auction;
        AuctionSniperService sniper;

        [TestInitialize]
        public void Initialize()
        {
            sniperListener = A.Fake<ISniperListener>();
            auction = A.Fake<Auction>(x => x.WithArgumentsForConstructor(new object[] {string.Empty}));
            sniper = new AuctionSniperService(ITEM_ID, auction, sniperListener);
        }

        [TestMethod]
        public void ReportsLostIfAuctionClosesImmediately()
        {
            sniper.AuctionClosed();

            A.CallTo(() => sniperListener.SniperStateChanged(
                A<SniperSnapshot>.That.HasState(SniperState.Lost)
                )).MustHaveHappened(Repeated.AtLeast.Once);
        }

        [TestMethod]
        public void ReportsLostIfAuctionClosesWhenBidding()
        {
            using (var scope = Fake.CreateScope())
            {
                sniper.CurrentPrice(123, 45, PriceSource.FromOtherBidder);
                sniper.AuctionClosed();

                using (scope.OrderedAssertions())
                {
                    A.CallTo(() => sniperListener.SniperStateChanged(
                        A<SniperSnapshot>.That.HasState(SniperState.Bidding)
                        )).MustHaveHappened(Repeated.AtLeast.Once);
                    A.CallTo(() => sniperListener.SniperStateChanged(
                        A<SniperSnapshot>.That.HasState(SniperState.Lost)
                        )).MustHaveHappened(Repeated.AtLeast.Once);
                }
            }
        }

        [TestMethod]
        public void ReportsWonIfAuctionClosesWhenWinning()
        {
            using (var scope = Fake.CreateScope())
            {
                sniper.CurrentPrice(123, 45, PriceSource.FromSniper);
                sniper.AuctionClosed();

                using(scope.OrderedAssertions())
                {
                    A.CallTo(() => sniperListener.SniperStateChanged(
                        A<SniperSnapshot>.That.HasState(SniperState.Winning)
                        )).MustHaveHappened(Repeated.AtLeast.Once);
                    A.CallTo(() => sniperListener.SniperStateChanged(
                        A<SniperSnapshot>.That.HasState(SniperState.Won)
                        )).MustHaveHappened(Repeated.AtLeast.Once);
                }
            }
        }

        [TestMethod]
        public void ReportsIsWinningWhenCurrentPriceComesFromSniper()
        {
            using (var scope = Fake.CreateScope())
            {
                sniper.CurrentPrice(123, 12, PriceSource.FromOtherBidder);
                sniper.CurrentPrice(135, 45, PriceSource.FromSniper);

                using (scope.OrderedAssertions())
                {
                    A.CallTo(() => sniperListener.SniperStateChanged(
                        A<SniperSnapshot>.That.Matches(ss => ss.State == SniperState.Bidding)
                        )).MustHaveHappened(Repeated.AtLeast.Once);
                    A.CallTo(() => sniperListener.SniperStateChanged(
                        A<SniperSnapshot>.That.Matches(ss => ss.State == SniperState.Winning)
                        )).MustHaveHappened(Repeated.AtLeast.Once);
                }
            }
        }

        [TestMethod]
        public void BidsHigherAndReportsBiddingWhenNewPriceArrives()
        {
            int price = 1001;
            int increment = 25;
            int bid = price + increment;
            SniperSnapshot state = new SniperSnapshot(ITEM_ID, price, bid, SniperState.Bidding);

            sniper.CurrentPrice(price, increment, PriceSource.FromOtherBidder);

            A.CallTo(() => sniperListener.SniperStateChanged(A<SniperSnapshot>.That.Matches(ss => ss.Equals(state))))
                .MustHaveHappened(Repeated.AtLeast.Once);
            A.CallTo(() => auction.Bid(price + increment)).MustHaveHappened(Repeated.Exactly.Once);
        }
    }
}
