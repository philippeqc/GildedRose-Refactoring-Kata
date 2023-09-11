#include "GildedRose.h"
#include <stdio.h>
#include <string.h>

Item *init_item(Item *item, const char *name, int sellIn, int quality) {
  item->sellIn = sellIn;
  item->quality = quality;
  item->name = strdup(name);

  return item;
}

extern char *print_item(char *buffer, Item *item) {
  sprintf(buffer, "%s, %d, %d", item->name, item->sellIn, item->quality);
}

typedef enum {
  ITEM_COMMON,
  ITEM_AGED_BRIE,

  ITEM_SULFURAS,
  ITEM_BACKSTAGE_PASSES,
  ITEM_CONJURED
} item_type;

typedef Item *(*ItemHandler)(Item *pItem);

item_type getItemType(Item *pItem) {
  if (strcmp(pItem->name, "Aged Brie") == 0) {
    return ITEM_AGED_BRIE;
  }
  if (strcmp(pItem->name, "Backstage passes to a TAFKAL80ETC concert") == 0) {
    return ITEM_BACKSTAGE_PASSES;
  }
  if (strcmp(pItem->name, "Sulfuras, Hand of Ragnaros") == 0) {
    return ITEM_SULFURAS;
  }
  if (strcmp(pItem->name, "Conjured") == 0) {
    return ITEM_CONJURED;
  }

  return ITEM_COMMON;
}

Item *lower_quality(Item *pItem, int rate) {
  if (pItem->sellIn == 0) {
    pItem->quality = pItem->quality - (2 * rate);
  } else {
    pItem->quality = pItem->quality - (1 * rate);
  }
  return pItem;
}

Item *raise_quality(Item *pItem) {
  if (pItem->sellIn < 0) {
    pItem->quality = pItem->quality + 2;
  } else {
    pItem->quality = pItem->quality + 1;
  }
  return pItem;
}

Item *raise_quality_backstage(Item *pItem) {
  if (pItem->sellIn == 0) {
    pItem->quality = 0;
  } else if (pItem->sellIn < 6) {
    pItem->quality = pItem->quality + 3;
  } else if (pItem->sellIn < 11) {
    pItem->quality = pItem->quality + 2;
  } else {
    pItem->quality = pItem->quality + 1;
  }
  return pItem;
}

Item *clamp_quality(Item *pItem) {
  if (pItem->quality > 50) {
    pItem->quality = 50;
  }
  if (pItem->quality < 0) {
    pItem->quality = 0;
  }
  return pItem;
}

Item *stable_quality(Item *pItem) { return pItem; }

Item *lower_sellIn(Item *pItem) {
  pItem->sellIn = pItem->sellIn - 1;
  return pItem;
}

Item *stable_sellIn(Item *pItem) { return pItem; }

void update_item_quality(Item *pItem) {
  ItemHandler pQualityHandler = clamp_quality;
  ItemHandler pSellInHandler = lower_sellIn;

  item_type type = getItemType(pItem);
  if (type == ITEM_COMMON) {
    lower_quality(pItem, 1);
  } else if (type == ITEM_CONJURED) {
    lower_quality(pItem, 2);
  } else if (type == ITEM_AGED_BRIE) {
    raise_quality(pItem);
  } else if (type == ITEM_BACKSTAGE_PASSES) {
    raise_quality_backstage(pItem);
  } else if (type == ITEM_SULFURAS) {
    pQualityHandler = stable_quality;
    pSellInHandler = stable_sellIn;
  }

  pQualityHandler(pItem);
  pSellInHandler(pItem);
}

void update_quality(Item items[], int size) {
  int i;

  for (i = 0; i < size; i++) {
    update_item_quality(&items[i]);
  }
}
