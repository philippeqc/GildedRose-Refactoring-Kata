namespace GildedRoseTests;

public class Tests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void TestWithRandomValues(
        [Values(1, 2, 3)] int x,
        [Random(-1.0, 1.0, 2)] double d)
    {
        Assert.That(x, Is.GreaterThan(0));
    }
    
    [TestCase(12, 3, 4)]
    [TestCase(12, 2, 6)]
    [TestCase(12, 4, 3)]
    public void DivideTest(int n, int d, int q)
    {
        Assert.That(n / d, Is.EqualTo(q));
    }
}