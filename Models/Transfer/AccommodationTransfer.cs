using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ferpuser.Models.Transfer
{
    public class AccommodationTransfer
    {
        public int Id { get; set; }
        public string Nif { get; set; }
        public string Mail { get; set; }
        public string Nombre { get; set; }
        public string Apellidos { get; set; }
        public string Direccion { get; set; }
        public string Poblacion { get; set; }
        public int? Provincia { get; set; }
        public string Pais { get; set; }
        public string CodigoPostal { get; set; }
        public string Movil { get; set; }
        public string Telefono { get; set; }
        public string Mail2 { get; set; }
        public string Cargo { get; set; }
        public string CategoriaProfesional { get; set; }
        public int? CentroTrabajo { get; set; }
        public string CentroTrabajo2 { get; set; }
        public int? ProvinciaCt { get; set; }
        public int? IdHotel { get; set; }
        public string TipoHabitacion { get; set; }
        public DateTime FechaLlegada { get; set; }
        public DateTime FechaSalida { get; set; }
        public int? Precio { get; set; }
        public string FacNombre { get; set; }
        public string FacNif { get; set; }
        public string FacDireccion { get; set; }
        public string FacCodigoPostal { get; set; }
        public int? FacProvincia { get; set; }
        public string FacLocalidad { get; set; }
        public string FacPais { get; set; }
        public string FacMail { get; set; }
        public DateTime FechaInscripcion { get; set; }
        public string Comentarios { get; set; }
        public string NifAcompanya { get; set; }
        public string MailAcompanya { get; set; }
        public string NombreAcompanya { get; set; }
        public string ApellidosAcompanya { get; set; }
        public string DireccionAcompanya { get; set; }
        public string PoblacionAcompanya { get; set; }
        public int? ProvinciaAcompanya { get; set; }
        public string CodigoPostalAcompanya { get; set; }
        public string TelefonoAcompanya { get; set; }
        public string Cancelado { get; set; }
        public string Activo { get; set; }
    }
}
