using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace pract7_trpo
{
    public class Appointment
    {
        public string dateString { get; set; }
        public int doctor_id { get; set; }
        public string diagnosis { get; set; }
        public string recomendations { get; set; }

        [JsonIgnore]

        private DateTime date = DateTime.Today;
        public DateTime Date
        {
            get { return date; }
            set { date = value; dateString = date.ToString("dd.MM.yyyy"); }
        }
    }
}
