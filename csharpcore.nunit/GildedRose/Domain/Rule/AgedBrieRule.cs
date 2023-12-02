using GildedRoseKata.Rule;

namespace GildedRoseKata.Domain.Rule;

public class AgedBrieRule
{
    public static ItemRule Rule()
    {
        return new ChainedItemRule(
            new SellInDecreaseEverydayRule(),
            new QualityChangeRule(1),
            new QualityValueRangeRule(0, 50)
        );
    }
}