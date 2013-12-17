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

        public SniperSnapshot(string itemId, int lastPrice, int lastBid, SniperState state)
        {
            _itemId = itemId;
            _lastPrice = lastPrice;
            _lastBid = lastBid;
            _state = state;
        }
        
        public override string ToString()
        {
            return string.Format("Item ID {0}, Last Price {1}, Last Bid {2}, State {3}", ItemId, LastPrice, LastBid, State);
        }

        public bool Equals(SniperSnapshot b)
        {
            return b.ItemId == ItemId && b.LastPrice == LastPrice && b.LastBid == LastBid && b.State == State;
        }
    }
}
