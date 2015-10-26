using AddressBook.ViewModels;

namespace AddressBook.Messages
{
    public class DeletePersonMessage
    {
        public DeletePersonMessage(IPersonViewModel person)
        {
            Person = person;
        }

        public IPersonViewModel Person { get; private set; }
    }
}
