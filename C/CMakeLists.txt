cmake_minimum_required (VERSION 3.8)

project ("GildedRoseUnitTests")

add_library(GildedRose GildedRose.c)

add_subdirectory(lib)
add_subdirectory(tests)
