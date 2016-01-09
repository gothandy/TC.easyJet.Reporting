using System.Collections.Generic;

namespace Gothandy.Tree.Extensions
{
    public static class Descendants
    {
        public static List<T> GetDescendants<T>(this BaseTree<T> self)
        {
            var list = new List<T>();

            AddDescendants(list, self);

            return list;
        }

        private static void AddDescendants<T>(List<T> list, BaseTree<T> current)
        {
            list.Add(current.Item);

            if (current.Count == 0) return;

            foreach (BaseTree<T> child in current)
            {
                AddDescendants(list, child);
            }
        }
    }
}
