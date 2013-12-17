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

        public virtual void ShowsSniperStatus(string itemId, string status)
        {
            var elem = Window.AutomationElement;
            var table = Window.Get<ListView>("snipersTable");
            Assert.AreEqual(itemId, table.Rows[0].Cells[0].Text);
            Assert.AreEqual(status, table.Rows[0].Cells[3].Text);
        }

        public virtual void ShowsSniperStatus(string itemId, int lastPrice, int lastBid, string status)
        {
            var elem = Window.AutomationElement;
            var table = Window.Get<ListView>("snipersTable");
            Assert.AreEqual(itemId,                 table.Rows[0].Cells[0].Text);
            Assert.AreEqual(lastPrice.ToString(),   table.Rows[0].Cells[1].Text);
            Assert.AreEqual(lastBid.ToString(),     table.Rows[0].Cells[2].Text);
            Assert.AreEqual(status,                 table.Rows[0].Cells[3].Text);
        }
    }
}
