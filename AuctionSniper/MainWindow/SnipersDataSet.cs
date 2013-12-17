using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace AuctionSniper.MainWindow
{
    public class SnipersDataSet : DataTable
    {
        public SnipersDataSet()
        {
            Columns.Add("Status");

            Rows.Add(new string[] { string.Empty });
        }

        public void SetStatusText(string statusText)
        {
            Rows[0][0] = statusText;
        }
    }
}
