namespace GildedRoseKata.Rule;

public class SellInDecreaseEverydayRule : ItemRule
{
    public void DoRule(Item item)
    {
        item.SellIn = item.SellIn - 1;
    }
}
