#include "Common.h"

void ostreamItem(const Item &b, std::ostream &os) {
  os << "name: " << b.name << ", sellIn: " << b.sellIn
     << ", quality: " << b.quality;
};
