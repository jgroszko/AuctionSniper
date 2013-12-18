using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionSniper.Common
{
    public enum SniperState
    {
        Joining,
        Bidding,
        Winning,
        Lost,
        Won
    }

    public class SniperSnapshot : IEquatable<SniperSnapshot>
    {
        #region Properties
        private string _itemId;

        public string ItemId
        {
            get { return _itemId; }
        }

        private int _lastPrice;

        public int LastPrice
        {
            get { return _lastPrice; }
        }

        private int _lastBid;

        public int LastBid
        {
            get { return _lastBid; }
        }

        private SniperState _state;

        public SniperState State
        {
            get { return _state; }
        }
        #endregion

        #region Constructor

        public SniperSnapshot(string itemId, int lastPrice, int lastBid, SniperState state)
        {
            _itemId = itemId;
            _lastPrice = lastPrice;
            _lastBid = lastBid;
            _state = state;
        }

        public static SniperSnapshot Joining(string itemId)
        {
            return new SniperSnapshot(itemId, 0, 0, SniperState.Joining);
        }

        #endregion

        #region Helpers

        public override string ToString()
        {
            return string.Format("Item ID {0}, Last Price {1}, Last Bid {2}, State {3}", ItemId, LastPrice, LastBid, State);
        }

        public bool Equals(SniperSnapshot b)
        {
            return b.ItemId == ItemId && b.LastPrice == LastPrice && b.LastBid == LastBid && b.State == State;
        }

        #endregion

        #region Public Methods

        public SniperSnapshot Winning(int price)
        {
            return new SniperSnapshot(_itemId, price, _lastBid, SniperState.Winning);
        }

        public SniperSnapshot Bidding(int price, int bid)
        {
            return new SniperSnapshot(_itemId, price, bid, SniperState.Bidding);
        }

        public SniperSnapshot Closed()
        {
            SniperState whenClosed;
            switch(_state)
            {
                case SniperState.Joining:
                    whenClosed = SniperState.Lost;
                    break;
                case SniperState.Bidding:
                    whenClosed = SniperState.Lost;
                    break;
                case SniperState.Winning:
                    whenClosed = SniperState.Won;
                    break;
                default:
                    throw new Exception("Auction is already closed");
            }

            return new SniperSnapshot(_itemId, _lastPrice, _lastBid, whenClosed);
        }

        #endregion
    }
}
