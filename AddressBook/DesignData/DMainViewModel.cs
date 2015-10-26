using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using AddressBook.ViewModels;

namespace AddressBook.DesignData
{
    public class DMainViewModel : IMainViewModel
    {
        public DMainViewModel()
        {
            IsBusy = false;
            Clients = new ObservableCollection<IPersonViewModel>(
                new List<IPersonViewModel>
                {
                    new DPersonViewModel(),
                    new DPersonViewModel { Name = "Joe", Surname = "Bloggs" },
                    new DPersonViewModel { Name = "Jim", Surname = "Kirk", City = "Bristol" },
                    new DPersonViewModel { Name = "John", Surname = "McNamara", City = "Glasgow", Age = 24 },
                });
        }

        public ObservableCollection<IPersonViewModel> Clients { get; set; }
        public ICommand AddNewClientCommand { get; set; }
        public bool IsBusy { get; set; }
    }
}
