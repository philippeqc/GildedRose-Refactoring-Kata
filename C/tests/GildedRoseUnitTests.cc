#include "ApprovalTests.hpp"
#include <catch2/catch_test_macros.hpp>

using namespace ApprovalTests;

extern "C" {
#include "../GildedRose.h"
}

TEST_CASE("AFirstTest") {
  Item items[1];
  init_item(items, "Foo", 0, 0);
  update_quality(items, 1);

  Approvals::verify(items[0], [](const Item &b, std::ostream &os) {
    os << "name: " << b.name << ", sellIn: " << b.sellIn
       << ", quantity: " << b.quality;
  });
}

void ostreamItem(const Item &b, std::ostream &os) {
  os << "name: " << b.name << ", sellIn: " << b.sellIn
     << ", quantity: " << b.quality;
};

TEST_CASE("TestGildedRoseGroup", "FirstTest") {
  Item items[6];
  int last = 0;

  init_item(items + last++, "+5 Dexterity Vest", 10, 20);
  init_item(items + last++, "Aged Brie", 2, 0);
  init_item(items + last++, "Elixir of the Mongoose", 5, 7);
  init_item(items + last++, "Sulfuras, Hand of Ragnaros", 0, 80);
  init_item(items + last++, "Backstage passes to a TAFKAL80ETC concert", 15,
            20);
  init_item(items + last++, "Conjured Mana Cake", 3, 6);

  SECTION(
      "At the end of each day our system lowers both values for every item") {
    // arrange
    Item vest = items[0];

    // act
    update_quality(items, last);

    // assert
    Item actual = items[0];
    Approvals::verify(actual, ostreamItem);
  }

  SECTION("The product looses 1 quality per day until it reaches the sell by "
          "date") {
    // arrange
    Item vest = items[0];

    // act
    for (int i = vest.sellIn; i > 0; i--) {
      update_quality(items, last);
    }

    // assert
    // REQUIRE(vest.quality - vest.sellIn == items[0].quality);
    Item actual = items[0];
    Approvals::verify(actual, ostreamItem);

    SECTION(
        "Once the sell by date has passed, Quality degrades twice as fast") {
      // Item vest = items[0];

      // act
      update_quality(items, last);

      // assert
      Item actual = items[0];
      Approvals::verify(actual, ostreamItem);
      // Or could be
      // REQUIRE(vest.quality - 2 == actual.quality);

      SECTION("The quality can degrade to 0") {
        // arrange
        Item vest = items[0];

        // act
        int daysLeft = (vest.quality + 1) / 2;
        for (int i = daysLeft; i > 0; i--) {
          update_quality(items, last);
        }

        // assert
        REQUIRE(0 == items[0].quality);

        SECTION("The Quality of an item is never negative") {

          // act
          update_quality(items, last);

          // assert
          Item actual = items[0];
          REQUIRE(0 == actual.quality);
          // This fails due to a too long file name
          // Approvals::verify(actual, ostreamItem);
        }
      }
    }
  }

  SECTION("Aged Brie actually increases in Quality the older it gets") {
    Item brie = items[1];
    REQUIRE(strcmp("Aged Brie", brie.name) == 0);

    
  }
}