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

void update_item_quality(Item *pItem) {
  switch (getItemType(pItem)) {
  case ITEM_COMMON:
  case ITEM_CONJURED: {
    if (pItem->quality > 0) {
      if (pItem->sellIn == 0) {
        pItem->quality = pItem->quality - 2;
      } else {
        pItem->quality = pItem->quality - 1;
      }
    }
    break;
  }
  case ITEM_AGED_BRIE: {
    if (pItem->sellIn < 0) {
      pItem->quality = pItem->quality + 2;
    } else {
      pItem->quality = pItem->quality + 1;
    }
    break;
  }
  case ITEM_BACKSTAGE_PASSES: {
    if (pItem->quality < 50) {
      if (pItem->sellIn == 0) {
        pItem->quality = 0;
      } else if (pItem->sellIn < 6) {
        pItem->quality = pItem->quality + 3;
      } else if (pItem->sellIn < 11) {
        pItem->quality = pItem->quality + 2;
      } else {
        pItem->quality = pItem->quality + 1;
      }
    }
    break;
  }
  case ITEM_SULFURAS: {
    return;
    break;
  }
  }

  pItem->sellIn = pItem->sellIn - 1;

  if (pItem->quality > 50) {
    pItem->quality = 50;
  }
  if (pItem->quality < 0) {
    pItem->quality = 0;
  }
}

void update_quality(Item items[], int size) {
  int i;

  for (i = 0; i < size; i++) {
    update_item_quality(&items[i]);
  }
}
