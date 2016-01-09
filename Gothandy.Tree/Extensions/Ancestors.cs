using System.Collections.Generic;

namespace Gothandy.Tree.Extensions
{
    public static class Ancestors
    {
        public static List<T> GetAncestors<T>(this BaseTree<T> self)
        {
            var list = new List<T>();

            AddAncestors(list, self);

            return list;
        }

        private static void AddAncestors<T>(List<T> list, BaseTree<T> current)
        {
            list.Insert(0, current.Item);
            if (current.Parent == null) return;
            AddAncestors(list, current.Parent);
        }
    }
}
