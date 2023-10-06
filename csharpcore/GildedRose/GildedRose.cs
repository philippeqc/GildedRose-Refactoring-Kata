using System.Collections.Generic;
using System.Linq;

namespace GildedRoseKata;

public interface ItemRule
{
    void DoRule(Item item);
}

public class WhenSellInLessThanRule : ItemRule
{
    private int _lessThanSellIn;
    private ItemRule _itemRule;
    public WhenSellInLessThanRule(ItemRule itemRule, int lessThanSellIn)
    {
        _itemRule = itemRule;
        _lessThanSellIn = lessThanSellIn;
    }

    public void DoRule(Item item)
    {
        if (item.SellIn < _lessThanSellIn)
            _itemRule.DoRule(item);
    }
}

public class QualityChangeRule : ItemRule
{
    private int _qualityRate;
    public QualityChangeRule(int qualityRate) =>
        _qualityRate = qualityRate;

    public void DoRule(Item item)
    {
        item.Quality += _qualityRate;
    }
}

public class QualityValueRangeRule : ItemRule
{
    private int _minimalQuality;
    private int _maximalQuality;
    public QualityValueRangeRule(int minimalQuality = 0, int maximalQuality = 50)
    {
        _minimalQuality = minimalQuality;
        _maximalQuality = maximalQuality;
    }
    public void DoRule(Item item)
    {
        if (item.Quality < _minimalQuality)
            item.Quality = _minimalQuality;
        if (item.Quality > _maximalQuality)
            item.Quality = _maximalQuality;
    }
}

public class QualityConstantRule : ItemRule
{
    private int _qualityConstant;
    private int _fromSellIn;
    public QualityConstantRule(int qualityConstant, int fromSellIn = int.MaxValue)
    {
        _qualityConstant = qualityConstant;
        _fromSellIn = fromSellIn;
    }
    public void DoRule(Item item)
    {
        if (item.SellIn <= _fromSellIn)
            item.Quality = _qualityConstant;
    }
}

public class SellInDecreaseEverydayRule : ItemRule
{
    public void DoRule(Item item)
    {
        item.SellIn = item.SellIn - 1;
    }
}

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

    public void UpdateItem(Item item)
    {
        // ItemRule itemRule;
        // switch (item.Name)
        // {
        //     case "Sulfuras, Hand of Ragnaros":
        //         {
        //             itemRule = new QualityConstantRule(80);
        //             break;
        //         }
        //     case "Aged Brie":
        //         {
        //             IList<ItemRule> itemRules = new List<ItemRule> {
        //                 new SellInDecreaseEverydayRule(),
        //                 new QualityChangeRule(1),
        //                 new QualityValueRangeRule(0, 50)
        //             };
        //             itemRule = new ChainedItemRule(itemRules);
        //             break;
        //         }
        //     case "Backstage passes to a TAFKAL80ETC concert":
        //         {
        //             IList<ItemRule> itemRules = new List<ItemRule> {
        //                 new SellInDecreaseEverydayRule(),
        //                 new QualityChangeRule(1),
        //                 new QualityChangeRule(1, fromSellIn: 10),
        //                 new QualityChangeRule(1, fromSellIn: 5),
        //                 new QualityConstantRule(0, fromSellIn: 0),
        //                 new QualityValueRangeRule(0, 50)
        //             };
        //             itemRule = new ChainedItemRule(itemRules);
        //             break;
        //         }
        //     default:
        //         {
        //             IList<ItemRule> itemsRules = new List<ItemRule>{
        //                 new SellInDecreaseEverydayRule(),
        //                 new QualityChangeRule(-1),
        //                 new QualityChangeRule(-1, fromSellIn: 0),
        //                 new QualityValueRangeRule(0, 50)
        //         };
        //             itemRule = new ChainedItemRule(itemsRules);
        //             break;
        //         }
        // }
        ItemRule itemRule;
        // if (_rules.ContainsKey(item.Name))
        // {
        //     itemRule = _rules[item.Name];
        // }
        // else
        // {
        //     itemRule = _defaultRule;
        // }
        if (!_rules.TryGetValue(item.Name, out itemRule))
        {
            itemRule = _defaultRule;
        }
        itemRule.DoRule(item);
    }

    public void UpdateQuality()
    {
        for (var i = 0; i < _items.Count; i++)
        {
            UpdateItem(_items[i]);

        }
    }
}

public class RuleCreator
{
    public IDictionary<string, ItemRule> rules { get; }
    private ItemRule defaultRule;
    public ItemRule DefaultRule { get { return defaultRule; } }

    public RuleCreator()
    {
        rules = new Dictionary<string, ItemRule>
        {
            { "Backstage passes to a TAFKAL80ETC concert", CreateBackstageRule() },
            { "Sulfuras, Hand of Ragnaros", CreateSulfurasRules() },
            { "Aged Brie", CreateAgedBriRule() }
        };
        defaultRule = CreateDefaultRule();
    }

    private ItemRule CreateBackstagePassesQualityRule()
    {
        ChainedItemRule backstagePassesQualityRule = new ChainedItemRule(
            new QualityChangeRule(1),
            new WhenSellInLessThanRule(new QualityChangeRule(1), lessThanSellIn: 10 + 1),
            new WhenSellInLessThanRule(new QualityChangeRule(1), lessThanSellIn: 5 + 1),
            new QualityConstantRule(0, fromSellIn: 0)
        );
        return backstagePassesQualityRule;
    }

    public ItemRule CreateBackstageRule()
    {
        ItemRule backstagePassesQualityRule = CreateBackstagePassesQualityRule();

        ItemRule backstagePassRule = new ChainedItemRule(
            new SellInDecreaseEverydayRule(),
            backstagePassesQualityRule,
            new QualityValueRangeRule(0, 50)
        );
        return backstagePassRule;
    }

    public ItemRule CreateSulfurasRules()
    {
        ItemRule sulfurasRule = new QualityConstantRule(80);
        return sulfurasRule;
    }

    public ItemRule CreateAgedBriRule()
    {
        ItemRule agedBrieRule = new ChainedItemRule(
            new SellInDecreaseEverydayRule(),
            new QualityChangeRule(1),
            new QualityValueRangeRule(0, 50)
        );
        return agedBrieRule;
    }

    public ItemRule CreateDefaultRule()
    {
        ItemRule defaultRule = new ChainedItemRule(
            new SellInDecreaseEverydayRule(),
            new QualityChangeRule(-1),
            new WhenSellInLessThanRule(new QualityChangeRule(-1), lessThanSellIn: 0 + 1),
            new QualityValueRangeRule(0, 50)
        );
        return defaultRule;
    }
}