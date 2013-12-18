using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Diagnostics;
using AuctionSniper.Common.Interfaces;

namespace AuctionSniper.Common
{
    public class SnipersDataSet : DataTable, ISniperListener
    {
        public enum Column
        {
            ITEM_IDENTIFIER,
            LAST_PRICE,
            LAST_BID,
            SNIPER_STATUS
        }

        public SnipersDataSet()
        {
            Columns.Add("Auction");
            Columns.Add("Last Price");
            Columns.Add("Last Bid");
            Columns.Add("Status");

            Rows.Add(new object[] { });
        }

        public virtual void SniperStateChanged(SniperSnapshot sniperSnapshot)
        {
            Debug.WriteLine("State Changed, ID " + sniperSnapshot.ItemId);
            Rows[0][(int)Column.ITEM_IDENTIFIER] = sniperSnapshot.ItemId;
            Rows[0][(int)Column.LAST_BID] = sniperSnapshot.LastBid;
            Rows[0][(int)Column.LAST_PRICE] = sniperSnapshot.LastPrice;
            Rows[0][(int)Column.SNIPER_STATUS] = sniperSnapshot.State.ToString();
        }
    }
}
