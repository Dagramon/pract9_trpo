using System;
using System.IO;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
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
using System.Text.Json;
using System.Runtime.CompilerServices;

namespace pract7_trpo.Pages
{
    public partial class MainForm : Page
    {
        public Doctor currentDoctor { get; set; }
        public Patient? currentPatient { get; set; }

        public ObservableCollection<Patient> Patients { get; set; } = new();

        Info info = new Info();

        public MainForm(Doctor _currentDoctor, Info _info)
        {
            InitializeComponent();
            info = _info;
            Patients = Info.PatientsList;
            Info.UpdateList();
            currentDoctor = _currentDoctor;
            InfoForm.DataContext = currentDoctor;
            FindBox.DataContext = info;
            DataContext = this;
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddPage(currentDoctor, Info.PatientsList, info));
        }

        private void AppointmentButton_Click(object sender, RoutedEventArgs e)
        {
            if (currentPatient != null)
            {
                NavigationService.Navigate(new AppointmentPage(currentPatient));
            }
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (currentPatient != null)
            {
                NavigationService.Navigate(new EditPage(currentPatient));
            }
        }

        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            if (currentPatient != null)
            {
                if (MessageBox.Show(
                    $"Вы точно хотите удалить пациента {currentPatient.Name}",
                    "Подтверждение",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    info.JSONFiles--;
                    info.Patients--;
                    string[] IDS = File.ReadAllLines("Patient_IDS.txt");
                    using (StreamWriter sw = File.CreateText("Patient_IDS.txt"))
                    {
                        foreach (string id in IDS)
                        {
                            if (id != currentPatient.ID)
                            {
                                sw.WriteLine(id);
                            }
                        }
                    }
                    File.Delete($"P_{currentPatient.ID}");
                    currentPatient = null;
                    Info.UpdateList();
                }
            }
        }
    }
}
