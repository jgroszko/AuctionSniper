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

        Application _app;
        WorkSession _workSession;
        ScreenRepository _screenRepository;

        private string _auctionId;

        public MainWindow Window
        {
            get
            {
                return _screenRepository.Get<MainWindow>("MainWindow", InitializeOption.NoCache);
            }
        }

        public void StartBiddingIn(string auctionId)
        {
            _auctionId = auctionId;

            LaunchApplication();

            Window.ShowsSniperStatus(_auctionId, AuctionSniper.Common.Constants.STATUS_JOINING);
        }

        #region Helpers
        private void LaunchApplication()
        {
            var directoryName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var location = Path.Combine(directoryName, @"AuctionSniper.exe");
            _app = Application.Launch(new ProcessStartInfo(
                location, _auctionId));

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

        public virtual void ShowsSniperHasLostAuction()
        {
            Window.ShowsSniperStatus(_auctionId, AuctionSniper.Common.Constants.STATUS_LOST);
        }

        public virtual void ShowsSniperHasWonAuction(int lastPrice)
        {
            Window.ShowsSniperStatus(_auctionId, lastPrice, lastPrice, AuctionSniper.Common.Constants.STATUS_WON);
        }

        public virtual void HasShownSniperIsBidding(int lastPrice, int lastBid)
        {
            Window.ShowsSniperStatus(_auctionId, lastPrice, lastBid, AuctionSniper.Common.Constants.STATUS_BIDDING);
        }

        public virtual void HasShownSniperIsWinning(int winningBid)
        {
            Window.ShowsSniperStatus(_auctionId, winningBid, winningBid, AuctionSniper.Common.Constants.STATUS_WINNING);
        }
    }
}
