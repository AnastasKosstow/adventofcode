﻿namespace adventofcode;

public class WarehouseWoes : ISolution
{
    public int Day => 15;

    public string Puzzle => "Warehouse Woes";

    private (char[,] Warehouse, char[] Instructions) Input;
    private readonly Dictionary<char, int[]> Directions = new()
    {
        { '^', new[] { -1, 0 } },
        { '>', new[] { 0, 1 } },
        { 'v', new[] { 1, 0 } },
        { '<', new[] { 0, -1 } },
    };

    public (string partOne, string partTwo) Execute()
    {
        var partOne = SolutionPartOne();
        var partTwo = SolutionPartTwo();
        return (partOne.ToString(), partTwo.ToString());
    }

    public void SetInput(string inputSource)
    {
        Input = ParseInput(inputSource);
    }

    private (char[,] Warehouse, char[] Instructions) ParseInput(string inputSource)
    {
        var lines = File.ReadAllLines(inputSource);

        int separatorIndex = Array.FindIndex(lines, string.IsNullOrEmpty);
        int rows = separatorIndex;
        int cols = lines[0].Length;

        char[,] matrix = new char[rows, cols];
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < cols; col++)
            {
                matrix[row, col] = lines[row][col];
            }
        }

        List<char> instructions = new();
        for (int idx = separatorIndex + 1; idx < lines.Length; idx++)
        {
            if (!string.IsNullOrEmpty(lines[idx]))
            {
                instructions.AddRange(lines[idx].ToCharArray());
            }
        }
        return (matrix, instructions.ToArray());
    }

    private int[] FindStartingPosition(char[,] warehouse)
    {
        for (int row = 0; row < warehouse.GetLength(0); row++)
        {
            for (int col = 0; col < warehouse.GetLength(1); col++)
            {
                if (warehouse[row, col] == '@')
                    return [row, col];
            }
        }

        return [0, 0];
    }

    private int SumBoxesCoordinates(char[,] warehouse)
    {
        int result = 0;
        for (int row = 1; row < warehouse.GetLength(0) - 1; row++)
        {
            for (int col = 1; col < warehouse.GetLength(1) - 1; col++)
            {
                if (warehouse[row, col] == 'O')
                    result += 100 * row + col;
            }
        }

        return result;
    }

    private int SolutionPartOne()
    {
        var warehouse = (char[,])Input.Warehouse.Clone();
        var instructions = Input.Instructions;

        var currentPosition = FindStartingPosition(warehouse);
        for (int idx = 0; idx < instructions.Length; idx++)
        {
            var direction = Directions[instructions[idx]];

            var nextPosition = new[] { currentPosition[0] + direction[0], currentPosition[1] + direction[1] };
            if (warehouse[nextPosition[0], nextPosition[1]] == '#')
            {
                continue;
            }

            if (warehouse[nextPosition[0], nextPosition[1]] == 'O')
            {
                var boxes = 1;
                var index = 1;
                while (true)
                {
                    if (warehouse[nextPosition[0] + (direction[0] * index), nextPosition[1] + (direction[1] * index)] != 'O')
                    {
                        break;
                    }
                    boxes++;
                    index++;
                }

                var boxNextPosition = new[] { nextPosition[0] + (direction[0] * boxes), nextPosition[1] + (direction[1] * boxes) };
                if (warehouse[boxNextPosition[0], boxNextPosition[1]] != '#')
                {
                    warehouse[boxNextPosition[0], boxNextPosition[1]] = 'O';
                }
                else
                {
                    continue;
                }
            }

            warehouse[nextPosition[0], nextPosition[1]] = '@';
            warehouse[currentPosition[0], currentPosition[1]] = '.';
            currentPosition = nextPosition;
        }

        var result = SumBoxesCoordinates(warehouse);
        return result;
    }

    private int SolutionPartTwo()
    {
        var warehouse = (char[,])Input.Warehouse.Clone();
        var instructions = Input.Instructions;

        for (int i = 0; i < warehouse.GetLength(0); i++)
        {
            for (int j = 0; j < warehouse.GetLength(1); j++)
            {
                Console.Write(warehouse[i, j]);
            }
            Console.WriteLine();
        }

        var scaledUpWarehouse = ScaleUpWarehouse(warehouse);

        var currentPosition = FindStartingPosition(warehouse);
        for (int idx = 0; idx < instructions.Length; idx++)
        {
            var direction = Directions[instructions[idx]];

            var nextPosition = new[] { currentPosition[0] + direction[0], currentPosition[1] + direction[1] };
            if (scaledUpWarehouse[nextPosition[0], nextPosition[1]] == '#')
            {
                continue;
            }

            if (scaledUpWarehouse[nextPosition[0], nextPosition[1]] is '[' or ']')
            {
                if (direction[0] == 0)
                {
                    if (direction[1] == '[')
                    {
                        
                    }
                    else if (direction[1] == ']')
                    {

                    }
                }
            }

            scaledUpWarehouse[nextPosition[0], nextPosition[1]] = '@';
            scaledUpWarehouse[currentPosition[0], currentPosition[1]] = '.';
            currentPosition = nextPosition;
        }

        var result = SumBoxesCoordinates(warehouse);
        return result;
    }

    private char[,] ScaleUpWarehouse(char[,] warehouse)
    {
        var scaledUpWarehouse = new char[warehouse.GetLength(0), warehouse.GetLength(1) * 2];
        for (int row = 0; row < warehouse.GetLength(0); row++)
        {
            for (int col = 0; col < warehouse.GetLength(1); col++)
            {
                if (warehouse[row, col] == '#')
                {
                    scaledUpWarehouse[row, col * 2] = '#';
                    scaledUpWarehouse[row, col * 2 + 1] = '#';
                }
                else if (warehouse[row, col] == 'O')
                {
                    scaledUpWarehouse[row, col * 2] = '[';
                    scaledUpWarehouse[row, col * 2 + 1] = ']';
                }
                else if (warehouse[row, col] == '@')
                {
                    scaledUpWarehouse[row, col * 2] = '@';
                    scaledUpWarehouse[row, col * 2 + 1] = '.';
                }
                else
                {
                    scaledUpWarehouse[row, col * 2] = '.';
                    scaledUpWarehouse[row, col * 2 + 1] = '.';
                }
            }
        }

        return scaledUpWarehouse;
    }
}