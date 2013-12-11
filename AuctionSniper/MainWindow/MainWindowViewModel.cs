using AuctionSniper.Common;
using AuctionSniper.Services;
using jabber.protocol.client;
using System;
using System.Collections.Generic;
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

            XmppService.Instance.OnAuthenticated += Authenticated;
            XmppService.Instance.OnMessage += Message;

            XmppService.Instance.Connect();
        }

        private void Authenticated(object sender, EventArgs e)
        {
            XmppService.Instance.Message(AuctionUser, SOLProtocol.JOIN_COMMAND_FORMAT);

            Status = "Joining";
        }

        private void Message(object sender, Message message)
        {
            Status = "Lost";
        }
    }
}
