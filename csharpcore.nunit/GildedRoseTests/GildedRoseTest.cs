using NUnit.Framework;
using System.Collections.Generic;
using GildedRoseKata;
using System;

namespace GildedRoseTests;

public class GildedRoseTest
{
    protected Item GetSampleItem(string name = "Some Object", int sellIn = 10, int quality = 20)
    {
        Item item = new Item { Name = name, SellIn = sellIn, Quality = quality };
        m_items.Add(item);

        return item;
    }

    protected IList<Item> m_items = new List<Item>();
    protected GildedRose m_app;
    private GildedRoseRule ruleCreator = new GildedRoseRule();

    [SetUp]
    public void Setup()
    {
        m_app = new GildedRose(m_items, ruleCreator.rules, ruleCreator.Default);
    }

    [Test]
    public void SellInDegradeAtTheEndOfEachDay()
    {
        // Arrange
        Item item = GetSampleItem();
        var expectedSellIn = item.SellIn - 1;

        // Act
        m_app.UpdateQuality();

        // Assert
        Assert.That(item.SellIn, Is.EqualTo(expectedSellIn));
    }
    
    
    [Test]
    public void QualityDegradeAtTheEndOfEachDay()
    {
        // Arrange
        Item item = GetSampleItem();
        var expectedQuality = item.Quality - 1;
    
        // Act
        m_app.UpdateQuality();
    
        // Assert
        Assert.That(item.Quality, Is.EqualTo(expectedQuality));
    }
    
    [Test]
    public void QualityDegradeTwiceAsFastAfterSellInDate()
    {
        // Arrange
        Item item = GetSampleItem(sellIn: 0);
        var expectedQuality = item.Quality - 2;
    
        // Act
        m_app.UpdateQuality();
    
        // Assert
        Assert.That(item.Quality, Is.EqualTo(expectedQuality));
    }
    
    [Test]
    public void QualityIsNeverNegative()
    {
        // Arrange
        var quality = 1;
        var expectedQuality = Math.Max(quality - 2, 0);
        Item item = GetSampleItem(sellIn: 0, quality: quality);
    
        // Act
        m_app.UpdateQuality();
    
        // Assert
        Assert.That(item.Quality, Is.EqualTo(expectedQuality));
    }
    
    [Test]
    public void AgedBrieQualityIncreasesWithTime()
    {
        // Arrange
        Item item = GetSampleItem(name: "Aged Brie");
        var expectedQuality = item.Quality + 1;
    
        // Act
        m_app.UpdateQuality();
    
        // Assert
        Assert.That(item.Quality, Is.EqualTo(expectedQuality));
    }
    
    [Test]
    public void TheQualityNeverRaisesOver50()
    {
        // Arrange
        var quality = 50;
        var expectedQuality = quality;
        Item item = GetSampleItem(name: "Aged Brie", quality: quality);
    
        // Act
        m_app.UpdateQuality();
    
        // Assert
        Assert.That(item.Quality, Is.EqualTo(expectedQuality));
    }
    
    [Test]
    public void SulfurasDontHaveToBeSold()
    {
        // Arrange
        Item item = GetSampleItem(name: "Sulfuras, Hand of Ragnaros");
        var expectedSellIn = item.SellIn;
    
        // Act
        m_app.UpdateQuality();
    
        // Assert
        Assert.That(item.SellIn, Is.EqualTo(expectedSellIn));
    }
    
    [Test]
    public void SulfurasNeverChangeQuality()
    {
        // Arrange
        var quality = 80;
        var expectedQuality = quality;
        Item item = GetSampleItem(name: "Sulfuras, Hand of Ragnaros", quality: quality);
    
        // Act
        m_app.UpdateQuality();
    
        // Assert
        Assert.That(item.Quality, Is.EqualTo(expectedQuality));
    }
    
    [Test]
    public void BackstagePassesIncreaseInQualityWithTime()
    {
        // Arrange
        var moreThan10Days = 20;
        Item item = GetSampleItem(name: "Backstage passes to a TAFKAL80ETC concert", sellIn: moreThan10Days);
        var expectedQuality = item.Quality + 1;
    
        // Act
        m_app.UpdateQuality();
    
        // Assert
        Assert.That(item.Quality, Is.EqualTo(expectedQuality));
    }
    
    [Test]
    public void BackstagePassesIncreaseInQualityBy2WhenWithin10DaysOfConcert()
    {
        // Arrange
        var within10Days = 10;
        Item item = GetSampleItem(name: "Backstage passes to a TAFKAL80ETC concert", sellIn: within10Days);
        var expectedQuality = item.Quality + 2;
    
        // Act
        m_app.UpdateQuality();
    
        // Assert
        Assert.That(item.Quality, Is.EqualTo(expectedQuality));
    }
    
    [Test]
    public void BackstagePassesIncreaseInQualityBy3WhenWithin5DaysOfConcert()
    {
        // Arrange
        var within5Days = 5;
        Item item = GetSampleItem(name: "Backstage passes to a TAFKAL80ETC concert", sellIn: within5Days);
        var expectedQuality = item.Quality + 3;
    
        // Act
        m_app.UpdateQuality();
    
        // Assert
        Assert.That(item.Quality, Is.EqualTo(expectedQuality));
    }
    
    [Test, Description("Does it display on success")]
    public void BackstagePassesLooseAllQualityAfterConcert()
    {
        // Arrange
        var expectedQuality = 0;
        var sellInAfterConcert = 0;
        Item item = GetSampleItem(name: "Backstage passes to a TAFKAL80ETC concert", sellIn: sellInAfterConcert);
    
        // Act
        m_app.UpdateQuality();
    
        // Assert
        Assert.That(item.Quality, Is.EqualTo(expectedQuality));
    }

    [Test, Description("Does it display on failure")]
    public void ConjuredItemQualityDegradeTwiceAsFast()
    {
        // Arrange
        Item item = GetSampleItem(name: "Conjured Mana Cake");
        var expectedQuality = item.Quality - 2;
    
        // Act
        m_app.UpdateQuality();
    
        // Assert
        Assert.That(item.Quality, Is.EqualTo(expectedQuality));
    }

    [Test, Description("Does it display on failure")]
    public void ConjuredItemQualityDegradeTwiceAsFastAfterSellIn()
    {
        // Arrange
        Item item = GetSampleItem(name: "Conjured Mana Cake", sellIn: 0);
        var expectedQuality = item.Quality - (2 * 2);

        // Act
        m_app.UpdateQuality();

        // Assert
        Assert.That(item.Quality, Is.EqualTo(expectedQuality));
    }
}