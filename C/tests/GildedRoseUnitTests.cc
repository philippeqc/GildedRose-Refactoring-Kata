#include "ApprovalTests.hpp"
#include "Common.h"
#include <catch2/catch_test_macros.hpp>
#include <catch2/generators/catch_generators.hpp>

using namespace ApprovalTests;

extern "C" {
#include "../GildedRose.h"
}

void update_until_sellIn_for_item(Item items[], int last, Item &item) {
  for (int i = item.sellIn; i > 0; i--) {
    update_quality(items, last);
  }
}

Item *findItem(Item items[], int last, const char *name) {
  std::string desired{name};
  for (int i = 0; i < last; i++) {
    if (strcmp(name, items[i].name) == 0) {
      return &items[i];
    }
    std::string current{items[i].name};
    if (current.find(desired) != std::string::npos) {
      return &items[i];
    }
  }
  assert("Could not find item");
  return NULL;
}

Item getItem(Item items[], int last, const char *name) {
  return *findItem(items, last, name);
}

TEST_CASE("AFirstTest") {
  Item items[1];
  init_item(items, "Foo", 0, 0);
  update_quality(items, 1);

  Approvals::verify(items[0], [](const Item &b, std::ostream &os) {
    os << "name: " << b.name << ", sellIn: " << b.sellIn
       << ", quality: " << b.quality;
  });
}

int populate_items(Item items[]) {
  int last = 0;

  init_item(items + last++, "+5 Dexterity Vest", 10, 20);
  init_item(items + last++, "Aged Brie", 2, 0);
  init_item(items + last++, "Elixir of the Mongoose", 5, 7);
  init_item(items + last++, "Sulfuras, Hand of Ragnaros", 0, 80);
  init_item(items + last++, "Backstage passes to a TAFKAL80ETC concert", 15,
            20);
  init_item(items + last++, "Conjured Mana Cake", 3, 6);

  return last;
}

TEST_CASE("BaseRules", "DomainRules") {
  Item items[6];
  int last = populate_items(items);

  SECTION(
      "At the end of each day our system lowers both values for every item") {
    // arrange
    Item vest = getItem(items, last, "+5 Dexterity Vest");

    // act
    update_quality(items, last);

    // assert
    Item actual = getItem(items, last, "+5 Dexterity Vest");
    Approvals::verify(actual, ostreamItem);
  }

  SECTION("The product looses 1 quality per day until it reaches the sell by "
          "date") {
    // arrange
    Item vest = getItem(items, last, "+5 Dexterity Vest");

    // act
    update_until_sellIn_for_item(items, last, vest);

    // assert
    Item actual = getItem(items, last, "+5 Dexterity Vest");
    // REQUIRE(vest.quality - vest.sellIn == actual.quality);
    Approvals::verify(actual, ostreamItem);

    SECTION(
        "Once the sell by date has passed, Quality degrades twice as fast") {
      // Item vest = getItem(items, last, "+5 Dexterity Vest");

      // act
      update_quality(items, last);

      // assert
      Item actual = getItem(items, last, "+5 Dexterity Vest");
      Approvals::verify(actual, ostreamItem);
      // Or could be
      // REQUIRE(vest.quality - 2 == actual.quality);

      SECTION("The quality can degrade to 0") {
        // arrange
        Item *pVest = findItem(items, last, "+5 Dexterity Vest");

        // act
        while (pVest->quality > 0) {
          update_quality(items, last);
        }

        // assert
        REQUIRE(0 == getItem(items, last, "+5 Dexterity Vest").quality);

        SECTION("The Quality of an item is never negative") {

          // act
          update_quality(items, last);

          // assert
          Item actual = getItem(items, last, "+5 Dexterity Vest");
          REQUIRE(0 == actual.quality);
          // This fails due to a too long file name
          // Approvals::verify(actual, ostreamItem);
        }
      }
    }
  }
}

TEST_CASE("Aged Brie", "DomainRules") {
  Item items[6];
  int last = populate_items(items);

  SECTION("Aged Brie actually increases in Quality the older it gets") {
    // arrange
    Item brie = getItem(items, last, "Aged Brie");

    // act
    update_quality(items, last);

    // assert
    Item actual = items[1];
    REQUIRE(brie.quality + 1 == actual.quality);
    Approvals::verify(actual, ostreamItem);
  }

  SECTION("The Quality of an item is never more than 50") {
    // arrange
    Item *pBrie = findItem(items, last, "Aged Brie");

    // Pre-condition: Quality reaches 50
    while (pBrie->quality < 50) {
      update_quality(items, last);
    }

    REQUIRE(pBrie->quality == 50);

    // act
    update_quality(items, last);

    // assert
    Item actual = getItem(items, last, "Aged Brie");
    REQUIRE(actual.quality == 50);
  }
}

TEST_CASE("Sulfuras", "DomainRules") {
  Item items[6];
  int last = populate_items(items);

  SECTION("Sulfuras, being a legendary item, never has to be sold or decreases "
          "in Quality") {
    // arrange
    Item sulfuras = getItem(items, last, "Sulfuras, Hand of Ragnaros");

    // act
    update_quality(items, last);

    // assert
    Item actual = getItem(items, last, "Sulfuras, Hand of Ragnaros");
    Approvals::verify(actual, ostreamItem);
  }
}

TEST_CASE("Backstage passes, like aged brie, increases in Quality as its "
          "SellIn value approaches",
          "DomainRules") {
  // arrange
  Item items[6];
  int last = populate_items(items);

  Item backstage = getItem(items, last, "Backstage passes");

  // act
  update_quality(items, last);

  // assert
  Item actual = getItem(items, last, "Backstage passes");
  REQUIRE(actual.quality > backstage.quality);
}

TEST_CASE(
    "Backstage passes Quality increases by 2 when there are 10 days or less") {
  // Arrange
  Item items[6];
  int last = populate_items(items);
  findItem(items, last, "Backstage passes")->sellIn = 10;
  Item backstage = getItem(items, last, "Backstage passes");

  // Act
  update_quality(items, last);

  // Assert
  Item actual = getItem(items, last, "Backstage passes");
  REQUIRE(actual.quality == backstage.quality + 2);
}

TEST_CASE(
    "Backstage passes Quality increases by 3 when there are 5 days or less") {
  // Arrange
  Item items[6];
  int last = populate_items(items);
  findItem(items, last, "Backstage passes")->sellIn = 5;
  Item backstage = getItem(items, last, "Backstage passes");

  // Act
  update_quality(items, last);

  // Assert
  Item actual = getItem(items, last, "Backstage passes");
  REQUIRE(actual.quality == backstage.quality + 3);
}

TEST_CASE("Backstage passes Quality drops to 0 after the concert") {
  // Arrange
  Item items[6];
  int last = populate_items(items);
  findItem(items, last, "Backstage passes")->sellIn = 0;

  // Act
  update_quality(items, last);

  // Assert
  REQUIRE(getItem(items, last, "Backstage passes").quality == 0);
}

TEST_CASE("Backstage passes", "DomainRules") {
  Item items[6];
  int last = populate_items(items);

  SECTION("Backstage passes, like aged brie, increases in Quality as its "
          "SellIn value approaches") {
    // arrange
    Item backstage = getItem(items, last, "Backstage passes");

    // act
    update_quality(items, last);

    // assert
    Item actual = getItem(items, last, "Backstage passes");
    REQUIRE(actual.quality > backstage.quality);

    SECTION("Quality increases by 2 when there are 10 days or less") {
      // Arrange
      findItem(items, last, "Backstage passes")->sellIn = 10;
      Item backstage = getItem(items, last, "Backstage passes");

      // Act
      update_quality(items, last);

      // Assert
      Item actual = getItem(items, last, "Backstage passes");
      REQUIRE(actual.quality == backstage.quality + 2);

      SECTION("Quality increases by 3 when there are 5 days or less") {
        findItem(items, last, "Backstage passes")->sellIn = 5;
        Item backstage = getItem(items, last, "Backstage passes");

        // Act
        update_quality(items, last);

        // Assert
        Item actual = getItem(items, last, "Backstage passes");
        REQUIRE(actual.quality == backstage.quality + 3);

        SECTION("Quality drops to 0 after the concert") {
          // Arrange
          findItem(items, last, "Backstage passes")->sellIn = 0;

          // Act
          update_quality(items, last);

          // Assert
          REQUIRE(getItem(items, last, "Backstage passes").quality == 0);
        }
      }
    }
  }
}

/*
TEST_CASE("ImproveWithAge", "DomainRules") {
  Item items[6];
  int last = populate_items(items);

  SECTION("Aged Brie actually increases in Quality the older it gets") {
    // arrange
    std::string name = GENERATE("Aged Brie", "Backstage passes");
    Item item = getItem(items, last, name.c_str());

    // act
    update_quality(items, last);

    // assert
    Item actual = items[1];
    REQUIRE(item.quality + 1 == actual.quality);
    Approvals::verify(actual, ostreamItem);
  }
}
*/

TEST_CASE("Conjured") {
  // Arrange
  Item items[1];
  int last = 0;

  init_item(items + last++, "Conjured", 10, 20);
  Item conjured = getItem(items, last, "Conjured");

  // Act
  update_quality(items, last);

  // Assert
  Item actual = getItem(items, last, "Conjured");
  REQUIRE(actual.quality == conjured.quality - 2);
}
