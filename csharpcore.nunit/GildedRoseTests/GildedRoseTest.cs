using NUnit.Framework;
using System.Collections.Generic;
using GildedRoseKata;

namespace GildedRoseTests;

public class GildedRoseTest
{
    [SetUp]
    public void Setup()
    {

    }

    [Test]
    public void SellInDecreaseByOneEachDay()
    {
        // Arrange
        var sellIn = 10;
        var expectedSellIn = sellIn - 1;
        IList<Item> items = GetSampleItem(sellIn: sellIn);
        GildedRose app = new GildedRose(items);

        // Act
        app.UpdateQuality();

        // Assert
        Assert.That(items[0].SellIn, Is.EqualTo(expectedSellIn));
    }

    [Test]
    public void QualityDecreaseByOneEachDay()
    {
        // Arrange
        var quality = 10;
        var expectedQuality = quality - 1;
        IList<Item> items = GetSampleItem(quality: quality);
        GildedRose app = new GildedRose(items);

        // Act
        app.UpdateQuality();

        // Assert
        Assert.That(items[0].Quality, Is.EqualTo(expectedQuality));
    }


    [Test]
    public void OnceSellInDatePassedQualityDecreaseTwiceAsFast()
    {
        // Arrange
        var quality = 10;
        var expectedQuality = quality - 2;
        IList<Item> items = GetSampleItem(sellIn: 0, quality: quality);
        GildedRose app = new GildedRose(items);

        // Act
        app.UpdateQuality();

        // Assert
        Assert.That(items[0].Quality, Is.EqualTo(expectedQuality));
    }

    [Test]
    public void QualityOfItemIsNeverNegative()
    {
        // Arrange
        var quality = 0;
        var expectedQuality = 0;
        IList<Item> items = GetSampleItem(quality: quality);

        GildedRose app = new GildedRose(items);

        // Act
        app.UpdateQuality();

        // Assert
        Assert.That(items[0].Quality, Is.EqualTo(expectedQuality));
    }

    public IList<Item>  GetSampleItem(string name = "foo", int sellIn = 10, int quality = 10) {
        IList<Item> items = new List<Item> { new Item { Name = name, SellIn = sellIn, Quality = quality } };
        return items;
    }
}