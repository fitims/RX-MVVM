using System.Windows.Input;
using AddressBook.ViewModels;

namespace AddressBook.DesignData
{
    public class DPersonViewModel : IPersonViewModel
    {
        public DPersonViewModel()
        {
            Name = "Joe";
            Surname = "Bloggs";
            Age = 40;
            Address1 = "Flat 1";
            Address2 = "Watergate House";
            Address3 = "Rotherhithe Street";
            City = "London";
        }

        public string Name { get; set; }
        public string Surname { get; set; }
        public int Age { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public string City { get; set; }
        public ICommand EditCommand { get; set; }
        public ICommand SaveCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        public ICommand CloseCommand { get; set; }
    }
}
