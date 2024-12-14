﻿using System.Text.RegularExpressions;

namespace adventofcode;

internal class Robot(int x, int y, int vx, int vy)
{
    internal int X { get; set; } = x;
    internal int Y { get; set; } = y;
    internal int VelocityX { get; set; } = vx;
    internal int VelocityY { get; set; } = vy;

    internal void Move(int width, int height)
    {
        X = (X + VelocityX) % width;
        if (X < 0)
        {
            X += width;
        }

        Y = (Y + VelocityY) % height;
        if (Y < 0)
        {
            Y += height;
        }
    }
}

internal partial class RestroomRedoubt
{
    private static readonly Regex Pattern = RobotPatter();
    private static readonly int width = 101;
    private static readonly int height = 103;

    private static Lazy<Robot[]> Input = new(() =>
    {
        var lines = File.ReadAllLines("day-14/input.txt");
        var input = lines.Select(line => line).ToArray();

        var robots = new Robot[input.Length];
        for (int idx = 0; idx < robots.Length; idx++)
        {
            var match = Pattern.Match(input[idx]);
            robots[idx] =
                new Robot(
                    int.Parse(match.Groups[1].Value),
                    int.Parse(match.Groups[2].Value),
                    int.Parse(match.Groups[3].Value),
                    int.Parse(match.Groups[4].Value)
                );
        }

        return robots;
    });

    private static long CalculateSafetyFactor(Robot[] robots, int seconds)
    {
        for (int idx = 0; idx < seconds; idx++)
        {
            foreach (var robot in robots)
            {
                robot.Move(width, height);
            }
        }

        var positions = new Dictionary<(int x, int y), int>();
        foreach (var robot in robots)
        {
            var position = (robot.X, robot.Y);
            if (!positions.TryGetValue(position, out int value))
            {
                value = 0;
                positions[position] = value;
            }

            positions[position] = ++value;
        }

        var quadrantCounts = new int[5];
        foreach (var robot in robots)
        {
            int quadrant = GetQuadrant(robot.X, robot.Y);
            if (quadrant > 0)
            {
                quadrantCounts[quadrant]++;
            }
        }

        return (long)quadrantCounts[1] * quadrantCounts[2] * quadrantCounts[3] * quadrantCounts[4];
    }

    private static int GetQuadrant(int x, int y)
    {
        if (x == width / 2 || y == height / 2)
        {
            return 0;
        }

        if (x < width / 2)
        {
            if (y < height / 2) return 1;
            return 3;
        }
        else
        {
            if (y < height / 2) return 2;
            return 4;
        }
    }

    internal static void SolutionPartOne()
    {
        var robots = Input.Value;
        var seconds = 10000;
        var result = CalculateSafetyFactor(robots, seconds);
    }

    internal static void SolutionPartTwo()
    {
        // print grid
    }

    [GeneratedRegex(@"p=(-?\d+),(-?\d+)\s+v=(-?\d+),(-?\d+)")]
    private static partial Regex RobotPatter();

}