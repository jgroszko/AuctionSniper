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
            SniperSnapshot joining = SniperSnapshot.Joining(ITEM_ID);
            SniperSnapshot bidding = joining.Bidding(555, 666);

            _snipersDataSet.AddSniper(joining);
            _snipersDataSet.SniperStateChanged(bidding);

            AssertRowMatchesSnapshot(0, bidding);
        }

        [TestMethod]
        public void NotifiesListenersWhenAddingASniper()
        {
            SniperSnapshot joining = SniperSnapshot.Joining(ITEM_ID);

            Assert.AreEqual(0, _snipersDataSet.Rows.Count);

            _snipersDataSet.AddSniper(joining);

            Assert.AreEqual(1, _snipersDataSet.Rows.Count);
            AssertRowMatchesSnapshot(0, joining);
        }

        [TestMethod]
        public void HoldsSnipersInAdditionOrder()
        {
            _snipersDataSet.AddSniper(SniperSnapshot.Joining("item 0"));
            _snipersDataSet.AddSniper(SniperSnapshot.Joining("item 1"));

            Assert.AreEqual("item 0", _snipersDataSet.Rows[0][(int)SnipersDataSet.Column.ITEM_IDENTIFIER]);
            Assert.AreEqual("item 1", _snipersDataSet.Rows[1][(int)SnipersDataSet.Column.ITEM_IDENTIFIER]);
        }

        [TestMethod]
        public void UpdatesCorrectRowForSniper()
        {
            SniperSnapshot joining = SniperSnapshot.Joining("item 0");
            SniperSnapshot joining2 = SniperSnapshot.Joining("item 1");

            SniperSnapshot bidding = joining.Bidding(555, 666);
            SniperSnapshot bidding2 = joining2.Bidding(666, 777);

            _snipersDataSet.AddSniper(joining);
            _snipersDataSet.AddSniper(joining2);

            _snipersDataSet.SniperStateChanged(bidding);
            _snipersDataSet.SniperStateChanged(bidding2);

            AssertRowMatchesSnapshot(0, bidding);
            AssertRowMatchesSnapshot(1, bidding2);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void ThrowsDefectIfNoExistingSniperForAnUpdate()
        {
            SniperSnapshot bidding = new SniperSnapshot("item 0", 0, 0, SniperState.Bidding);

            _snipersDataSet.SniperStateChanged(bidding);
        }

        private void AssertRowMatchesSnapshot(int row, SniperSnapshot ss)
        {
            Assert.AreEqual(ss.ItemId, _snipersDataSet.Rows[row][(int)SnipersDataSet.Column.ITEM_IDENTIFIER]);
            Assert.AreEqual(ss.LastPrice.ToString(), _snipersDataSet.Rows[row][(int)SnipersDataSet.Column.LAST_PRICE]);
            Assert.AreEqual(ss.LastBid.ToString(), _snipersDataSet.Rows[row][(int)SnipersDataSet.Column.LAST_BID]);
            Assert.AreEqual(ss.State.ToString(), _snipersDataSet.Rows[row][(int)SnipersDataSet.Column.SNIPER_STATUS]);
        }
    }
}
