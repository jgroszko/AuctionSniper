using AuctionSniper.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TestStack.White;
using TestStack.White.Factory;
using TestStack.White.ScreenObjects;
using TestStack.White.ScreenObjects.Services;
using TestStack.White.ScreenObjects.Sessions;
using TestStack.White.UIItems;
using TestStack.White.UIItems.WindowItems;
using TestStack.White.UIItems.WPFUIItems;

namespace AuctionSniperTests
{
    public class AuctionSniperDriver : IDisposable
    {
        public const string SNIPER_ID = "sniper@jgroszko-server";
        public const string WINDOW_TITLE = "Auction Sniper";

        Application _app;
        WorkSession _workSession;
        ScreenRepository _screenRepository;

        public MainWindow Window
        {
            get
            {
                return _screenRepository.Get<MainWindow>(WINDOW_TITLE, InitializeOption.NoCache);
            }
        }

        public void StartBiddingIn(params FakeAuctionServer[] auctions)
        {
            LaunchApplication(string.Join(",", auctions.Select(a => a.ItemId).ToArray()));

            Window.HasTitle(WINDOW_TITLE);
            Window.GridHasColumnTitles();

            foreach (FakeAuctionServer auction in auctions)
            {
                Window.ShowsSniperStatus(auction.ItemId, AuctionSniper.Common.Constants.STATUS_JOINING);
            }
        }

        #region Helpers
        private void LaunchApplication(string auctionId)
        {
            var directoryName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var location = Path.Combine(directoryName, @"AuctionSniper.exe");
            _app = Application.Launch(new ProcessStartInfo(
                location, auctionId));

            var workConfiguration =
                new WorkConfiguration
                {
                    ArchiveLocation = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                    Name = "AuctionSniper"
                };

            _workSession = new WorkSession(workConfiguration, new NullWorkEnvironment());

            _screenRepository = _workSession.Attach(_app);
        }


        public void Dispose()
        {
            if (_workSession != null)
                _workSession.Dispose();

            if(_app != null)
                _app.Dispose();
        }
        #endregion

        public virtual void ShowsSniperHasLostAuction(FakeAuctionServer auction)
        {
            Window.ShowsSniperStatus(auction.ItemId, AuctionSniper.Common.Constants.STATUS_LOST);
        }

        public virtual void ShowsSniperHasWonAuction(FakeAuctionServer auction, int lastPrice)
        {
            Window.ShowsSniperStatus(auction.ItemId, lastPrice, lastPrice, AuctionSniper.Common.Constants.STATUS_WON);
        }

        public virtual void HasShownSniperIsBidding(FakeAuctionServer auction, int lastPrice, int lastBid)
        {
            Window.ShowsSniperStatus(auction.ItemId, lastPrice, lastBid, AuctionSniper.Common.Constants.STATUS_BIDDING);
        }

        public virtual void HasShownSniperIsWinning(FakeAuctionServer auction, int winningBid)
        {
            Window.ShowsSniperStatus(auction.ItemId, winningBid, winningBid, AuctionSniper.Common.Constants.STATUS_WINNING);
        }
    }
}
