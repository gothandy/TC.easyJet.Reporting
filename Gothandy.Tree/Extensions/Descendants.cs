using System.Collections.Generic;

namespace Gothandy.Tree.Extensions
{
    public static class Descendants
    {
        public static List<BaseTree<T>> GetDescendants<T>(this BaseTree<T> self)
        {
            var list = new List<BaseTree<T>>();

            AddDescendants(list, self);

            return list;
        }

        private static void AddDescendants<T>(List<BaseTree<T>> list, BaseTree<T> current)
        {
            if (current == null) return;

            list.Add(current);

            if (current.Count == 0) return;

            foreach (BaseTree<T> child in current)
            {
                AddDescendants(list, child);
            }
        }
    }
}
