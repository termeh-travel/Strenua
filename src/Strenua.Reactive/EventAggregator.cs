using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace Strenua.Reactive
{
    public class EventAggregator : IEventAggregator
    {
        private readonly Subject<object> _subject = new Subject<object>();

        public IDisposable Subscribe<T>(Action<T> action) where T : notnull
        {
            return _subject.OfType<T>()
                .AsObservable()
                .Subscribe(action);
        }

        public void Publish<T>(T @event) where T: notnull
        {
            _subject.OnNext(@event);
        }

        public void Dispose()
        {
            _subject.Dispose();
        }
    }
}