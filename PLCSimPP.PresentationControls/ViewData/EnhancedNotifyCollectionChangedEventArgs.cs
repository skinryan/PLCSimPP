using System.Collections;
using System.Collections.Specialized;

namespace PLCSimPP.PresentationControls.ViewData
{
    /// <summary>
    /// Enhanced notification collection changed event args
    /// </summary>
    public class EnhancedNotifyCollectionChangedEventArgs : IReset
    {
        /// <summary>
        /// Notification collection
        /// </summary>
        public NotifyCollectionChangedEventArgs NotifyCollection { get; set; }
        /// <summary>
        /// Source
        /// </summary>
        public object Source { get; set; }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="source"></param>
        /// <param name="notifyCollection"></param>
        public EnhancedNotifyCollectionChangedEventArgs(object source, NotifyCollectionChangedEventArgs notifyCollection)
        {
            NotifyCollection = notifyCollection;
            Source = source;
        }

        /// <summary>
        /// Undo
        /// </summary>
        public void Undo()
        {
            var il = Source as IList;

            switch (NotifyCollection.Action)
            {
                case (NotifyCollectionChangedAction.Add):
                    {
                        foreach (var item in NotifyCollection.NewItems)
                        {
                            if (il != null) il.Remove(item);
                        }
                        break;
                    }
                case (NotifyCollectionChangedAction.Remove):
                    {
                        foreach (var item in NotifyCollection.OldItems)
                        {
                            il.Add(item);
                        }
                        break;
                    }
                case (NotifyCollectionChangedAction.Move):
                    {
                        foreach (var item in NotifyCollection.OldItems)
                        {
                            il.Remove(item);
                            il.Insert(NotifyCollection.OldStartingIndex, item);
                        }
                        break;
                    }
            }
        }
        /// <summary>
        /// Redo
        /// </summary>
        public void Redo()
        {
            var il = Source as IList;

            switch (NotifyCollection.Action)
            {
                case (NotifyCollectionChangedAction.Add):
                    {
                        foreach (var item in NotifyCollection.NewItems)
                        {
                            il.Add(item);
                        }
                        break;
                    }
                case (NotifyCollectionChangedAction.Remove):
                    {
                        foreach (var item in NotifyCollection.OldItems)
                        {
                            il.Remove(item);
                        }
                        break;
                    }
                case (NotifyCollectionChangedAction.Move):
                    {
                        foreach (var item in NotifyCollection.OldItems)
                        {
                            il.Remove(item);
                            il.Insert(NotifyCollection.NewStartingIndex, item);
                        }
                        break;
                    }
            }
        }
    }
}
