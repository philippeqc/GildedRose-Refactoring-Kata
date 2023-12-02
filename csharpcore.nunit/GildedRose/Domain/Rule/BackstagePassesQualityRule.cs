
using GildedRoseKata.Rule;

namespace GildedRoseKata.Domain.Rule;

public class BackstagePassesQualityRule
{
    public ItemRule rule { get; }

    public BackstagePassesQualityRule()
    {
        rule = new ChainedItemRule(
            new QualityChangeRule(1),
            new WhenSellInLessThanRule(new QualityChangeRule(1), lessThanSellIn: 10 + 1),
            new WhenSellInLessThanRule(new QualityChangeRule(1), lessThanSellIn: 5 + 1),
            new QualityConstantRule(0, fromSellIn: 0)
        );
    }
}