using System;
using System.Text.Json;

namespace SnakeServer.Core.Models
{
    public class Point
    {
        public int X { get; set; }

        public int Y { get; set; }

        public Point()
        {
        }

        public Point(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }

        public static bool operator ==(Point p1, Point p2)
        {
            if (p2 is null && p1 is null)
                return true;

            if ((p1 is null) ^ (p2 is null))
                return false;

            return p1.X == p2.X && p1.Y == p2.Y;
        }

        public static bool operator !=(Point p1, Point p2)
        {
            if (p2 is null || p1 is null)
                return true;

            return p1.X != p2.X || p1.Y != p2.Y;
        }

        public override bool Equals(object obj)
        {
            return obj is Point point &&
                   X == point.X &&
                   Y == point.Y;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(X, Y);
        }

        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}