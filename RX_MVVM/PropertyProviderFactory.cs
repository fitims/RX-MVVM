namespace RX_MVVM
{
    public interface IPropertyProviderFactory
    {
        IPropertyProvider<T> Create<T>(ViewModelBase viewModelBase);
    }

    public class PropertyProviderFactory : IPropertyProviderFactory
    {
        private readonly ISchedulers _schedulers;

        public PropertyProviderFactory(ISchedulers schedulers)
        {
            _schedulers = schedulers;
        }

        public IPropertyProvider<T> Create<T>(ViewModelBase viewModelBase)
        {
            return new PropertyProvider<T>(viewModelBase, _schedulers);
        }
    }
}
