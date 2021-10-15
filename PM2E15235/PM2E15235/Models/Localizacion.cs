using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace PM2E15235.Models
{
    public class Localizacion
    {
        [PrimaryKey, AutoIncrement]
        public int id { get; set; }
        public double latitud { get; set; }

        public double longitud { get; set; }

        public String descripcionLarga { get; set; }

        public String descripcionCorta { get; set; }

        public override string ToString()
        {
            return "Descripcion Corta: " + descripcionCorta + " | Descripcion Larga: " + descripcionLarga + " | Latitud: "
                + latitud + "Longitud: " + longitud;
        }
    }
}
