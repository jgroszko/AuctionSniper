using AuctionSniper.Common;
using AuctionSniper.Common.Interfaces;
using AuctionSniper.Common.Services;
using jabber.protocol.client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AuctionSniper.MainWindow
{
    class MainWindowViewModel : BaseViewModel, ISniperListener
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(MainWindowViewModel));

        public const string CONFIG_JID = "jid";
        public const string CONFIG_PASSWORD = "password";
        public const string CONFIG_HOST = "host";

        private XmppService _xmpp;
        private Auction _auction;
        
        private int _auctionId;
        public int AuctionId
        {
            get
            {
                return _auctionId;
            }
            set
            {
                if (_auctionId != value)
                {
                    _auctionId = value;

                    RaisePropertyChanged(() => this.AuctionId);
                }
            }
        }

        public string AuctionUser
        {
            get
            {
                return string.Format("auction-{0}@jgroszko-server",
                    AuctionId);
            }
        }

        private string _status;
        public string Status
        {
            get
            {
                return _status;
            }
            set
            {
                if (_status != value)
                {
                    _status = value;

                    RaisePropertyChanged(() => this.Status);
                }
            }
        }

        public MainWindowViewModel()
        {
            if (Environment.GetCommandLineArgs().Length < 2)
                throw new Exception("Must specify an auction id!");

            AuctionId = int.Parse(Environment.GetCommandLineArgs()[1]);

            JoinAuction();
        }

        public void JoinAuction()
        {
            _auction = new Auction(AuctionUser);

            string jid = ConfigurationManager.AppSettings[CONFIG_JID];

            _xmpp = new XmppService(jid,
                                    ConfigurationManager.AppSettings[CONFIG_PASSWORD],
                                    ConfigurationManager.AppSettings[CONFIG_HOST],
                                    new AuctionMessageTranslator(
                                        jid,
                                        new AuctionSniperService(_auction,
                                            this)));

            _auction.XmppService = _xmpp;

            _xmpp.Connect();

            _auction.Join();

            Status = "Joining";
        }

        public void SniperLost()
        {
            Status = "Lost";
        }

        public void SniperBidding()
        {
            Status = "Bidding";
        }

        public void SniperWinning()
        {
            Status = "Winning";
        }

        public void SniperWon()
        {
            Status = "Won";
        }
    }
}
