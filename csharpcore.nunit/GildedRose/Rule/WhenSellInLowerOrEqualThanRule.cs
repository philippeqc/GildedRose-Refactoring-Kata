namespace GildedRoseKata.Rule;

public class WhenSellInLowerOrEqualThanRule : ItemRule
{
    private int _sellInLowerOrEqual;
    private ItemRule _itemRule;
    public WhenSellInLowerOrEqualThanRule(ItemRule itemRule, int sellInLowerOrEqual)
    {
        _itemRule = itemRule;
        _sellInLowerOrEqual = sellInLowerOrEqual;
    }

    public void DoRule(Item item)
    {
        if (item.SellIn <= _sellInLowerOrEqual)
            _itemRule.DoRule(item);
    }
}