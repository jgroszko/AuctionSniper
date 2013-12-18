using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestStack.White.ScreenObjects;
using TestStack.White.UIItems;
using TestStack.White.UIItems.WindowItems;

namespace AuctionSniperTests
{
    public class MainWindow : AppScreen
    {
        public MainWindow(Window window, ScreenRepository screenRepository) : base(window, screenRepository)
        {
        }

        public virtual ListView Grid
        {
            get
            {
                return Window.Get<ListView>("snipersTable");
            }
        }

        public virtual void ShowsSniperStatus(string itemId, string status)
        {
            foreach(ListViewRow row in Grid.Rows)
            {
                if(row.Cells[0].Text == itemId)
                {
                    Assert.AreEqual(status, row.Cells[3].Text);
                    return;
                }
            }

            Assert.Fail(string.Format("Could not find auction {0}", itemId));
        }

        public virtual void ShowsSniperStatus(string itemId, int lastPrice, int lastBid, string status)
        {
            foreach (ListViewRow row in Grid.Rows)
            {
                if (row.Cells[0].Text == itemId)
                {
                    Assert.AreEqual(lastPrice.ToString(), row.Cells[1].Text);
                    Assert.AreEqual(lastBid.ToString(), row.Cells[2].Text);
                    Assert.AreEqual(status, row.Cells[3].Text);
                    return;
                }
            }

            Assert.Fail(string.Format("Could not find auction {0}", itemId));
        }

        public virtual void HasTitle(string title)
        {
            Assert.AreEqual(title, Window.Title);
        }

        public virtual void GridHasColumnTitles()
        {
            Assert.AreEqual("Auction",      Grid.Header.Columns[0].Text);
            Assert.AreEqual("Last Price",   Grid.Header.Columns[1].Text);
            Assert.AreEqual("Last Bid",     Grid.Header.Columns[2].Text);
            Assert.AreEqual("Status",       Grid.Header.Columns[3].Text);
        }
    }
}
