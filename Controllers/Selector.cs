using Ferpuser.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ferpuser.Controllers
{
    public class Selector
    {
        public static Accommodation SelectAccommodation(Accommodation r)
        {
            var a = new Accommodation
            {
                Imported = r.Imported,
                Reviewed = r.Reviewed,
                Exported = r.Exported,
                StartDate = r.StartDate,
                Number = r.Number,
                EndDate = r.EndDate,
                Paid = r.Paid,
                Fee = r.Fee,
                Id = r.Id,
                ClientId = r.ClientId,
                CongressId = r.CongressId,
                RegistrantId = r.RegistrantId,
                InvoiceNumber = r.InvoiceNumber
            };

            if (r.RoomType != null) {
                a.RoomType = new RoomType
                {
                    Id = r.RoomTypeId,
                    Name = r.RoomType.Name,
                    Occupants = r.RoomType.Occupants
                };
            }

            if (r.Registrant != null)
            {
                a.Registrant = new Registrant
                {
                    Id = r.RegistrantId,
                    Name = r.Registrant.Name,
                    Surnames = r.Registrant.Surnames
                };
            }

            if (r.Congress != null)
            {
                a.Congress = new Congress
                {
                    Id = r.CongressId,
                    Name = r.Congress.Name,
                    Number = r.Congress.Number,
                    Code = r.Congress.Code
                };
            }

            if (r.Client != null)
            {
                a.Client = new Client
                {
                    Id = r.ClientId.Value,
                    BusinessName = r.Client.BusinessName
                };
            }
            return a;
        }

        public static Registration SelectRegistration(Registration r)
        {
            var a = new Registration
            {
                Imported = r.Imported,
                Reviewed = r.Reviewed,
                Exported = r.Exported,
                Paid = r.Paid,
                Fee = r.Fee,
                Id = r.Id,
                ClientId = r.ClientId,
                CongressId = r.CongressId,
                RegistrantId = r.RegistrantId
            };
            if (r.Registrant != null)
            {
                a.Registrant = new Registrant
                {
                    Id = r.RegistrantId,
                    Name = r.Registrant.Name,
                    Surnames = r.Registrant.Surnames
                };
            }

            if (r.Congress != null)
            {
                a.Congress = new Congress
                {
                    Id = r.CongressId,
                    Name = r.Congress.Name,
                    Number = r.Congress.Number,
                    Code = r.Congress.Code
                };
            }

            if (r.Client != null)
            {
                a.Client = new Client
                {
                    Id = r.ClientId.Value,
                    BusinessName = r.Client.BusinessName
                };
            }
            return a;
        }
    }
}
