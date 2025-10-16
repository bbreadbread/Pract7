using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Pract7
{
    class Doctor : INotifyPropertyChanged
    {
        private string name;
        private string lastName;
        private string middleName;
        private string specialisation;
        private string password;

       
        public string Name { get { return name; } set { name = value; } }
        public string LastName { get { return lastName; } set { lastName = value; } }
        public string MiddleName { get { return middleName; } set { middleName = value; } }
        public string Specialisation { get { return specialisation; } set { specialisation = value; } }
        public string Password { get { return password; } set { password = value; } }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}

