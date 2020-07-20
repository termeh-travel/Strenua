using System;

namespace Termeh.Toolkit.Reactive
{
    public interface IEventAggregator : IDisposable
    {
        IDisposable Subscribe<T>(Action<T> action);
        void Publish<T>(T @event);
    }
}