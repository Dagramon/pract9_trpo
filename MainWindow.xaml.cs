using System.Text;
using System.IO;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using pract7_trpo.Pages;

namespace pract7_trpo
{
    
    public partial class MainWindow : Window
    {
        Random rng = new Random();
        private Doctor currentDoctor = new Doctor();
        private Doctor registeredDoctor = new Doctor();
        private Patient currentPatient = new Patient();
        private Patient addedPatient = new Patient();
        private Patient foundPatient = new Patient();
        Info info = new Info();
        public MainWindow()
        {
            InitializeComponent();

            UpdateInfo();
            DataContext = info;
            MainFrame.Navigate(new LoginPage(info));
        }
        private void UpdateInfo()
        {
            if (File.Exists("IDS.txt") && File.Exists("Patient_IDS.txt"))
            {
                string[] DoctorIDS = File.ReadAllLines("IDS.txt");
                foreach (string ID in DoctorIDS)
                {
                    if (File.Exists($"D_{ID}"))
                    {
                        info.JSONFiles++;
                        info.Doctors++;
                    }
                }
                string[] PatientIDS = File.ReadAllLines("Patient_IDS.txt");
                foreach (string ID in PatientIDS)
                {
                    if (File.Exists($"P_{ID}"))
                    {
                        info.JSONFiles++;
                        info.Patients++;
                    }
                }
            }  
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            ThemeHelper.Toggle();
        }
    }
}