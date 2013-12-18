using AuctionSniper.Common;
using AuctionSniper.Common.Interfaces;
using AuctionSniper.Common.Services;
using jabber.protocol.client;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AuctionSniper.MainWindow
{
    class MainWindowViewModel : BaseViewModel
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(MainWindowViewModel));

        public const string CONFIG_JID = "jid";
        public const string CONFIG_PASSWORD = "password";
        public const string CONFIG_HOST = "host";
        public const string CONFIG_AUCTION_USER_FORMAT = "auction-user";

        private XmppService _xmpp;
        private Auction _auction;
        
        private string[] _auctionIds;
        public string[] AuctionIds
        {
            get
            {
                return _auctionIds;
            }
            set
            {
                if (_auctionIds != value)
                {
                    _auctionIds = value;

                    RaisePropertyChanged(() => this.AuctionIds);
                }
            }
        }

        private SnipersDataSet _snipersStatus = new SnipersDataSet();
        public SnipersDataSet SnipersStatus
        {
            get
            {
                return _snipersStatus;
            }
            set
            {
                _snipersStatus = value;
                RaisePropertyChanged(() => this.SnipersStatus);
            }
        }

        public MainWindowViewModel()
        {
            if (Environment.GetCommandLineArgs().Length < 2)
                throw new Exception("Must specify an auction id!");

            AuctionIds = Environment.GetCommandLineArgs()[1].Split(new char[] { ',' });

            string jid = ConfigurationManager.AppSettings[CONFIG_JID];
            _xmpp = new XmppService(jid,
                                    ConfigurationManager.AppSettings[CONFIG_PASSWORD],
                                    ConfigurationManager.AppSettings[CONFIG_HOST]);

            _xmpp.Connect();

            foreach (string itemId in AuctionIds)
            {
                JoinAuction(itemId);
            }
        }

        public void JoinAuction(string itemId)
        {
            string auctionUser = string.Format(ConfigurationManager.AppSettings[CONFIG_AUCTION_USER_FORMAT], itemId);

            _auction = new Auction(auctionUser);

            _xmpp.AddMessageHandler(new AuctionMessageTranslator(_xmpp.GetUser(),
                new AuctionSniperService(itemId, _auction, SnipersStatus)),
                auctionUser);

            _auction.XmppService = _xmpp;

            _auction.Join();
        }
    }
}
