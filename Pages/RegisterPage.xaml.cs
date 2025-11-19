using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
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
    /// Логика взаимодействия для RegisterPage.xaml
    /// </summary>
    public partial class RegisterPage : Page
    {
        public RegisterPage(Info _info)
        {
            InitializeComponent();
            info = _info;
            RegForm.DataContext = registeredDoctor;
        }

        private Doctor registeredDoctor = new Doctor();
        public Info info;
        Random rng = new Random();

        private string CreateID()
        {
            if (!File.Exists("IDS.txt"))
            {
                StreamWriter sw1 = new StreamWriter("IDS.txt");
                sw1.Close();
            }
            string[] takenIDS = File.ReadAllLines("IDS.txt");
            int newID;
            bool taken = false;
            do
            {
                newID = rng.Next(10000, 100000);
                foreach (string id in takenIDS)
                {
                    if (Convert.ToInt32(id) == newID)
                    {
                        taken = true;
                    }
                }

            } while (taken == true);

            StreamWriter sw = File.AppendText("IDS.txt");
            sw.WriteLine(newID);
            sw.Close();

            return newID.ToString();
        }
        private void Register_Click(object sender, RoutedEventArgs e)
        {
            if (registeredDoctor.Name != null &&
                registeredDoctor.LastName != null &&
                registeredDoctor.MiddleName != null &&
                registeredDoctor.Specialization != null &&
                registeredDoctor.Password != null)
            {
                if (registeredDoctor.Password == registeredDoctor.RepeatPassword)
                {
                    string ID = CreateID();
                    registeredDoctor.ID = ID;
                    string jsonString = JsonSerializer.Serialize(registeredDoctor);
                    File.WriteAllText($"D_{ID}", jsonString);
                    MessageBox.Show($"Доктор зарегестрирован с идентификатором {ID}");
                    registeredDoctor = new Doctor();
                    RegForm.DataContext = registeredDoctor;
                    info.JSONFiles++;
                    info.Doctors++;
                    NavigationService.GoBack();
                }
                else
                {
                    MessageBox.Show("Пароли не совпадают");
                }
            }
            else
            {
                MessageBox.Show("Все поля должны быть заполнены");
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
