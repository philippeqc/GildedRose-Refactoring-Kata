#include "ApprovalTests.hpp"
#include "Common.h"
#include <catch2/catch_test_macros.hpp>
#include <catch2/generators/catch_generators.hpp>

using namespace ApprovalTests;

extern "C" {
#include "../GildedRose.h"
}

class GildedRoseTestsFixture {
protected:
  Item items[6];
  int last{0};

public:
  GildedRoseTestsFixture() {
    init_item(items + last++, "+5 Dexterity Vest", 10, 20);
    init_item(items + last++, "Aged Brie", 2, 0);
    init_item(items + last++, "Elixir of the Mongoose", 5, 7);
    init_item(items + last++, "Sulfuras, Hand of Ragnaros", 0, 80);
    init_item(items + last++, "Backstage passes to a TAFKAL80ETC concert", 15,
              20);
    init_item(items + last++, "Conjured Mana Cake", 3, 6);
  }

protected:
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
};

TEST_CASE_METHOD(GildedRoseTestsFixture, "A") {
  // arrange
  Item vest = getItem(items, last, "+5 Dexterity Vest");

  // act
  update_quality(items, last);

  // assert
  Item actual = getItem(items, last, "+5 Dexterity Vest");
  Approvals::verify(actual, ostreamItem);
}