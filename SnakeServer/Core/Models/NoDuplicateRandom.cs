using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SnakeServer.Core.Models
{
    public class NoDuplicateRandom
    {
        private List<int> list;
        private Random random;

        public NoDuplicateRandom(int maxValue)
        {
            this.random = new Random();
            this.list = Enumerable.Range(0, maxValue + 1).ToList();
        }

        public bool TryNext(out int result)
        {
            if (!list.Any())
            {
                result = default;
                return false;
            }

            int index = random.Next(0, list.Count);
            result = list.ElementAt(index);
            list.RemoveAt(index);

            return true;
        }
    }
}
