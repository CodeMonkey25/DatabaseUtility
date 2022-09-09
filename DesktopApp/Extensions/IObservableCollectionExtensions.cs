using System;
using DynamicData.Binding;

namespace DesktopApp.Extensions
{
    public static class IObservableCollectionExtensions
    {
        public static int RemoveAll<T>(this IObservableCollection<T> collection, Predicate<T> match)
        {
            int removed = 0;

            // using (collection.SuspendNotifications())
            // {
                for (int i = collection.Count - 1; i >= 0; i--)
                {
                    if (!match(collection[i])) continue;

                    collection.RemoveAt(i);
                    removed++;
                }
            // }

            return removed;
        }
    }
}