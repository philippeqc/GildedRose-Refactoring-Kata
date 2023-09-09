if(NOT TARGET ApprovalTestsAmalgamated)
    set(LIB_NAME ApprovalTestsAmalgamated)
    add_library(${LIB_NAME}  catch_amalgamated.cpp )
    target_link_libraries(${LIB_NAME} ApprovalTests::ApprovalTests)
    add_library(${LIB_NAME}::${LIB_NAME} ALIAS ${LIB_NAME})
endif()

