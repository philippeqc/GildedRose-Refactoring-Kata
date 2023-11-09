
using GildedRoseKata;

namespace GildedRoseTests;

public class Helper
{
    public static (GildedRose, Item) GetAppWithSampleItem(string name = "foo", int sellIn = 10, int quality = 10)
    {
        IList<Item> items = new List<Item> { new Item { Name = name, SellIn = sellIn, Quality = quality } };
        GildedRose app = new GildedRose(items);

        return (app, items[0]);
    }
}
