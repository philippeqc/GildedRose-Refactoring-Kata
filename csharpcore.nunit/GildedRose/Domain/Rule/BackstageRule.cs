
using GildedRoseKata.Rule;

namespace GildedRoseKata.Domain.Rule;

public class BackstagePassesRule
{
    public static ItemRule Rule()
    {
        return new ChainedItemRule(
            new SellInDecreaseEverydayRule(),
            new ChainedItemRule(
                new QualityChangeRule(1),
                new WhenSellInLowerOrEqualThanRule(new QualityChangeRule(1), sellInLowerOrEqual: 10),
                new WhenSellInLowerOrEqualThanRule(new QualityChangeRule(1), sellInLowerOrEqual: 5),
                new QualityConstantRule(0, fromSellIn: 0)
            ),
            new QualityValueRangeRule(0, 50)
        );
    }
}