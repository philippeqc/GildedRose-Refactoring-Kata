set(TEST_NAME GildedRoseUnitTests.tests)
set(CMAKE_CXX_STANDARD 14)
find_package(Catch2 REQUIRED)
set(SOURCE_FILES
        GildedRoseUnitTests.cc
        main.cpp
)
add_executable(${TEST_NAME} ${SOURCE_FILES})
target_link_libraries(${TEST_NAME} GildedRose ApprovalTestsAmalgamated::ApprovalTestsAmalgamated Catch2::Catch2WithMain)
target_compile_definitions(${TEST_NAME} PRIVATE -DAPPROVAL_TESTS_HIDE_DEPRECATED_CODE=1)
add_test(NAME ${TEST_NAME} COMMAND ${TEST_NAME})
