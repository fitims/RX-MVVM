using System;
using System.Reactive.Subjects;

namespace RX_MVVM
{
    public interface IPropertySubject<T> : ISubject<T>
    {
        T Value { get; set; }
    }

    public class PropertySubject<T>  :IPropertySubject<T>
    {
        private readonly Subject<T> _subject = new Subject<T>();
        private T _value;

        public void OnNext(T value)
        {
            SetValue(value);
        }

        private void SetValue(T value)
        {
            _value = value;
            _subject.OnNext(value);            
        }

        public void OnError(Exception error)
        {
            _subject.OnError(error);
        }

        public void OnCompleted()
        {
            _subject.OnCompleted();
        }

        public IDisposable Subscribe(IObserver<T> observer)
        {
            return _subject.Subscribe(observer);
        }

        public T Value
        {
            get { return _value; }
            set { SetValue(value); }
        }
    }
}
