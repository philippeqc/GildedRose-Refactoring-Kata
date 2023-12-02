using System.Collections.Generic;
using System.Linq;

namespace GildedRoseKata.Rule;

public class ChainedItemRule : ItemRule
{
    private IEnumerable<ItemRule> _itemRules;

    public ChainedItemRule(IEnumerable<ItemRule> itemRules) =>
        _itemRules = itemRules.ToList();

    public ChainedItemRule(params ItemRule[] itemRules)
        : this((IEnumerable<ItemRule>)itemRules) { }

    public void DoRule(Item item)
    {
        foreach (var itemRule in _itemRules)
        {
            itemRule.DoRule(item);
        }
    }
}