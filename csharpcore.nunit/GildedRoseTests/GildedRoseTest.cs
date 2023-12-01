﻿using NUnit.Framework;
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
        (GildedRose app, Item item) = Helper.GetAppWithSampleItem(sellIn: sellIn);

        // Act
        app.UpdateQuality();

        // Assert
        Assert.That(item.SellIn, Is.EqualTo(expectedSellIn));
    }

    [Test]
    public void QualityDecreaseByOneEachDay()
    {
        // Arrange
        var quality = 10;
        var expectedQuality = quality - 1;
        (GildedRose app, Item item) = Helper.GetAppWithSampleItem(quality: quality);

        // Act
        app.UpdateQuality();

        // Assert
        Assert.That(item.Quality, Is.EqualTo(expectedQuality));
    }


    [Test]
    public void OnceSellInDatePassedQualityDecreaseTwiceAsFast()
    {
        // Arrange
        var quality = 10;
        var expectedQuality = quality - 2;
        (GildedRose app, Item item) = Helper.GetAppWithSampleItem(sellIn: 0, quality: quality);

        // Act
        app.UpdateQuality();

        // Assert
        Assert.That(item.Quality, Is.EqualTo(expectedQuality));
    }

    [Test]
    public void QualityOfItemIsNeverNegative()
    {
        // Arrange
        var quality = 0;
        var expectedQuality = 0;
        (GildedRose app, Item item) = Helper.GetAppWithSampleItem(quality: quality);

        // Act
        app.UpdateQuality();

        // Assert
        Assert.That(item.Quality, Is.EqualTo(expectedQuality));
    }


    [Test]
    public void AgedBrieRaiseQualityWithTime()
    {
        // Arrange
        var quality = 10;
        var expectedQuality = quality + 1;
        (GildedRose app, Item item) = Helper.GetAppWithSampleItem(name: "Aged Brie", quality: quality);

        // Act
        app.UpdateQuality();

        // Assert
        Assert.That(item.Quality, Is.EqualTo(expectedQuality));
    }

    [Test]
    public void QualityIsNeverIncreasedAboveFifty() {
        // Arrange
        var quality = 50;
        var expectedQuality = quality;
        (GildedRose app, Item item) = Helper.GetAppWithSampleItem(name: "Aged Brie", quality: quality);

        // Act
        app.UpdateQuality();

        // Assert
        Assert.That(item.Quality, Is.EqualTo(expectedQuality));
    }

    [TestCase("Some item", 75, 74)]
    [TestCase("Aged Brie", 75, 75)]
    public void QualityIsNeverMoreThan50_AsATestCase(string name, int startQuality, int endQuality)
    {
        // Arrange
        var expectedQuality = endQuality;
        (GildedRose app, Item item) = Helper.GetAppWithSampleItem(name: name, quality: startQuality);

        // Act
        app.UpdateQuality();

        // Assert
        Assert.That(item.Quality, Is.EqualTo(expectedQuality));
    }

    [TestCase("Some item", 75, ExpectedResult = 74)]
    [TestCase("Aged Brie", 75, ExpectedResult = 75)]
    public int QualityIsNeverMoreThan50_AsATestCaseWithExpectedResult(string name, int startQuality)
    {
        // Arrange
        (GildedRose app, Item item) = Helper.GetAppWithSampleItem(name: name, quality: startQuality);

        // Act
        app.UpdateQuality();

        // Assert
        return item.Quality;
    }
}