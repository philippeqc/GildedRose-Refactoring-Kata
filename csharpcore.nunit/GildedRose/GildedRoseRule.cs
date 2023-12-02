using System.Collections.Generic;
using GildedRoseKata.Domain.Rule;
using GildedRoseKata.Rule;

namespace GildedRoseKata;

public class GildedRoseRule
{
    public IDictionary<string, ItemRule> rules { get; }
    private ItemRule defaultRule;
    public ItemRule Default { get { return defaultRule; } }

    public GildedRoseRule()
    {
        rules = new Dictionary<string, ItemRule>
        {
            { "Backstage passes to a TAFKAL80ETC concert", BackstagePassesRule.Rule() },
            { "Sulfuras, Hand of Ragnaros", SulfurasRule.Rule() },
            { "Aged Brie", AgedBrieRule.Rule() }
        };
        defaultRule = DefaultRule.Rule();
    }
}