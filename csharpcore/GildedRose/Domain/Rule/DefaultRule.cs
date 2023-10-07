using GildedRoseKata.Rule;

namespace GildedRoseKata.Domain.Rule;

public class DefaultRule
{
    public ItemRule rule;

    public DefaultRule()
    {
        rule = new ChainedItemRule(
            new SellInDecreaseEverydayRule(),
            new QualityChangeRule(-1),
            new WhenSellInLessThanRule(new QualityChangeRule(-1), lessThanSellIn: 0 + 1),
            new QualityValueRangeRule(0, 50)
        );
    }
}