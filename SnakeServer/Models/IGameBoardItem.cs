using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SnakeServer.Models
{
    public interface IGameBoardItem
    {
        IEnumerable<Point> Points { get; }
        void Add(Point point);
        Point this[int index] { get; }
    }
}
