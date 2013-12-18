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
            Assert.AreEqual(itemId, Grid.Rows[0].Cells[0].Text);
            Assert.AreEqual(status, Grid.Rows[0].Cells[3].Text);
        }

        public virtual void ShowsSniperStatus(string itemId, int lastPrice, int lastBid, string status)
        {
            Assert.AreEqual(itemId,                 Grid.Rows[0].Cells[0].Text);
            Assert.AreEqual(lastPrice.ToString(),   Grid.Rows[0].Cells[1].Text);
            Assert.AreEqual(lastBid.ToString(),     Grid.Rows[0].Cells[2].Text);
            Assert.AreEqual(status,                 Grid.Rows[0].Cells[3].Text);
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
