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
        (GildedRose app, IList<Item> items) = GetAppWithSampleItem(sellIn: sellIn);

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
        (GildedRose app, IList<Item> items) = GetAppWithSampleItem(quality: quality);

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
        (GildedRose app, IList<Item> items) = GetAppWithSampleItem(sellIn: 0, quality: quality);

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
        (GildedRose app, IList<Item> items) = GetAppWithSampleItem(quality: quality);

        // Act
        app.UpdateQuality();

        // Assert
        Assert.That(items[0].Quality, Is.EqualTo(expectedQuality));
    }


    [Test]
    public void AgedBrieRaiseQualityWithTime()
    {
        // Arrange
        var quality = 10;
        var expectedQuality = quality + 1;
        (GildedRose app, IList<Item> items) = GetAppWithSampleItem(name: "Aged Brie", quality: quality);

        // Act
        app.UpdateQuality();

        // Assert
        Assert.That(items[0].Quality, Is.EqualTo(expectedQuality));
    }

    public (GildedRose, IList<Item>) GetAppWithSampleItem(string name = "foo", int sellIn = 10, int quality = 10) {
        IList<Item> items = new List<Item> { new Item { Name = name, SellIn = sellIn, Quality = quality } };
        GildedRose app = new GildedRose(items);

        return (app, items);
    }
}