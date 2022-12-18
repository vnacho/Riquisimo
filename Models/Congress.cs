using IbanNet;
using IbanNet.DataAnnotations;
using LazZiya.ExpressLocalization.Messages;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Ferpuser.Models
{
    public class Congress : CostCenter
    {
        #region Main data
        [Required(ErrorMessage = DataAnnotationsErrorMessages.RequiredAttribute_ValidationError)]
        [Remote("CheckIfCodeExists", "Congresses", AdditionalFields = "Code,Id", ErrorMessage = "Ya existe un congreso con este código")]
        [Range(1, 99999, ErrorMessage = "El valor no está en el rango [1, 99999]")]
        [Display(Name = "Código")]
        public int Number { get; set; }
        //[Range(0, 999, ErrorMessage = "El valor no está en el rango [1, 99999]")]
        //[Display(Name = "Almacen")]
        //public int Warehouse { get; set; } = 0;
        [Display(Name = "Lugar")]
        public string Place { get; set; }

        [Required(ErrorMessage = DataAnnotationsErrorMessages.RequiredAttribute_ValidationError)]
        [Display(Name = "Fecha de inicio")]
        public DateTime StartDate { get; set; } = DateTime.Now;

        [Required(ErrorMessage = DataAnnotationsErrorMessages.RequiredAttribute_ValidationError)]
        [Display(Name = "Fecha de finalización")]
        public DateTime EndDate { get; set; } = DateTime.Now.AddDays(1);


        //[Required(ErrorMessage = DataAnnotationsErrorMessages.RequiredAttribute_ValidationError)]
        //[Range(0.01, 99.99, ErrorMessage = "El valor no está en el rango (0, 100)")]
        //[Display(Name = "IVA")]
        //public decimal IVA { get; set; } = new decimal(21);

        [Required(ErrorMessage = DataAnnotationsErrorMessages.RequiredAttribute_ValidationError)]
        [Remote("CheckIfCodeExists", "Congresses", AdditionalFields = "Id,Number", ErrorMessage = "Ya existe un congreso con este código")]
        [Display(Name = "Clave")]
        [RegularExpression("^[A-Z0-9]*$", ErrorMessage = "El código solo debe contener mayúsculas y números")]
        [StringLength(5, ErrorMessage = "El código debe tener 5 carácteres como máximo")]
        public string Code { get; set; }

        //[Required(ErrorMessage = DataAnnotationsErrorMessages.RequiredAttribute_ValidationError)]
        [Display(Name = "Cadena de conexión")]
        public string ConnectionString { get; set; }
        [Display(Name = "Cadena de conexión")]
        public string NewConnectionString
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(ConnectionString) && string.IsNullOrWhiteSpace(DatabaseServer))
                {
                    return ConnectionString;
                } else
                {
                    return "Server=" + DatabaseServer + ";Database=" + Database + ";User=" + DatabaseUser + ";Password=" + DatabasePassword + ";";
                }
            }
        }

        //Datos de servidor mysql para componer cadena de conexión. Por ejemplo: Server=36enfermeriatraumatologia.com;Database=cot36_bdatos;User=cot36_user;Password="g7RcU@$;Advk";
        [Display(Name = "Servidor BDD")]
        public string DatabaseServer { get; set; }
        [Display(Name = "Nombre BDD")]
        public string Database { get; set; }
        [Display(Name = "Usuario BDD")]
        public string DatabaseUser { get; set; }
        [Display(Name = "Contraseña BDD")]
        public string DatabasePassword { get; set; }
        [Display(Name = "Prefijo de la base de datos")]
        public string DatabasePrefix { get; set; }
        //*****

        //Datos de servidor mysql para componer cadena de conexión para comités/ponentes. Por ejemplo: Server=36enfermeriatraumatologia.com;Database=cot36_bdatos;User=cot36_user;Password="g7RcU@$;Advk";
        [Display(Name = "Servidor BDD (Comité)")]
        public string DatabaseServerComite { get; set; }
        [Display(Name = "Nombre BDD (Comité)")]
        public string DatabaseComite { get; set; }
        [Display(Name = "Usuario BDD (Comité)")]
        public string DatabaseUserComite { get; set; }
        [Display(Name = "Contraseña BDD (Comité)")]
        public string DatabasePasswordComite { get; set; }
        [Display(Name = "Prefijo de la base de datos (Comité)")]
        public string DatabasePrefixComite { get; set; }

        /// <summary>
        /// Método que obtiene la cadena de conexión para datos de comite
        /// </summary>
        public string NewConnectionStringComite
        {
            get
            {            
                if (!string.IsNullOrWhiteSpace(DatabaseServerComite) &&
                    !string.IsNullOrWhiteSpace(DatabaseComite) &&
                    !string.IsNullOrWhiteSpace(DatabaseUserComite) &&
                    !string.IsNullOrWhiteSpace(DatabasePasswordComite))
                   return "Server=" + DatabaseServerComite + ";Database=" + DatabaseComite + ";User=" + DatabaseUserComite + ";Password=" + DatabasePasswordComite + ";";
                return NewConnectionString;
            }
        }

        public string GetDatabasePrefixComite()
        {
            if (!string.IsNullOrWhiteSpace(DatabasePrefixComite))
                return DatabasePrefixComite;
            return DatabasePrefix;
        }
        //*****


        [Display(Name = "Ocultar inscripciones")]
        public bool HideRegistrations { get; set; }
        [Display(Name = "Finalizado")]
        public bool Ended { get; set; }

        [Display(Name = "Número de expediente")]
        public string CertificateNumber { get; set; }
        [Display(Name = "Acreditador")]
        public string CertificateCreditor { get; set; }
        [Display(Name = "Número de créditos")]
        public double CertificateCredits { get; set; }
        [Display(Name = "Ciudad de certificado")]
        public string CertificateCity { get; set; }
        [Display(Name = "Fecha de certificado")]
        public DateTime CertificateTime { get; set; } = DateTime.Now;
        #endregion

        #region Extra fields
        [Range(1, 999999, ErrorMessage = "El valor no está en el rango [1, 999999]")]
        [Display(Name = "Código Interno")]
        public int InternalCode { get; set; } = 1;

        [Display(Name = "Organiza")]
        public string Organizer { get; set; }
        [Display(Name = "Dominio Web")]
        public string WebDomain { get; set; }
        [Display(Name = "Sede")]
        public string Headquarters { get; set; }
        [Display(Name = "Asistentes")]
        [Range(1, 99999, ErrorMessage = "El valor no está en el rango [1, 99999]")]
        public int Attendants { get; set; } = 1;

        [Display(Name = "Precio de Inscripción")]
        [Range(1, 99999.99, ErrorMessage = "El valor no está en el rango [1, 99999,99]")]
        public decimal InscriptionFee { get; set; } = 1;


        [Display(Name = "Hotel Ponentes")]
        public string SpeakersHotel { get; set; }

        [Display(Name = "Restauración")]
        public string Catering { get; set; }


        [Display(Name = "Presidente")]
        public string President { get; set; }


        [Display(Name = "Centro de trabajo")]
        public string Workplace { get; set; }

        [Display(Name = "Cargo")]
        public string Position { get; set; }

        [Display(Name = "Teléfono personal")]
        [Phone]
        public string PersonalPhone { get; set; }

        [Display(Name = "Teléfono corporativo")]
        [Phone]
        public string CorporatePhone { get; set; }


        [Display(Name = "Email")]
        [EmailAddress]
        public string Email { get; set; }

        [Display(Name = "Email alternative")]
        [EmailAddress]
        public string Email2 { get; set; }


        [Display(Name = "Comunicaciones y tipo")]
        public string CommunicationsAndType { get; set; }

        [Display(Name = "Secretaría Técnica")]
        public string TechnicalSecretariat { get; set; }

        [Display(Name = "Secretaría Científica")]
        public string ScientificSecretariat { get; set; }
        [Display(Name = "Infraestructuras")]
        public string Infrastructures { get; set; }

        [Display(Name = "Comunicaciones y RR.SS.")]
        public string CommunicationsAndSocialMedia { get; set; }

        [Display(Name = "Financiación")]
        public string Financing { get; set; }

        [Display(Name = "Viajes")]
        public string Travels { get; set; }

        [Range(1, 99999999.99, ErrorMessage = "El valor no está en el rango [1, 99999,99]")]
        [Display(Name = "Presupuesto")]
        public decimal Budget { get; set; } = 1;

        [Display(Name = "Notas a tener en cuenta")]
        public string Notes { get; set; }

        public List<Registration> Registrations { get; set; }
        public List<Accommodation> Accommodations { get; set; }
        public List<Expense> Expenses { get; set; }
        public List<CongressEmailAccounts> CongressEmailAccounts { get; set; }

        #endregion

        #region Campos Es Obra
        [Display(Name = "Tipo evento")]
        [StringLength(1)]
        public string TipoCongress { get; set; }

        [Display(Name = "Cliente")]
        public string NombreCliente { get; set; } 

        [Display(Name = "Dirección Obra")]
        [StringLength(40, ErrorMessage = "40 carácteres como máximo")]
        public string DireccionObra { get; set; }

        [Display(Name = "Código postal Obra")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "El código solo debe contener números")]
        [StringLength(5, ErrorMessage = "El código debe tener 5 carácteres como máximo")]
        public string CodigoPostalObra { get; set; }

        [Display(Name = "Población Obra")]
        [StringLength(40, ErrorMessage = "40 carácteres como máximo")]
        public string PoblacionObra { get; set; } 

        [Display(Name = "Provincia Obra")]
        public string ProvinciaObra { get; set; } 

        [Display(Name = "Fecha inicio")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime FechaInicio { get; set; } = DateTime.Now;

        [Display(Name = "Email facturación")]
        public string EmailFacturacion { get; set; }

        [Display(Name = "Email documentación")]
        public string EmailDocumentacion { get; set; }

        [Display(Name = "Finalizada")]
        public bool Finalizada { get; set; }
        public ICollection<DocumentoObra> DocumentosObra { get; set; } = new List<DocumentoObra>();
        public ICollection<ContratoObra> ContratosObra { get; set; } = new List<ContratoObra>();

        [StringLength(15, ErrorMessage = "15 carácteres como máximo")]
        public string Plataforma { get; set; }

        [DataType(DataType.Url)]
        [Url]
        public string Url { get; set; }

        [StringLength(30, ErrorMessage = "30 carácteres como máximo")]
        public string Usuario { get; set; }

        //[DataType(DataType.Password)]
        [StringLength(30, ErrorMessage = "30 carácteres como máximo")]
        public string Contraseña { get; set; }

        #endregion         //Fin Campos ES Obra

        public ICollection<Encuentro> Encuentros { get; set; }

        [Display(Name = "Sociedad científica")]
        public int? SociedadCientificaId { get; set; }
        [Display(Name = "Sociedad científica")]
        public SociedadCientifica SociedadCientifica { get; set; }

        #region methods
        public string GetDayAndTimePrint()
        {
            string res = Place;
            if (StartDate.Date == EndDate.Date)
            {
                res += " el " + EndDate.ToString("d \\de MMMM \\de yyyy");
            }
            else
            {
                res += " del ";
                if (StartDate.Month == EndDate.Month && StartDate.Year == EndDate.Year)
                {
                    res += StartDate.Day;
                }
                else if (StartDate.Year == EndDate.Year)
                {
                    res += StartDate.ToString("d \\de MMMM");
                }
                else
                {
                    res += StartDate.ToString("d \\de MMMM \\de yyyy");
                }
                res += " al " + EndDate.ToString("d \\de MMMM \\de yyyy");
            }
            return res;
        }

        public decimal TotalContratos()
        {
            decimal resultado = 0;
            foreach (ContratoObra contrato in ContratosObra)
            {
                resultado += contrato.ImporteContrato;
            }

            return resultado;
        }

        public string DisplayName
        {
            get
            {
                return Number + " " + Name;
            }
        }
        #endregion
    }
}
