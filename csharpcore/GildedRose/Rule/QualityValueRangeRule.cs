namespace GildedRoseKata.Rule;

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
