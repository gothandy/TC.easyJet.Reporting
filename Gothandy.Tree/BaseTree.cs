using System.Collections.ObjectModel;

namespace Gothandy.Tree
{
    public abstract class BaseTree<T> : Collection<BaseTree<T>>
    {
        public T Item { get; private set; }
        public BaseTree<T> Parent { get; private set; }

        public BaseTree(T item)
        {
            this.Item = item;
        }
        protected override void InsertItem(int index, BaseTree<T> item)
        {
            item.Parent = this;
            base.InsertItem(index, item);
        }

        protected override void SetItem(int index, BaseTree<T> item)
        {
            item.Parent = this;
            base.SetItem(index, item);
        }

        public override string ToString()
        {
            return Item.ToString();
        }
    }
}
