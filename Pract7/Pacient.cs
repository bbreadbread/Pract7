using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Pract7
{
	class Pacient : INotifyPropertyChanged
    {
        private string name;
		private string lastName;
		private string middleName;
		private string birthday;
		private string lastAppointment;
		private string lastDoctor;
		private string diagnosis;
		private string recomendations;

        
        public string Name { get { return name; } set { name = value; } }
		public string LastName { get { return lastName; } set { lastName = value; } }
		public string MiddleName { get { return middleName; } set { middleName = value; } }
        public string Birthday { get { return birthday; } set { birthday = value; } }
		public string LastAppointment { get { return lastAppointment; } set { lastAppointment = value; } }
        public string LastDoctor { get { return lastDoctor; } set { lastDoctor = value; } }
		public string Diagnosis { get { return diagnosis; } set { diagnosis = value; } }
		public string Recomendations { get { return recomendations; } set { recomendations = value; } }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
	
