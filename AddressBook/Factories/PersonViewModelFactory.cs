using AddressBook.Models;
using AddressBook.Services;
using AddressBook.ViewModels;
using RX_MVVM;

namespace AddressBook.Factories
{
    public interface IPersonViewModelFactory
    {
        IPersonViewModel Create(Person person);
    }

    public class PersonViewModelFactory : IPersonViewModelFactory
    {
        private readonly IPropertyProviderFactory _propertyProviderFactory;
        private readonly IMessageBus _messageBus;
        private readonly IDialogService _dialogService;

        public PersonViewModelFactory(IPropertyProviderFactory propertyProviderFactory, IMessageBus messageBus, IDialogService dialogService)
        {
            _propertyProviderFactory = propertyProviderFactory;
            _messageBus = messageBus;
            _dialogService = dialogService;
        }

        public IPersonViewModel Create(Person person)
        {
            return new PersonViewModel(_propertyProviderFactory, _messageBus, _dialogService, person);
        }
    }
}
