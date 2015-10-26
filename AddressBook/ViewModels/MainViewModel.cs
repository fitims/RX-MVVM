using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Windows;
using System.Windows.Input;
using AddressBook.Extensions;
using AddressBook.Factories;
using AddressBook.Messages;
using AddressBook.Models;
using AddressBook.Repositories;
using AddressBook.Services;
using RX_MVVM;

namespace AddressBook.ViewModels
{
    public interface IMainViewModel
    {
        ObservableCollection<IPersonViewModel> Clients { get; }
        ICommand AddNewClientCommand { get; }
        bool IsBusy { get; }
    }

    public class MainViewModel : ViewModelBase, IMainViewModel
    {
        private readonly ICommandObserver<Unit> _addNewClientCommand;
        private readonly IPropertySubject<bool> _isBusy;

        public MainViewModel(IPropertyProviderFactory propertyProviderFactory, IMessageBus messageBus, IDialogService dialogService, IPersonRepository personRepository, IPersonViewModelFactory personViewModelFactory)
        {
            var pp = propertyProviderFactory.Create<IMainViewModel>(this);
            
            //create properies
            _isBusy = pp.CreateProperty(i => i.IsBusy, false);

            // create commands
            _addNewClientCommand = pp.CreateCommand<Unit>(i => i.AddNewClientCommand);

            Clients = new ObservableCollection<IPersonViewModel>();

            IsBusy = true;
            ShouldDispose(personRepository.GetAllClients().ObserveOnDispatcher().Subscribe(c =>
            {
                foreach (var item in c.Select(personViewModelFactory.Create))
                {
                    Clients.Add(item);
                }

                IsBusy = false;
            }));

            ShouldDispose(messageBus.Subscribe<DeletePersonMessage>(m =>
            {
                var msg = string.Format("Are you sure you want to delete {0} {1}?", m.Person.Name, m.Person.Surname);
                if (dialogService.ShowWarning("Delte", msg) == MessageBoxResult.OK)
                {
                    IsBusy = true;
                    ShouldDispose(personRepository.Delete(m.Person.ToModel())
                                                  .ObserveOnDispatcher()
                                                  .Subscribe(_ =>
                                                  {
                                                      Clients.Remove(m.Person);
                                                      IsBusy = false;                                                      
                                                  }));
                }
            }));

            ShouldDispose(_addNewClientCommand.OnExecute.Subscribe(_ =>
            {
                var newClient = personViewModelFactory.Create(new Person());
                if (dialogService.ShowViewModel("New Person", newClient as ViewModelBase))
                {
                    IsBusy = true;
                    ShouldDispose(personRepository.Save(newClient.ToModel())
                                                  .ObserveOnDispatcher()
                                                  .Subscribe(u =>
                                                  {
                                                      Clients.Add(newClient);
                                                      IsBusy = false;
                                                  }));
                }
            }));
        }

        public ObservableCollection<IPersonViewModel> Clients { get; private set; }

        public ICommand AddNewClientCommand
        {
            get { return _addNewClientCommand; }
        }

        public bool IsBusy
        {
            get { return _isBusy.Value; }
            set { _isBusy.Value = value; }
        }
    }
}
