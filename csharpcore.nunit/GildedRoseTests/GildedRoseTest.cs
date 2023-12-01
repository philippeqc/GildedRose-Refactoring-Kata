using NUnit.Framework;
using System.Collections.Generic;
using GildedRoseKata;

namespace GildedRoseTests;

public class GildedRoseTest
{
    protected Item GetSampleItem(string name = "foo", int sellIn = 10, int quality = 10)
    {
        Item item = new Item { Name = name, SellIn = sellIn, Quality = quality };
        m_items.Add(item);

        return item;
    }

    protected IList<Item> m_items = new List<Item>();
    protected GildedRose m_app;

    [SetUp]
    public void Setup()
    {
        m_app = new GildedRose(m_items);
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

    [Test]
    public void SulfurasNeverHasToBeSold()
    {
        // Arrange
        Item item = GetSampleItem(name: "Sulfuras, Hand of Ragnaros");
        int expectedSellIn = item.SellIn;
        // Act
        m_app.UpdateQuality();

        // Assert
        Assert.That(item.SellIn, Is.EqualTo(expectedSellIn));
    }

    [Test]
    public void SulfurasNeverDecreaseInQuality()
    {
        // Arrange
        Item item = GetSampleItem(name: "Sulfuras, Hand of Ragnaros");
        int expectedQuality = item.Quality;
        // Act
        m_app.UpdateQuality();

        // Assert
        Assert.That(item.Quality, Is.EqualTo(expectedQuality));
    }

    [TestCase(15, 1)]
    [TestCase(10, 2)]
    [TestCase(5, 3)]
    public void BackstagePassIncreaseInQualityFasterAsConcertApproach(int sellIn, int qualityIncrease)
    {
        // Arrange
        Item item = GetSampleItem(name: "Backstage passes to a TAFKAL80ETC concert", sellIn: sellIn);
        int expectedQuality = item.Quality + qualityIncrease;
        // Act
        m_app.UpdateQuality();

        // Assert
        Assert.That(item.Quality, Is.EqualTo(expectedQuality));
    }

    [Test]
    public void BackstagePassQualityDropTo0AfterTheConcert()
    {
        // Arrange
        Item item = GetSampleItem(name: "Backstage passes to a TAFKAL80ETC concert", sellIn: 0);
        int expectedQuality = 0;
        // Act
        m_app.UpdateQuality();

        // Assert
        Assert.That(item.Quality, Is.EqualTo(expectedQuality));
    }

    [Test]
    [Ignore("Ignore as long as not ready to implement")]
    public void ConjuredItemsQualityDegradeTwiceAsFast()
    {
        // Arrange
        Item item = GetSampleItem(name: "Conjured");
        int expectedQuality = item.Quality - 2;
        // Act
        m_app.UpdateQuality();

        // Assert
        Assert.That(item.Quality, Is.EqualTo(expectedQuality));
    }

    [Test]
    [Ignore("Ignore as long as not ready to implement")]
    public void ConjuredItemsQualityDegradeTwiceAsFastAfterSellIn()
    {
        // Arrange
        Item item = GetSampleItem(name: "Conjured", sellIn: 0);
        int expectedQuality = item.Quality - (2 * 2);
        // Act
        m_app.UpdateQuality();

        // Assert
        Assert.That(item.Quality, Is.EqualTo(expectedQuality));
    }
}