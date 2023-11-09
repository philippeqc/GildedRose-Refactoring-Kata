using NUnit.Framework;
using NUnit.Framework;
using System.Collections.Generic;
using GildedRoseKata;

namespace GildedRoseTests
{
public class GildedRoseTest
{
    [SetUp]
    public void Setup()
    {
        
    }

    [Test]
    public void Test1()
    {
        IList<Item> items = new List<Item> { new Item { Name = "foo", SellIn = 0, Quality = 0 } };
        GildedRose app = new GildedRose(items);
        app.UpdateQuality();
        Assert.That("fixme", Is.EqualTo(items[0].Name));
    }
}
}