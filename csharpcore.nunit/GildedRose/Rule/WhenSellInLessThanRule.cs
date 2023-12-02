namespace GildedRoseKata.Rule;

public class WhenSellInLessThanRule : ItemRule
{
    private int _lessThanSellIn;
    private ItemRule _itemRule;
    public WhenSellInLessThanRule(ItemRule itemRule, int lessThanSellIn)
    {
        _itemRule = itemRule;
        _lessThanSellIn = lessThanSellIn;
    }

    public void DoRule(Item item)
    {
        if (item.SellIn < _lessThanSellIn)
            _itemRule.DoRule(item);
    }
}