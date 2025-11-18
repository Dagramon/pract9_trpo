using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace pract7_trpo
{
    public class Info : INotifyPropertyChanged
    {
        private int jsonfiles { get; set ; }
        private int patients { get; set; }
        private int doctors { get; set; }

        private static string findString { get; set; } = "";

        public static ObservableCollection<Patient> PatientsList = new();

        public int JSONFiles
        {
            get { return jsonfiles; }
            set { jsonfiles = value; OnPropertyChanged() ; }
        }
        public int Patients
        {
            get { return patients; }
            set { patients = value; OnPropertyChanged(); }
        }
        public int Doctors
        {
            get { return doctors; }
            set { doctors = value; OnPropertyChanged(); }
        }

        public string FindString
        {
            get { return findString; }
            set { findString = value; OnPropertyChanged(); UpdateList(); }
        }
        private static string GetDoctorByID(string ID)
        {
            if (File.Exists($"D_{ID}"))
            {
                string jsonString = File.ReadAllText($"D_{ID}");
                Doctor doctor = JsonSerializer.Deserialize<Doctor>(jsonString);
                return doctor.Name;
            }

            return "Не найден";
        }

        public static void UpdateList()
        {
            PatientsList.Clear();
            if (File.Exists("Patient_IDS.txt"))
            {
                using (StreamReader sr = File.OpenText("Patient_IDS.txt"))
                {
                    while (!sr.EndOfStream)
                    {
                        Patient patient;
                        string id = sr.ReadLine();
                        string jsonString = File.ReadAllText($"P_{id}");
                        patient = JsonSerializer.Deserialize<Patient>(jsonString);
                        patient.LastDoctorName = GetDoctorByID(patient.LastDoctor.ToString());
                        if (patient.ID.StartsWith(findString))
                        {
                            PatientsList.Add(patient);
                        }
                        else if (findString == string.Empty)
                        { 
                            PatientsList.Add(patient);
                        }
                    }
                }
            }
            else
            {
                File.CreateText("Patient_IDS.txt");
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string? propname = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs (propname));
        }
    }
}
