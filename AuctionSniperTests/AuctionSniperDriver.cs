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
        public const string STATUS_JOINING = "Joining";
        public const string STATUS_LOST = "Lost";

        Application _app;
        WorkSession _workSession;
        ScreenRepository _screenRepository;

        public MainWindow Window
        {
            get
            {
                return _screenRepository.Get<MainWindow>("MainWindow", InitializeOption.NoCache);
            }
        }

        public void StartBiddingIn(string auctionId)
        {
            LaunchApplication(auctionId);

            Window.ShowsSniperStatus(STATUS_JOINING);
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

        internal void ShowsSniperHasLostAuction()
        {
            Window.ShowsSniperStatus(STATUS_LOST);
        }
    }
}
