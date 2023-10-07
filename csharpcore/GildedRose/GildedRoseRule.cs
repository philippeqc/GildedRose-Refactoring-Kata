using System.Collections.Generic;
using GildedRoseKata.Domain.Rule;
using GildedRoseKata.Rule;

namespace GildedRoseKata;

public class GildedRoseRule
{
    public IDictionary<string, ItemRule> rules { get; }
    private ItemRule defaultRule;
    public ItemRule DefaultRule { get { return defaultRule; } }

    public GildedRoseRule()
    {
        rules = new Dictionary<string, ItemRule>
        {
            { "Backstage passes to a TAFKAL80ETC concert", new BackstagePassesRule().rule },
            { "Sulfuras, Hand of Ragnaros", new QualityConstantRule(80) },
            { "Aged Brie", new AgedBrieRule().rule }
        };
        defaultRule = new DefaultRule().rule;
    }
}