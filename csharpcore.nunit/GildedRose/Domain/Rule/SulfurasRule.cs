using GildedRoseKata.Rule;

namespace GildedRoseKata.Domain.Rule;

public class SulfurasRule
{
    public static ItemRule Rule()
    {
        return new QualityConstantRule(80);
    }

}