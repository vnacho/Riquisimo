using LazZiya.ExpressLocalization.Messages;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Ferpuser.Models
{
    public class CostCenterProduct : BaseModel
    {
        [Required(ErrorMessage = DataAnnotationsErrorMessages.RequiredAttribute_ValidationError)]
        [Display(Name = "Congreso")]
        public Guid CongressId { get; set; }
        [Display(Name = "Congreso")]
        public Congress Congress { get; set; }

        [Display(Name = "Cliente")]
        public Guid? ClientId { get; set; }
        [Display(Name = "Cliente")]
        public Client Client { get; set; }

        [Display(Name = "Dirección de facturación")]
        public Guid? BillingLocationId { get; set; }
        [Display(Name = "Dirección de facturación")]
        public ClientLocation BillingLocation { get; set; }
        [Display(Name = "Vendedor")]
        public Account Account { get; set; }
        [Display(Name = "Vendedor")]
        public Guid? AccountId { get; set; }

        #region ProductData
        [Display(Name = "Artículo")]
        public string Product { get; set; }
        [Display(Name = "Descripción del artículo")]
        public string ProductDescription { get; set; }
        [Display(Name = "Observaciones del artículo")]
        public string ProductNotes { get; set; }

        [Display(Name = "Serie")]
        public string Serie { get; set; }
        [Required(ErrorMessage = DataAnnotationsErrorMessages.RequiredAttribute_ValidationError)]
        [Display(Name = "Precio")]
        public decimal Fee { get; set; }

        [NotMapped]
        public string FeeDisplay => Fee.ToString().Replace(".", ",");

        [Display(Name = "Unidades")]
        public double Units { get; set; } = 1;

        [Required(ErrorMessage = DataAnnotationsErrorMessages.RequiredAttribute_ValidationError)]
        [Display(Name = "IVA")]
        public virtual string VATId { get; set; } = "03";


        [Required(ErrorMessage = DataAnnotationsErrorMessages.RequiredAttribute_ValidationError)]
        [Display(Name = "Forma de pago")]
        public string FPag { get; set; } = "00";

        [Display(Name = "Tipo de documento")]
        public DocumentType DocumentType { get; set; }

        [Required(ErrorMessage = DataAnnotationsErrorMessages.RequiredAttribute_ValidationError)]
        [Display(Name = "Tipo de documento")]
        public Guid DocumentTypeId { get; set; } = new Guid("51e91cdd-bfbd-4081-8a60-eaee0a9bea39");

        [Display(Name = "Base imponible")]
        public decimal GetTaxBase()
        {
            return Fee / (1 + (VAT / 100));
        }
        public decimal VAT { get; set; } = new decimal(21);
        [Display(Name = "Incluir datos del evento")]
        public bool ShowCostCenterInfoOnInvoice { get; set; } = true;

        [NotMapped]
        public virtual decimal BasePrice
        {
            get
            {
                return GetTaxBase();
            }
            set
            {
                Fee = value * (1 + (VAT / 100));
            }
        }
        [NotMapped]
        public virtual string BasePriceStr
        {
            get
            {
                return BasePrice.ToString("F6").Replace(",", ".");
            }
        }
        [NotMapped]
        public virtual decimal TotalPrice
        {
            get
            {
                return BasePrice * new decimal(Units);
            }
        }
        [NotMapped]
        public virtual string TotalPriceStr
        {
            get
            {
                return Math.Round(TotalPrice, 2).ToString("F6").Replace(",", ".");
            }
        }
        [NotMapped]
        public virtual decimal BasePriceVAT
        {
            get
            {
                return Fee;
            }
        }
        [NotMapped]
        public virtual string BasePriceVATStr
        {
            get
            {
                return Math.Round(BasePriceVAT, 2).ToString("F6").Replace(",", ".");
            }
        }
        [NotMapped]
        public virtual decimal TotalPriceVAT
        {
            get
            {
                return Fee * new decimal(Units);
            }
        }
        [NotMapped]
        public virtual string TotalPriceVATStr
        {
            get
            {
                return TotalPriceVAT.ToString("F6").Replace(",", ".");
            }
        }
        #endregion

        #region InvoicingData

        [Display(Name = "Número de documento")]
        public int Number { get; set; }

        [Display(Name = "Nº Factura")]
        public string InvoiceNumber { get; set; }

        [Display(Name = "Fecha de facturación")]
        public DateTime? InvoiceDate { get; set; }

        [Display(Name = "Fecha de cobro")]
        public DateTime? PaidDate { get; set; }

        #endregion

        #region Status
        [Display(Name = "Importado")]
        public bool Imported { get; set; }

        [Display(Name = "Pagado")]
        public bool Paid { get; set; }

        [Display(Name = "Notificado")]
        public bool Notified { get; set; }

        [Display(Name = "Autorización")]
        public bool Authorization { get; set; }

        [Display(Name = "Solo facturación")]
        public bool OnlyBilling { get; set; }

        [Display(Name = "Facturado")]
        public bool Exported { get; set; }

        [Display(Name = "Revisado")]
        public bool Reviewed { get; set; }
        #endregion

        [Display(Name = "Comentarios")]
        public string Notes { get; set; }

        public string GetStatus()
        {
            if (Paid)
            {
                return "Pagado";
            }
            if (Exported)
            {
                return "Facturado";
            }
            if (Reviewed)
            {
                return "Revisado";
            }
            if (Imported)
            {
                return "Importado";
            }
            return "Creado";            
        }
    }
}
