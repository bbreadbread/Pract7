using System.Text;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.IO;
using System.Text.RegularExpressions;

namespace Pract7
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainViewModel main;
        private bool userActivated = false;

        private string regPas = string.Empty;
        private string regConfirmPas = string.Empty;
        private string authPas = string.Empty;
        public MainWindow()
        {
            this.main = new MainViewModel();
            InitializeComponent();
            this.DataContext = this.main;
            UpdateInfo();
        }

        private void RegBut_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(main.NewDoctor.Name) ||
                string.IsNullOrWhiteSpace(main.NewDoctor.LastName) ||
                string.IsNullOrWhiteSpace(main.NewDoctor.MiddleName) ||
                string.IsNullOrWhiteSpace(main.NewDoctor.Specialisation))
            {
                MessageBox.Show("Все поля обязательны для заполнения");
                return;
            }

            if (regPas != regConfirmPas)
            {
                MessageBox.Show("Пароли не совпадают");
                return;
            }

            main.NewDoctor.Password = regPas;

            string id = GenerateId(0);
            string file = Path.Combine("Data", $"D_{id}.json");

            File.WriteAllText(file, JsonSerializer.Serialize(main.NewDoctor));

            MessageBox.Show($"Доктор сохранён в {file}");
            UpdateInfo();
        }

        private void authAuthBut_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(main.AuthId) || string.IsNullOrWhiteSpace(authPas))
            {
                MessageBox.Show("Введите ID и пароль");
                return;
            }

            string file = Path.Combine("Data", $"D_{main.AuthId}.json");
            if (!File.Exists(file))
            {
                MessageBox.Show("Пользователь не найден");
                return;
            }

            var doc = JsonSerializer.Deserialize<Doctor>(File.ReadAllText(file));
            if (doc == null || doc.Password != authPas)
            {
                MessageBox.Show("Неверный пароль");
                return;
            }

            userActivated = true;
            addLastDocTB.Text = main.AuthId;
            main.Doctor = doc;
        }

        private void SearchBut_Click(object sender, RoutedEventArgs e)
        {
            if (!userActivated)
            {
                MessageBox.Show("Сначала войдите в систему");
                return;
            }

            string filePath = Path.Combine("Data", $"P_{main.SearchId}.json");

            if (!File.Exists(filePath))
            {
                MessageBox.Show("Пользователь не найден");
                return;
            }

            string json = File.ReadAllText(filePath);
            var pac = JsonSerializer.Deserialize<Pacient>(json);

            string docFile = Path.Combine("Data", $"D_{main.AuthId}.json");
            if (File.Exists(docFile))
            {
                var doc = JsonSerializer.Deserialize<Doctor>(File.ReadAllText(docFile));
                recepLastDocTB.Text = $"{doc.LastName} {doc.Name} {doc.MiddleName}";
            }
            else
            {
                recepLastDocTB.Text = "Доктор не найден";
            }
            
            main.Pacient = pac;
        }

        private void RecepBut_Click(object sender, RoutedEventArgs e)
        {
            if (!userActivated)
            {
                MessageBox.Show("Сначала войдите в систему");
                return;

            }

            main.Pacient.LastDoctor = main.AuthId;

            string file = Path.Combine("Data", $"P_{main.SearchId}.json");
            File.WriteAllText(file, JsonSerializer.Serialize(main.Pacient));
            

            string docFile = Path.Combine("Data", $"D_{main.AuthId}.json");
            if (File.Exists(docFile))
            {
                var doc = JsonSerializer.Deserialize<Doctor>(File.ReadAllText(docFile));
                recepLastDocTB.Text = $"{doc.LastName} {doc.Name} {doc.MiddleName}";
            }
            else
            {
                recepLastDocTB.Text = "Доктор не найден";
            }
            MessageBox.Show("Данные приёма сохранены");
        }

        private void AddBut_Click(object sender, RoutedEventArgs e)
        {
            if (!userActivated)
            {
                MessageBox.Show("Сначала войдите в систему");
                return;
            }

            main.NewPacient.LastDoctor = addLastDocTB.Text;

            string jsonString = JsonSerializer.Serialize(main.NewPacient);
            string filePath = Path.Combine("Data", $"P_{GenerateId(1)}.json");
            File.WriteAllText(filePath, jsonString);

            MessageBox.Show($"Пациент сохранён в {filePath}");
            UpdateInfo();
        }

        private void editBut_Click(object sender, RoutedEventArgs e)
        {
            if (!userActivated)
            {
                MessageBox.Show("Сначала войдите в систему");
                return;
            }

            if (string.IsNullOrEmpty(main.SearchId))
            {
                MessageBox.Show("Сначала найдите пациента");
                return;
            }

            string filePath = Path.Combine("Data", $"P_{main.SearchId}.json");
            var pac = JsonSerializer.Deserialize<Pacient>(File.ReadAllText(filePath));

            string docFile = Path.Combine("Data", $"D_{main.AuthId}.json");

            if (File.Exists(docFile))
            {
                var doc = JsonSerializer.Deserialize<Doctor>(File.ReadAllText(docFile));
                recepLastDocTB.Text = $"{doc.LastName} {doc.Name} {doc.MiddleName}";
            }
            else
            {
                recepLastDocTB.Text = "Доктор не найден";
            }

            string currentDiagnosis = pac.Diagnosis;
            string currentRecomendations = pac.Recomendations;

            pac.Name = main.Pacient.Name;
            pac.LastName = main.Pacient.LastName;
            pac.MiddleName = main.Pacient.MiddleName;
            pac.Birthday = main.Pacient.Birthday;
            pac.LastAppointment = main.Pacient.LastAppointment;
            

            pac.Diagnosis = currentDiagnosis;
            pac.Recomendations = currentRecomendations;

            File.WriteAllText(filePath, JsonSerializer.Serialize(pac, new JsonSerializerOptions { WriteIndented = true }));

            //я не смогла сделать обновление без сброса диагноза и рекомендаций (без помощи доп. переменных, при помощи привязок), поэтому оно сбрасывает все.
            main.Pacient = pac;

            MessageBox.Show("Изменения сохранены");
        }

        private void resetBut_Click(object sender, RoutedEventArgs e)
        {
            if (!userActivated)
            {
                MessageBox.Show("Сначала войдите в систему");
                return;
            }

            if (string.IsNullOrEmpty(main.SearchId))
            {
                MessageBox.Show("Сначала найдите пациента");
                return;
            }

            string filePath = Path.Combine("Data", $"P_{main.SearchId}.json");
            var pac = JsonSerializer.Deserialize<Pacient>(File.ReadAllText(filePath));

            //я не смогла сделать сброс без сброса диагноза и рекомендаций (без помощи доп. переменных, при помощи привязок), поэтому оно сбрасывает все.
            main.Pacient = pac;

            MessageBox.Show("Данные сброшены к файлу");
        }
        private string GenerateId(int role)
        {
            var rnd = new Random();
            while (true)
            {

                string file;
                int id;

                if (role == 0)
                {
                    id = rnd.Next(10_000, 100_000);
                    file = Path.Combine("Data", $"D_{id}.json");
                }
                else
                {
                    id = rnd.Next(1000_000, 10000_000);
                    file = Path.Combine("Data", $"P_{id}.json");
                }

                if (!File.Exists(file)) return id.ToString();
            }

        }

        private void UpdateInfo()
        {
            if (!Directory.Exists("Data"))
            {
                totalTB.Text = "Нет данных";
                docsTB.Text = "";
                patsTB.Text = "";
                return;
            }

            var files = Directory.EnumerateFiles("Data", "*.json");
            int total = files.Count();
            int docs = files.Count(f => Path.GetFileName(f).StartsWith("D_"));
            int pats = files.Count(f => Path.GetFileName(f).StartsWith("P_"));

            totalTB.Text = $"Всего файлов: {total}";
            docsTB.Text = $"Докторов: {docs}";
            patsTB.Text = $"Пациентов: {pats}";
        }

        /////////а
        private void RegPasChanged(object sender, RoutedEventArgs e)
           => regPas = ((PasswordBox)sender).Password;

        private void RegConfirmChanged(object sender, RoutedEventArgs e)
            => regConfirmPas = ((PasswordBox)sender).Password;

        private void AuthPasChanged(object sender, RoutedEventArgs e)
            => authPas = ((PasswordBox)sender).Password;
    }
}