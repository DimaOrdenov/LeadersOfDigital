using System;
using NoTryCatch.BL.Core;

namespace LeadersOfDigital.BusinessLayer
{
    public class ExtendedUserContext : UserContext
    {
        public string FirstName { get; private set; }

        public string SecondName { get; private set; }

        public string LastName { get; private set; }

        public event EventHandler UserContextChanged;

        public void SetContext(string firstName, string secondName, string lastName)
        {
            FirstName = firstName;
            SecondName = secondName;
            LastName = lastName;

            UserContextChanged?.Invoke(this, new EventArgs());
        }
    }
}
