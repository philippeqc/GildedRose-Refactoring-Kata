using System.Text;
using ApprovalTests;
using ApprovalTests.Reporters;
using GildedRoseKata;

namespace GildedRoseTests;

[UseReporter(typeof(DiffReporter))]
[TestFixture]
public class ApprovalTest
{
    [Test]
    public void ThirtyDays()
    {
        StringBuilder fakeOutput = new StringBuilder();
        Console.SetOut(new StringWriter(fakeOutput));
        Console.SetIn(new StringReader($"a{Environment.NewLine}"));

        TextTestFixture.Main(new string[] { });
        var output = fakeOutput.ToString();

        Approvals.Verify(output);
    }

    [Test]
    public void SellInDecreaseByOneEachDay()
    {
        // Arrange
        var sellIn = 10;
        (GildedRose app, Item item) = Helper.GetAppWithSampleItem(sellIn: sellIn);

        // Act
        app.UpdateQuality();

        // Assert
        Approvals.Verify(item);
    }

    // [Test]
    // public void QualityDecreaseByOneEachDay()
    // {
    //     // Arrange
    //     var quality = 10;
    //     (GildedRose app, Item item) = Helper.GetAppWithSampleItem(quality: quality);

    //     // Act
    //     app.UpdateQuality();

    //     // Assert
    //     Approvals.Verify(item);
    // }

    // [Test]
    // public void OnceSellInDatePassedQualityDecreaseTwiceAsFast()
    // {
    //     // Arrange
    //     var quality = 10;
    //     (GildedRose app, Item item) = Helper.GetAppWithSampleItem(sellIn: 0, quality: quality);

    //     // Act
    //     app.UpdateQuality();

    //     // Assert
    //     Approvals.Verify(item);
    // }


    // [Test]
    // public void QualityOfItemIsNeverNegative()
    // {
    //     // !!!
    //     Assert.Fail();
    // }


    // [Test]
    // public void AgedBrieRaiseQualityWithTime()
    // {
    //     // !!!
    //     Assert.Fail();
    // }

}