using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Логика взаимодействия для AddPage.xaml
    /// </summary>
    public partial class AddPage : Page
    {
        public ObservableCollection<Patient> patients { get; set; }
        Patient addedPatient = new Patient();
        Doctor currentDoctor;
        Random rng = new Random();
        Info info;
        public AddPage(Doctor _currentDoctor, ObservableCollection<Patient> _patients, Info _info)
        {
            InitializeComponent();
            info = _info;
            currentDoctor = _currentDoctor;
            patients = _patients;
            DataContext = addedPatient;
        }
        private string CreatePatientID()
        {
            if (!File.Exists("Patient_IDS.txt"))
            {
                StreamWriter sw1 = new StreamWriter("Patient_IDS.txt");
                sw1.Close();
            }
            string[] takenIDS = File.ReadAllLines("Patient_IDS.txt");
            int newID;
            bool taken = false;
            do
            {
                newID = rng.Next(1000000, 10000000);
                foreach (string id in takenIDS)
                {
                    if (Convert.ToInt32(id) == newID)
                    {
                        taken = true;
                    }
                }

            } while (taken == true);

            StreamWriter sw = File.AppendText("Patient_IDS.txt");
            sw.WriteLine(newID);
            sw.Close();

            return newID.ToString();
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
        private void AddPatient_Click(object sender, RoutedEventArgs e)
        {
            if (currentDoctor.ID != null)
            {
                if (addedPatient.Name != null &&
                    addedPatient.LastName != null &&
                    addedPatient.MiddleName != null &&
                    addedPatient.Birthday != null &&
                    addedPatient.PhoneNumber != null)
                {
                    addedPatient.LastDoctor = Convert.ToInt32(currentDoctor.ID);
                    string ID = CreatePatientID();
                    addedPatient.LastDoctorName = GetDoctorByID(addedPatient.LastDoctor.ToString());
                    addedPatient.ID = ID;
                    string jsonString = JsonSerializer.Serialize(addedPatient);
                    File.WriteAllText($"P_{ID}", jsonString);
                    MessageBox.Show($"Пациент добавлен с идентификатором {ID}");
                    info.JSONFiles++;
                    info.Patients++;
                    patients.Add(addedPatient);
                    NavigationService.GoBack();
                }

                else
                {
                    MessageBox.Show("Все поля должны быть заполнены");
                }
            }
            else
            {
                MessageBox.Show("Врач должен войти");
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
