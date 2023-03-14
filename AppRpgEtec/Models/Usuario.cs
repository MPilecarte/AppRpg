using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppRpgEtec.Models
{
    public class Usuario
    {
        public int Id { get; set; }
        public String UserName { get; set; }
        public String PasswordString { get; set; }
        public String Perfil { get; set; }
        public String Token { get; set; }
        public byte[] Foto { get; set; }
        public String Email { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    } 
}
