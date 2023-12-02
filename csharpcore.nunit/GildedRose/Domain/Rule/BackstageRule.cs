
using GildedRoseKata.Rule;

namespace GildedRoseKata.Domain.Rule;

public class BackstagePassesRule
{
    public ItemRule rule { get; }

    public BackstagePassesRule()
    {
        rule = new ChainedItemRule(
            new SellInDecreaseEverydayRule(),
            new BackstagePassesQualityRule().rule,
            new QualityValueRangeRule(0, 50)
        );
    }
}