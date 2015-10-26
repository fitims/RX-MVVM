using System;
using System.Linq.Expressions;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Windows.Input;

namespace RX_MVVM
{
    public interface IPropertyProvider<T> : IDisposable
    {
        IPropertySubject<K> CreateProperty<K>(Expression<Func<T, K>> expression);
        IPropertySubject<K> CreateProperty<K>(Expression<Func<T, K>> expression, K value);
        IPropertySubject<K> CreateProperty<K>(Expression<Func<T, K>> expression, IObservable<K> values);
        ICommandObserver<K> CreateCommand<K>(Expression<Func<T, ICommand>> expression);
        ICommandObserver<K> CreateCommand<K>(Expression<Func<T, ICommand>> expression, bool isEnabled);
        ICommandObserver<K> CreateCommand<K>(Expression<Func<T, ICommand>> expression, IObservable<bool> isEnabled);
    }


    public class PropertyProvider<T> : IPropertyProvider<T>
    {
        private readonly ViewModelBase _viewModel;
        private readonly ISchedulers _schedulers;
        private readonly CompositeDisposable _disposables = new CompositeDisposable();

        public PropertyProvider(ViewModelBase viewModel, ISchedulers schedulers)
        {
            _viewModel = viewModel;
            _schedulers = schedulers;

            _viewModel.ShouldDispose(_disposables);
        }

        private PropertySubject<K> GetProperty<K>(Expression<Func<T, K>> expr)
        {
            var propertyName = ((MemberExpression) expr.Body).Member.Name;
            var propSubject = new PropertySubject<K>();

            _disposables.Add(propSubject.ObserveOn(_schedulers.Dispatcher)
                                        .Subscribe(v => _viewModel.OnPropertyChanged(propertyName)));

            return propSubject;
        }

        public void Dispose()
        {
            if(!_disposables.IsDisposed)
                _disposables.Dispose();
        }


        public IPropertySubject<K> CreateProperty<K>(Expression<Func<T, K>> expression)
        {
            return GetProperty(expression);
        }

        public IPropertySubject<K> CreateProperty<K>(Expression<Func<T, K>> expression, K value)
        {
            var propSubject = GetProperty(expression);
            propSubject.Value = value;
            return propSubject;
        }

        public IPropertySubject<K> CreateProperty<K>(Expression<Func<T, K>> expression, IObservable<K> values)
        {
            var propSubject = GetProperty(expression);
            _disposables.Add(values.Subscribe(v => propSubject.Value = v));
            return propSubject;
        }

        public ICommandObserver<K> CreateCommand<K>(Expression<Func<T, ICommand>> expression)
        {
            return new CommandObserver<K>(true);
        }

        public ICommandObserver<K> CreateCommand<K>(Expression<Func<T, ICommand>> expression, bool isEnabled)
        {
            return new CommandObserver<K>(isEnabled);
        }

        public ICommandObserver<K> CreateCommand<K>(Expression<Func<T, ICommand>> expression, IObservable<bool> isEnabled)
        {
            var cmd = new CommandObserver<K>(true);
            _disposables.Add(isEnabled.Subscribe(cmd.SetCanExecute));
            return cmd;
        }
    }
}
