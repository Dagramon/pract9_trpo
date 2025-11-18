using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace pract7_trpo.Pages
{
    /// <summary>
    /// Логика взаимодействия для AppointmentPage.xaml
    /// </summary>
    public partial class AppointmentPage : Page
    {
        Patient currentPatient;
        public AppointmentPage(Patient _currentPatient)
        {
            InitializeComponent();
            currentPatient = _currentPatient;
            DataContext = currentPatient;
        }
        private string GetDoctorByID(string ID)
        {
            if (File.Exists($"D_{ID}"))
            {
                string jsonString = File.ReadAllText($"D_{ID}");
                Doctor doctor = JsonSerializer.Deserialize<Doctor>(jsonString);
                return doctor.Name;
            }

            return "Не найден";
        }
        private void Reset_Click(object sender, RoutedEventArgs e)
        {
            if (currentPatient.ID != null)
            {
                if (File.Exists($"P_{currentPatient.ID}"))
                {
                    string jsonString = File.ReadAllText($"P_{currentPatient.ID}");
                    currentPatient = JsonSerializer.Deserialize<Patient>(jsonString);
                    currentPatient.LastDoctorName = GetDoctorByID(currentPatient.LastDoctor.ToString());
                    DataContext = currentPatient;
                }
            }
        }
        private void EditPatient_Click(object sender, RoutedEventArgs e)
        {
            if (File.Exists($"P_{currentPatient.ID}"))
            {
                Appointment appointment = new Appointment() { Date = DateTime.Today, diagnosis = currentPatient.Diagnosis, doctor_id = currentPatient.LastDoctor, recomendations = currentPatient.Recomendations};
                currentPatient.AppointmentStories.Add(appointment);
                string jsonString = JsonSerializer.Serialize(currentPatient);
                currentPatient.LastDoctorName = GetDoctorByID(currentPatient.LastDoctor.ToString());
                File.WriteAllText($"P_{currentPatient.ID}", jsonString);
                MessageBox.Show($"Данные о пациенте обновлены");
                NavigationService.GoBack();
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            Info.UpdateList();
            NavigationService.GoBack();
        }
    }
}
