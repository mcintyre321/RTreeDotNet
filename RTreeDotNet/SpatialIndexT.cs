using System.Collections.Generic;
using System.Linq;
using RTreeDotNet.rtree;

namespace RTreeDotNet
{
    public class SpatialIndex<T> : ISpatialIndex<T>
    {
        private int idCounter = 1;
        Dictionary<T, int> nodeToId = new Dictionary<T, int>();
        Dictionary<int, T> idToNode = new Dictionary<int, T>();

        readonly ISpatialIndex inner = new RTree();
        public void add(Rectangle r, T item)
        {
            var id = idCounter++;
            nodeToId[item] = id;
            idToNode[id] = item;
            inner.add(r, id);
        }

        public bool Delete(Rectangle r, T item)
        {
            var id = nodeToId[item];
            nodeToId.Remove(item);
            idToNode.Remove(id);
            return inner.Delete(r, id);
        }

        public IEnumerable<T> Nearest(Point p, float furthestDistance)
        {
            List<T> items = new List<T>();

            inner.Nearest(p, id =>
            {
                var t = idToNode[id];
                items.Add(t);
                return true;
            }, furthestDistance);
            return items;
        }

        public IEnumerable<T> NearestN(Point p,  int n, float distance)
        {
            List<T> items = new List<T>();
            inner.NearestN(p, id =>
            {
                var t = idToNode[id];
                items.Add(t);
                return true;
            }, n, distance);
            return items.AsEnumerable();
        }

        public IEnumerable<T> NearestNUnsorted(Point p, int n, float distance)
        {
            List<T> items = new List<T>();
            inner.NearestNUnsorted(p, id =>
            {
                var t = idToNode[id];
                items.Add(t);
                return true;
            }, n, distance);
            return items.AsEnumerable();
        }

        public IEnumerable<T> Intersects(Rectangle r)
        {
            List<T> items = new List<T>();
            inner.Intersects(r, id =>
            {
                var t = idToNode[id];
                items.Add(t);
                return true;
            });
            return items.AsEnumerable();
        }

        public IEnumerable<T> Contains(Rectangle r)
        {
            List<T> items = new List<T>();
            inner.Contains(r, id =>
            {
                var t = idToNode[id];
                items.Add(t);
                return true;
            });
            return items.AsEnumerable();
        }

        public int Size => inner.Size;

        public Rectangle GetBounds()
        {
            return inner.GetBounds();
        }

        public void init()
        {

            inner.init();
        }
    }
}