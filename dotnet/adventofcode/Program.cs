﻿using adventofcode.builder;

var adventofcode = new AdventOfCodeBuilder();

adventofcode
    .Select(
        selector => selector.Day21,
        selector => selector.Day22
        )
    .Run();
