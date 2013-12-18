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
        }

        public virtual void AddSniper(SniperSnapshot sniperSnapshot)
        {
            Rows.Add(new object[] {
                sniperSnapshot.ItemId,
                sniperSnapshot.LastBid,
                sniperSnapshot.LastPrice,
                sniperSnapshot.State
            });
        }

        public virtual void SniperStateChanged(SniperSnapshot sniperSnapshot)
        {
            foreach(DataRow row in Rows)
            {
                if(row[0].ToString() == sniperSnapshot.ItemId)
                {
                    row[(int)Column.LAST_BID] = sniperSnapshot.LastBid;
                    row[(int)Column.LAST_PRICE] = sniperSnapshot.LastPrice;
                    row[(int)Column.SNIPER_STATUS] = sniperSnapshot.State.ToString();
                    return;
                }
            }

            throw new Exception("Unknown auction id");
        }
    }
}
