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

        public virtual void ShowsSniperStatus(string statusText)
        {
            var statusLabel = Window.Get<Label>("statusText");
            Assert.AreEqual(statusText, statusLabel.Text);
        }
    }
}
