using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Windows.Input;
using AddressBook.Extensions;
using AddressBook.Messages;
using AddressBook.Models;
using AddressBook.Services;
using RX_MVVM;

namespace AddressBook.ViewModels
{
    public interface IPersonViewModel
    {
        string Name { get; }
        string Surname { get; }
        int Age { get; }
        string Address1 { get; }
        string Address2 { get; }
        string Address3 { get; }
        string City { get; }
        ICommand EditCommand { get; }
        ICommand SaveCommand { get; }
        ICommand DeleteCommand { get; }
        ICommand CloseCommand { get; }
    }

    public class PersonViewModel : ViewModelBase, IPersonViewModel, ICloseable 
    {
        private Person _model;
        private IPropertySubject<string> _name;
        private IPropertySubject<string> _surname;
        private IPropertySubject<int> _age;
        private IPropertySubject<string> _address1;
        private IPropertySubject<string> _address2;
        private IPropertySubject<string> _address3;
        private IPropertySubject<string> _city;
        private ICommandObserver<Unit> _editCommand;
        private ICommandObserver<Unit> _saveCommand;
        private ICommandObserver<Unit> _deleteCommand;
        private ICommandObserver<Unit> _closeCommand;
        private readonly ISubject<bool> _closeSubject = new Subject<bool>(); 

        public PersonViewModel(IPropertyProviderFactory propertyProviderFactory, IMessageBus messageBus, IDialogService dialogService, Person model)
        {
            _model = model;
            var pp = propertyProviderFactory.Create<IPersonViewModel>(this);
            
            // creating properties
            _name = pp.CreateProperty(i => i.Name, model.Name);
            _surname = pp.CreateProperty(i => i.Surname, model.Surname);
            _age = pp.CreateProperty(i => i.Age, model.Age);
            _address1 = pp.CreateProperty(i => i.Address1, model.Address1);
            _address2 = pp.CreateProperty(i => i.Address2, model.Address2);
            _address3 = pp.CreateProperty(i => i.Address3, model.Address3);
            _city = pp.CreateProperty(i => i.City, model.City);

            // creating commands
            _editCommand = pp.CreateCommand<Unit>(i => i.EditCommand);
            _saveCommand = pp.CreateCommand<Unit>(i => i.SaveCommand, !string.IsNullOrEmpty(model.Name) && !string.IsNullOrEmpty(model.Surname));
            _deleteCommand = pp.CreateCommand<Unit>(i => i.DeleteCommand);
            _closeCommand = pp.CreateCommand<Unit>(i => i.CloseCommand);

            // Observing commands
            ShouldDispose(_name.CombineLatest(_surname, (n,s) => !string.IsNullOrEmpty(n) && !string.IsNullOrEmpty(s)).Subscribe(_saveCommand.SetCanExecute));
            ShouldDispose(_saveCommand.OnExecute.Subscribe(_ =>
            {
                UpdateModel();
                _closeSubject.OnNext(true);
            }));
            ShouldDispose(_editCommand.OnExecute.Subscribe(_ => dialogService.ShowViewModel("Edit Person", this)));
            ShouldDispose(_deleteCommand.OnExecute.Subscribe(_ => messageBus.Publish(new DeletePersonMessage(this))));
            ShouldDispose(_closeCommand.OnExecute.Subscribe(_ =>
            {
                ResetData();
                _closeSubject.OnNext(false);
            }));
        }

        public string Name
        {
            get { return _name.Value;  }
            set { _name.Value = value; }
        }

        public string Surname
        {
            get { return _surname.Value; }
            set { _surname.Value = value; }
        }

        public int Age
        {
            get { return _age.Value; }
            set { _age.Value = value; }
        }

        public string Address1
        {
            get { return _address1.Value; }
            set { _address1.Value = value; }
        }

        public string Address2
        {
            get { return _address2.Value; }
            set { _address2.Value = value; }
        }

        public string Address3
        {
            get { return _address3.Value; }
            set { _address3.Value = value; }
        }

        public string City
        {
            get { return _city.Value; }
            set { _city.Value = value; }
        }

        public ICommand EditCommand
        {
            get { return _editCommand;  }
        }

        public ICommand SaveCommand
        {
            get { return _saveCommand; }
        }

        public ICommand DeleteCommand
        {
            get { return _deleteCommand; }
        }

        public ICommand CloseCommand
        {
            get { return _closeCommand; }
        }

        private void UpdateModel()
        {
            _model = this.ToModel();
        }

        private void ResetData()
        {
            Name = _model.Name;
            Surname = _model.Surname;
            Surname = _model.Surname;
            Age = _model.Age;
            Address1 = _model.Address1;
            Address2 = _model.Address2;
            Address3 = _model.Address3;
            City = _model.City;
        }

        IObservable<bool> ICloseable.Close
        {
            get { return _closeSubject.AsObservable();  }
        }
    }
}
