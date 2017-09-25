using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace CalendarsTester.Core.Helpers
{
    /// 
    /// Helper implementation for grouping list models with headers
    /// Reference from https://gist.github.com/jamesmontemagno/4ee0112aebe22ef85354#file-grouping-cs
    /// (or http://motzcod.es/post/94643411707/enhancing-xamarinforms-listview-with-grouping)
    ///
    /// <typeparam name="K">Refers to the type of key to identify a group</typeparam>
    /// <typeparam name="T">Refers to the type of group items</typeparam>
    public class Grouping<K, T> : ObservableCollection<T>
    {
        public K Key { get; private set; }

        public Grouping(K key, IEnumerable<T> items)
        {
            Key = key;
            foreach (var item in items)
            {
                Items.Add(item);
            }
        }
    }
}
