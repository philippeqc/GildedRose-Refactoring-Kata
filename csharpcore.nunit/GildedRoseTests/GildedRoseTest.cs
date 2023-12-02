using NUnit.Framework;
using System.Collections.Generic;
using GildedRoseKata;
using System;

namespace GildedRoseTests;

public class GildedRoseTest
{
    private GildedRoseRule ruleCreator = new GildedRoseRule();

    IList<Item> GetSampleItem(string name = "Some Object", int sellIn = 10, int quality = 20)
    {
        IList<Item> items = new List<Item> { new() { Name = name, SellIn = sellIn, Quality = quality } };
        return items;
    }

    int GetFirstItemQuality(IList<Item> items)
    {
        return items[0].Quality;
    }

    int GetFirstItemSellIn(IList<Item> items)
    {
        return items[0].SellIn;
    }

    [SetUp]
    public void Setup(){}
    
    [Test]
    public void SellInDegradeAtTheEndOfEachDay()
    {
        // Arrange
        IList<Item> items = GetSampleItem();
        var expectedSellIn = GetFirstItemSellIn(items) - 1;

        // Act
        GildedRose app = new(items, ruleCreator.rules, ruleCreator.DefaultRule);
        app.UpdateQuality();

        // Assert
        Assert.That(items[0].SellIn, Is.EqualTo(expectedSellIn));
    }
    
    
    [Test]
    public void QualityDegradeAtTheEndOfEachDay()
    {
        // Arrange
        IList<Item> items = GetSampleItem();
        var expectedQuality = GetFirstItemQuality(items) - 1;
    
        // Act
        GildedRose app = new(items, ruleCreator.rules, ruleCreator.DefaultRule);
        app.UpdateQuality();
    
        // Assert
        Assert.That(items[0].Quality, Is.EqualTo(expectedQuality));
    }
    
    [Test]
    public void QualityDegradeTwiceAsFastAfterSellInDate()
    {
        // Arrange
        IList<Item> items = GetSampleItem(sellIn: 0);
        var expectedQuality = GetFirstItemQuality(items) - 2;
    
        // Act
        GildedRose app = new(items, ruleCreator.rules, ruleCreator.DefaultRule);
        app.UpdateQuality();
    
        // Assert
        Assert.That(items[0].Quality, Is.EqualTo(expectedQuality));
    }
    
    [Test]
    public void QualityIsNeverNegative()
    {
        // Arrange
        var quality = 1;
        var expectedQuality = Math.Max(quality - 2, 0);
        IList<Item> items = GetSampleItem(sellIn: 0, quality: quality);
    
        // Act
        GildedRose app = new(items, ruleCreator.rules, ruleCreator.DefaultRule);
        app.UpdateQuality();
    
        // Assert
        Assert.That(items[0].Quality, Is.EqualTo(expectedQuality));
    }
    
    [Test]
    public void AgedBrieQualityIncreasesWithTime()
    {
        // Arrange
        IList<Item> items = GetSampleItem(name: "Aged Brie");
        var expectedQuality = GetFirstItemQuality(items) + 1;
    
        // Act
        GildedRose app = new(items, ruleCreator.rules, ruleCreator.DefaultRule);
        app.UpdateQuality();
    
        // Assert
        Assert.That(items[0].Quality, Is.EqualTo(expectedQuality));
    }
    
    [Test]
    public void TheQualityNeverRaisesOver50()
    {
        // Arrange
        var quality = 50;
        var expectedQuality = quality;
        IList<Item> items = GetSampleItem(name: "Aged Brie", quality: quality);
    
        // Act
        GildedRose app = new(items, ruleCreator.rules, ruleCreator.DefaultRule);
        app.UpdateQuality();
    
        // Assert
        Assert.That(items[0].Quality, Is.EqualTo(expectedQuality));
    }
    
    [Test]
    public void SulfurasDontHaveToBeSold()
    {
        // Arrange
        IList<Item> items = GetSampleItem(name: "Sulfuras, Hand of Ragnaros");
        var expectedSellIn = GetFirstItemSellIn(items);
    
        // Act
        GildedRose app = new(items, ruleCreator.rules, ruleCreator.DefaultRule);
        app.UpdateQuality();
    
        // Assert
        Assert.That(items[0].SellIn, Is.EqualTo(expectedSellIn));
    }
    
    [Test]
    public void SulfurasNeverChangeQuality()
    {
        // Arrange
        var quality = 80;
        var expectedQuality = quality;
        IList<Item> items = GetSampleItem(name: "Sulfuras, Hand of Ragnaros", quality: quality);
    
        // Act
        GildedRose app = new(items, ruleCreator.rules, ruleCreator.DefaultRule);
        app.UpdateQuality();
    
        // Assert
        Assert.That(items[0].Quality, Is.EqualTo(expectedQuality));
    }
    
    [Test]
    public void BackstagePassesIncreaseInQualityWithTime()
    {
        // Arrange
        var moreThan10Days = 20;
        IList<Item> items = GetSampleItem(name: "Backstage passes to a TAFKAL80ETC concert", sellIn: moreThan10Days);
        var expectedQuality = GetFirstItemQuality(items) + 1;
    
        // Act
        GildedRose app = new(items, ruleCreator.rules, ruleCreator.DefaultRule);
        app.UpdateQuality();
    
        // Assert
        Assert.That(items[0].Quality, Is.EqualTo(expectedQuality));
    }
    
    [Test]
    public void BackstagePassesIncreaseInQualityBy2WhenWithin10DaysOfConcert()
    {
        // Arrange
        var within10Days = 10;
        IList<Item> items = GetSampleItem(name: "Backstage passes to a TAFKAL80ETC concert", sellIn: within10Days);
        var expectedQuality = GetFirstItemQuality(items) + 2;
    
        // Act
        GildedRose app = new(items, ruleCreator.rules, ruleCreator.DefaultRule);
        app.UpdateQuality();
    
        // Assert
        Assert.That(items[0].Quality, Is.EqualTo(expectedQuality));
    }
    
    [Test]
    public void BackstagePassesIncreaseInQualityBy3WhenWithin5DaysOfConcert()
    {
        // Arrange
        var within5Days = 5;
        IList<Item> items = GetSampleItem(name: "Backstage passes to a TAFKAL80ETC concert", sellIn: within5Days);
        var expectedQuality = GetFirstItemQuality(items) + 3;
    
        // Act
        GildedRose app = new(items, ruleCreator.rules, ruleCreator.DefaultRule);
        app.UpdateQuality();
    
        // Assert
        Assert.That(items[0].Quality, Is.EqualTo(expectedQuality));
    }
    
    [Test, Description("Does it display on success")]
    public void BackstagePassesLooseAllQualityAfterConcert()
    {
        // Arrange
        var expectedQuality = 0;
        var sellInAfterConcert = 0;
        IList<Item> items = GetSampleItem(name: "Backstage passes to a TAFKAL80ETC concert", sellIn: sellInAfterConcert);
    
        // Act
        GildedRose app = new(items, ruleCreator.rules, ruleCreator.DefaultRule);
        app.UpdateQuality();
    
        // Assert
        Assert.That(items[0].Quality, Is.EqualTo(expectedQuality));
    }
    
    [Test, Description("Does it display on failure")]
    [Ignore("Conjured items not yet implemented")]
    public void ConjuredItemQualityDegradeTwiceAsFast()
    {
        // Arrange
        IList<Item> items = GetSampleItem(name: "Conjured Mana Cake");
        var expectedQuality = GetFirstItemQuality(items) - 2;
    
        // Act
        GildedRose app = new(items, ruleCreator.rules, ruleCreator.DefaultRule);
        app.UpdateQuality();
    
        // Assert
        Assert.That(items[0].Quality, Is.EqualTo(expectedQuality));
    }
}