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
            new WhenSellInLowerOrEqualThanRule(new QualityChangeRule(-1), sellInLowerOrEqual: 0),
            new QualityValueRangeRule(0, 50)
        );
    }
}