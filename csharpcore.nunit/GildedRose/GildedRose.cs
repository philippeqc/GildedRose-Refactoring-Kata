using System.Collections.Generic;
using GildedRoseKata.Rule;

namespace GildedRoseKata;

public class GildedRose
{
    private readonly IList<Item> _items;
    private readonly IDictionary<string, ItemRule> _rules;
    private readonly ItemRule _defaultRule;

    public GildedRose(IList<Item> items, IDictionary<string, ItemRule> rules, ItemRule defaultRule)
    {
        _items = items;
        _rules = rules;
        _defaultRule = defaultRule;
    }

    private void UpdateItem(Item item)
    {
        if (!_rules.TryGetValue(item.Name, out ItemRule itemRule))
        {
            itemRule = _defaultRule;
        }
        itemRule.DoRule(item);
    }

    public void UpdateQuality()
    {
        foreach (var item in _items)
        {
            UpdateItem(item);
        }
    }
}
