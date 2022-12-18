using LazZiya.ExpressLocalization.Messages;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Ferpuser.Models
{
    public class Product : BaseModel
    {
        public Expense Expense { get; set; }
        public Guid ExpenseId { get; set; }
        [Display(Name = "Artículo")]
        public string ProductCode { get; set; }
        [Display(Name = "Descripción del artículo")]
        public string ProductDescription { get; set; } = "";
        [Display(Name = "Observaciones del artículo")]
        public string ProductNotes { get; set; }

        [Required(ErrorMessage = DataAnnotationsErrorMessages.RequiredAttribute_ValidationError)]
        [Display(Name = "Precio")]
        public decimal Fee { get; set; }
        [Display(Name = "Unidades")]
        public double Units { get; set; } = 1;

        [Required(ErrorMessage = DataAnnotationsErrorMessages.RequiredAttribute_ValidationError)]
        [Display(Name = "IVA")]
        public string VATId { get; set; } = "03";

        public decimal VAT { get; set; } = new decimal(21);

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
                return GetTaxBase() * new decimal(Units);
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
        [Display(Name = "Base imponible")]
        public decimal GetTaxBase()
        {
            return Fee / (1 + (VAT / 100));
        }
    }
}
