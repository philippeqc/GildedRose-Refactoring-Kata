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

int is_item_quality_decrease_with_time(Item *pItem) {
  return strcmp(pItem->name, "Aged Brie") &&
         strcmp(pItem->name, "Backstage passes to a TAFKAL80ETC concert");
}

void update_item_quality(Item *pItem) {
  if (is_item_quality_decrease_with_time(pItem)) {
    if (pItem->quality > 0) {
      if (strcmp(pItem->name, "Sulfuras, Hand of Ragnaros")) {
        pItem->quality = pItem->quality - 1;
      }
    }
  } else {
    if (pItem->quality < 50) {
      pItem->quality = pItem->quality + 1;

      if (!strcmp(pItem->name, "Backstage passes to a TAFKAL80ETC concert")) {
        if (pItem->sellIn < 11) {
          if (pItem->quality < 50) {
            pItem->quality = pItem->quality + 1;
          }
        }

        if (pItem->sellIn < 6) {
          if (pItem->quality < 50) {
            pItem->quality = pItem->quality + 1;
          }
        }
      }
    }
  }

  if (strcmp(pItem->name, "Sulfuras, Hand of Ragnaros")) {
    pItem->sellIn = pItem->sellIn - 1;
  }

  if (pItem->sellIn < 0) {
    if (strcmp(pItem->name, "Aged Brie")) {
      if (strcmp(pItem->name, "Backstage passes to a TAFKAL80ETC concert")) {
        if (pItem->quality > 0) {
          if (strcmp(pItem->name, "Sulfuras, Hand of Ragnaros")) {
            pItem->quality = pItem->quality - 1;
          }
        }
      } else {
        pItem->quality = pItem->quality - pItem->quality;
      }
    } else {
      if (pItem->quality < 50) {
        pItem->quality = pItem->quality + 1;
      }
    }
  }
}

void update_quality(Item items[], int size) {
  int i;

  for (i = 0; i < size; i++) {
    update_item_quality(&items[i]);
  }
}
