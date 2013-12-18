using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace AuctionSniper.Common
{
    public class SnipersDataSet : DataTable
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

            Rows.Add(new object[] {
                string.Empty,
                0,
                0,
                SniperState.Joining
            });
        }

        public virtual void SetStatusText(string statusText)
        {
            Rows[0][(int)Column.SNIPER_STATUS] = statusText;
        }

        public virtual void SniperStatusChanged(SniperSnapshot sniperState)
        {
            Rows[0][(int)Column.ITEM_IDENTIFIER] = sniperState.ItemId;
            Rows[0][(int)Column.LAST_BID] = sniperState.LastBid;
            Rows[0][(int)Column.LAST_PRICE] = sniperState.LastPrice;
            Rows[0][(int)Column.SNIPER_STATUS] = sniperState.State.ToString();
        }
    }
}
