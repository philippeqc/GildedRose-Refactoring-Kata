using GildedRoseKata.Rule;

namespace GildedRoseKata.Domain.Rule;

public class AgedBrieRule
{
    public ItemRule rule;
    public AgedBrieRule()
    {
        rule = new ChainedItemRule(
            new SellInDecreaseEverydayRule(),
            new QualityChangeRule(1),
            new QualityValueRangeRule(0, 50)
        );
    }
}