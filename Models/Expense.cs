using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Ferpuser.Models
{
    public class Expense : CostCenterProduct
    {
        public List<Product> Products { get; set; } = new List<Product>();
        //[NotMapped]
        //public override decimal BasePrice
        //{
        //    get
        //    {
        //        decimal sum = base.BasePrice;
        //        foreach (var p in Products)
        //        {
        //            sum += p.BasePrice;
        //        }
        //        return sum;
        //    }
        //}

        //[NotMapped]
        //public override decimal TotalPrice
        //{
        //    get
        //    {
        //        decimal sum = base.TotalPrice;
        //        foreach (var p in Products)
        //        {
        //            sum += p.TotalPrice;
        //        }
        //        return sum;
        //    }
        //}

        //[NotMapped]
        //public override decimal BasePriceVAT
        //{
        //    get
        //    {
        //        decimal sum = base.BasePriceVAT;
        //        foreach (var p in Products)
        //        {
        //            sum += p.BasePriceVAT;
        //        }
        //        return sum;
        //    }
        //}

        //[NotMapped]
        //public override decimal TotalPriceVAT
        //{
        //    get
        //    {
        //        decimal sum = base.TotalPriceVAT;
        //        foreach (var p in Products)
        //        {
        //            sum += p.TotalPriceVAT;
        //        }
        //        return sum;
        //    }
        //}

        public List<decimal> GetDistinctVATs()
        {
            var d = Products.Where(p => p.VAT != VAT).Select(p => p.VAT).Distinct().ToList();
            d.Add(VAT);
            return d;
        }

        public decimal TotalOfVAT(decimal vat)
        {
            decimal total = new decimal(0);
            if (VAT == vat)
            {
                total += base.TotalPrice;
            }
            total += Products.Where(p => p.VAT == vat).Sum(p => p.TotalPrice);
            return total;
        }
        public decimal TotalVATOfVAT(decimal vat)
        {
            decimal total = new decimal(0);
            if (VAT == vat)
            {
                total += base.TotalPriceVAT;
            }
            total += Products.Where(p => p.VAT == vat).Sum(p => p.TotalPriceVAT);
            return total - TotalOfVAT(vat);
        }

        public decimal TotalVAT
        {
            get
            {
                return base.TotalPriceVAT + Products.Sum(p => p.TotalPriceVAT);
            }
        }
    }
}
