using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace RX_MVVM
{
    public interface IMessageBus
    {
        IDisposable Subscribe<T>(Action<T> action);
        void Publish<T>(T item);
    }

    public class MessageBus : IMessageBus
    {
        private readonly ISubject<object> _messageBus = new Subject<object>();

        public IDisposable Subscribe<T>(Action<T> action)
        {
            return _messageBus.OfType<T>().Subscribe(action);
        }

        public void Publish<T>(T item)
        {
            _messageBus.OnNext(item);
        }
    }
}
