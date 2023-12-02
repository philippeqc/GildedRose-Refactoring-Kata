using GildedRoseKata.Rule;

namespace GildedRoseKata.Domain.Rule;

public class ConjuredRule
{
    public static ItemRule Rule()
    {
        return new ChainedItemRule(
            new SellInDecreaseEverydayRule(),
            new QualityChangeRule(-2),
            new WhenSellInLowerOrEqualThanRule(new QualityChangeRule(-2), sellInLowerOrEqual: -1),
            new QualityValueRangeRule(0, 50)
        );
    }
}