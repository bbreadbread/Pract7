using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Pract7
{
    class MainViewModel : INotifyPropertyChanged
    {
        private Doctor doctor;
        private Pacient pacient;
        private Pacient newPacient;
        private Doctor newDoctor;

        public Doctor NewDoctor { get { return newDoctor; } set { newDoctor = value; onPropertyChanged("NewDoctor"); } }
        public Doctor Doctor {  get { return doctor; } set { doctor = value;  onPropertyChanged("Doctor"); } }
        public Pacient Pacient { get { return pacient; } set { pacient = value; onPropertyChanged("Pacient"); } }
        
        public Pacient NewPacient { get { return newPacient; } set { newPacient = value; onPropertyChanged("NewPacient"); } }
       
        private string authId = string.Empty;
        private string searchId = string.Empty;
        public string AuthId { get => authId; set { authId = value; onPropertyChanged(); } } 
        public string SearchId { get => searchId; set { searchId = value; onPropertyChanged(); } }
        public MainViewModel()
        {
            Doctor = new Doctor();
            Pacient = new Pacient();
            NewPacient = new Pacient();
            NewDoctor = new Doctor();
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        //взяла с видео на ютубе https://www.youtube.com/watch?v=3TU91YoyywQ
        void onPropertyChanged(params string[] propertyNames)
        {
            PropertyChangedEventHandler? handler = PropertyChanged;

            if (handler != null)
            {
                foreach (var property in propertyNames)
                {
                    handler(this, new PropertyChangedEventArgs(property));
                }
            }
        }
    }
}
