using AuctionSniper.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionSniperTests
{
    [TestClass]
    public class SnipersDataSetTests
    {
        private const string ITEM_ID = "item id";

        private SnipersDataSet _snipersDataSet;

        [TestInitialize]
        public void Initialize()
        {
            _snipersDataSet = new SnipersDataSet();
        }

        [TestMethod]
        public void HasEnoughColumns()
        {
            Assert.AreEqual(Enum.GetNames(typeof(SnipersDataSet.Column)).Length,
                _snipersDataSet.Columns.Count);
        }

        [TestMethod]
        public void SetsSniperValuesInColumns()
        {
            _snipersDataSet.SniperStatusChanged(new SniperSnapshot(ITEM_ID, 555, 666, SniperState.Bidding));

            Assert.AreEqual(ITEM_ID, _snipersDataSet.Rows[0][(int)SnipersDataSet.Column.ITEM_IDENTIFIER]);
            Assert.AreEqual("555", _snipersDataSet.Rows[0][(int)SnipersDataSet.Column.LAST_PRICE]);
            Assert.AreEqual("666",     _snipersDataSet.Rows[0][(int)SnipersDataSet.Column.LAST_BID]);
            Assert.AreEqual(Constants.STATUS_BIDDING,
                                     _snipersDataSet.Rows[0][(int)SnipersDataSet.Column.SNIPER_STATUS]);
        }
    }
}
