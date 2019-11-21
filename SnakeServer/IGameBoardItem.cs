using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SnakeServer
{
    public interface IGameBoardItem
    {
        IEnumerable<Point> Points { get; }
        public void Add(Point point);
        public void Delete(Point point);
        public Point this[int index] { get; set; }
    }
}
