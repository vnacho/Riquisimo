using Ferpuser.BLL.Filters;
using Ferpuser.BLL.Managers;
using Ferpuser.Data;
using Ferpuser.Models;
using Ferpuser.Models.Dtos;
using Ferpuser.Models.Enums;
using Ferpuser.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Ferpuser.Controllers
{
    [Authorize(Policy = "Collaborations")]
    public class InformeResumenEmpresaController : ControlPresupuestarioBaseController
    {
        public ControlPresupuestarioManager _manager { get; set; }
        private readonly ApplicationDbContext _db; 

        public InformeResumenEmpresaController(ControlPresupuestarioManager manager, ApplicationDbContext db)
        {
            _manager = manager;
            _db = db;
        }

        public IActionResult Index()
        {
            InformeResumenEmpresaViewModel model = new InformeResumenEmpresaViewModel() { Year = DateTime.Now.Year };
            return View(model);
        }

        [HttpPost, ActionName("Index")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> IndexConfirmed()
        {
            InformeResumenEmpresaViewModel model = new InformeResumenEmpresaViewModel();

            await TryUpdateModelAsync<InformeResumenEmpresaViewModel>(model, "",
                f => f.Month,
                f => f.Year);

            if (!ModelState.IsValid) //Salir, no continuar
                return View(model);

            //Borrar los registros de origen del mes seleccionado, se recalcularán siempre
            _db.Origen.RemoveRange(_db.Origen.Where(f =>
                f.Mes == model.Month &&
                f.Anio == model.Year));
            _db.SaveChanges();

            List<InformeResumenEmpresaDto> listDirectos = new List<InformeResumenEmpresaDto>();
            List<InformeResumenEmpresaDto> listIndirectos = new List<InformeResumenEmpresaDto>();
            List<InformeResumenEmpresaDto> listEstructuras = new List<InformeResumenEmpresaDto>();
            List<InformeResumenEmpresaDto> listE9 = new List<InformeResumenEmpresaDto>();

            List<VerOrigen> listVerOrigen = _db.VerOrigen.ToList();

            //Se obtiene los items del mes
            ControlPresupuestarioFilter filterMes = new ControlPresupuestarioFilter()
            {
                Tipo = TipoInformeControlPresupuestario.N1,
                FechaDesde = new DateTime(model.Year.Value, model.Month.Value, 1),
                FechaHasta = new DateTime(model.Year.Value, model.Month.Value, 1).AddMonths(1).AddSeconds(-1)
            };            

            //E9
            var listE9Mes = _db.GrabacionE9
                .Include(f => f.CentroCoste).ThenInclude(f => f.TipoCentroCoste)
                .Include(f => f.Destino).ThenInclude(f => f.TipoCentroCoste)
                .Where(f => f.Fecha >= filterMes.FechaDesde && f.Fecha <= filterMes.FechaHasta);
            foreach (var item in listE9Mes)
            {
                var e9 = listE9.FirstOrDefault(f =>
                    f.TipoCosteCode == item.CentroCoste.NivelAnalitico1 &&
                    f.CentroCosteCode == item.CentroCoste.NivelAnalitico2);

                if (e9 == null)
                {
                    listE9.Add(new InformeResumenEmpresaDto() //Se indicó que no se iba a diferenciar Entrada/Salida
                    {
                        CentroCosteCode = item.CentroCoste.NivelAnalitico2?.Trim(),
                        TipoCosteCode = item.CentroCoste.NivelAnalitico1?.Trim(),
                        CentroCosteNombre = item.CentroCoste?.Nombre,
                        TipoCosteNombre = item.CentroCoste?.TipoCentroCoste?.Descripcion,
                        HaberMes = item.Importe
                    });
                }
                else
                {
                    e9.HaberMes += item.Importe;
                }
            }

            IEnumerable<ControlPresupuestarioDto> listMes = _manager.Get(filterMes);
            foreach (var item in listMes)
            {
                var centros =_db.CentrosCoste
                    .Include(f => f.TipoCentroCoste)
                    .Where(f => f.NivelAnalitico1 == item.TipoCosteCode && f.NivelAnalitico2 == item.CentroCosteCode);
                if (centros == null || !centros.Any())
                    continue;

                CentroCoste cc = centros.First();
                    
                InformeResumenEmpresaDto dto = new InformeResumenEmpresaDto()
                {
                    CentroCosteCode = item.CentroCosteCode?.Trim(),
                    CentroCosteNombre = item.CentroCosteNombre,
                    CuentaCode = item.CuentaCode?.Trim(),
                    CuentaNombre = item.CuentaNombre,
                    DebeMes = item.Debe,
                    HaberMes = item.Haber,
                    TipoCosteCode = item.TipoCosteCode?.Trim(),
                    TipoCosteNombre = item.TipoCosteNombre,
                    AddOrigen = listVerOrigen.Any(f => f.NivelAnalitico1 == item.TipoCosteCode?.Trim())
                };
                switch (cc.TipoCentroCoste.Tipo.ToUpper()?.Trim())
                {
                    case "D":
                        listDirectos.Add(dto);
                        break;
                    case "I":
                        listIndirectos.Add(dto);
                        break;
                    case "E":
                        listEstructuras.Add(dto);
                        break;
                    case "Z":
                        listE9.Add(dto);
                        break;
                }
            }                

            //Partes de personal
            var listPersonalMes = _db.PartePersonal
                .Include(f => f.CentroCoste).ThenInclude(f => f.TipoCentroCoste)
                .Where(f => f.Fecha >= filterMes.FechaDesde && f.Fecha <= filterMes.FechaHasta)
                .OrderBy(f => f.PersonalId);

            Personal p = null;
            foreach (PartePersonal item in listPersonalMes)
            {
                //Debe
                InformeResumenEmpresaDto dto = new InformeResumenEmpresaDto()
                {
                    CentroCosteCode = item.CentroCoste.NivelAnalitico2?.Trim(),
                    CentroCosteNombre = item.CentroCoste.Nombre,
                    DebeMes = item.Importe,
                    TipoCosteCode = item.CentroCoste.NivelAnalitico1?.Trim(),
                    TipoCosteNombre = item.CentroCoste.TipoCentroCoste.Descripcion,
                    AddOrigen = listVerOrigen.Any(f => f.NivelAnalitico1 == item.CentroCoste.NivelAnalitico1?.Trim())
                };

                InformeResumenEmpresaDto existente = null;
                switch (item.CentroCoste.TipoCentroCoste.Tipo.ToUpper()?.Trim())
                {
                    case "D":
                        existente = listDirectos.FirstOrDefault(f => f.CentroCosteCode == dto.CentroCosteCode);
                        if (existente == null)
                            listDirectos.Add(dto);
                        else
                            existente.DebeMes += dto.DebeMes;
                        break;
                    case "I":
                        existente = listIndirectos.FirstOrDefault(f => f.TipoCosteCode == dto.TipoCosteCode && f.CentroCosteCode == dto.CentroCosteCode);
                        if (existente == null)
                            listIndirectos.Add(dto);
                        else
                            existente.DebeMes += dto.DebeMes;
                        break;
                    case "E":
                        existente = listEstructuras.FirstOrDefault(f => f.TipoCosteCode == dto.TipoCosteCode && f.CentroCosteCode == dto.CentroCosteCode);
                        if (existente == null)
                            listEstructuras.Add(dto);
                        else
                            existente.DebeMes += dto.DebeMes;
                        break;
                    case "Z":
                        existente = listE9.FirstOrDefault(f => f.TipoCosteCode == dto.TipoCosteCode && f.CentroCosteCode == dto.CentroCosteCode);
                        if (existente == null)
                            listE9.Add(dto);
                        else
                            existente.DebeMes += dto.DebeMes;
                        break;
                }

                //Haber => Se inserta en el centro de coste del personal
                if (p == null || p.Id != item.PersonalId)
                    p = _db.Personal.Include(f => f.CentroCoste.TipoCentroCoste).SingleOrDefault(f => f.Id == item.PersonalId);

                if (p == null)
                    continue;

                dto = new InformeResumenEmpresaDto()
                {
                    CentroCosteCode = p.CentroCoste.NivelAnalitico2?.Trim(),
                    CentroCosteNombre = p.CentroCoste.Nombre,
                    HaberMes = item.Importe,
                    TipoCosteCode = p.CentroCoste.NivelAnalitico1?.Trim(),
                    TipoCosteNombre = p.CentroCoste.TipoCentroCoste.Descripcion,
                    AddOrigen = listVerOrigen.Any(f => f.NivelAnalitico1 == p.CentroCoste.NivelAnalitico1?.Trim())
                };

                switch (p.CentroCoste.TipoCentroCoste.Tipo.ToUpper()?.Trim())
                {
                    case "D":
                        existente = listDirectos.FirstOrDefault(f => f.TipoCosteCode == dto.TipoCosteCode && f.CentroCosteCode == dto.CentroCosteCode);
                        if (existente == null)
                            listDirectos.Add(dto);
                        else
                            existente.HaberMes += dto.HaberMes;
                        break;
                    case "I":
                        existente = listIndirectos.FirstOrDefault(f => f.TipoCosteCode == dto.TipoCosteCode && f.CentroCosteCode == dto.CentroCosteCode);
                        if (existente == null)
                            listIndirectos.Add(dto);
                        else
                            existente.HaberMes += dto.HaberMes;
                        break;
                    case "E":
                        existente = listEstructuras.FirstOrDefault(f => f.TipoCosteCode == dto.TipoCosteCode && f.CentroCosteCode == dto.CentroCosteCode);
                        if (existente == null)
                            listEstructuras.Add(dto);
                        else
                            existente.HaberMes += dto.HaberMes;
                        break;
                    case "Z":
                        existente = listE9.FirstOrDefault(f => f.TipoCosteCode == dto.TipoCosteCode && f.CentroCosteCode == dto.CentroCosteCode);
                        if (existente == null)
                            listE9.Add(dto);
                        else
                            existente.HaberMes += dto.HaberMes;
                        break;
                }
            }


            //Partes de almacén
            var listAlmacenMes = _db.PartesInternosAlmacen
                .Include(f => f.Destino)
                .Include(f => f.ArticulosAlmacen).ThenInclude(f => f.CentroCoste)
                .Where(f => f.fecha >= filterMes.FechaDesde && f.fecha <= filterMes.FechaHasta)
                .OrderBy(f => f.ArticulosAlmacenId);

            ArticulosAlmacen a = null;
            foreach (PartesInternosAlmacen item in listAlmacenMes)
            {
                //Debe
                InformeResumenEmpresaDto dto = new InformeResumenEmpresaDto()
                {
                    CentroCosteCode = item.Destino.NivelAnalitico2?.Trim(),
                    CentroCosteNombre = item.Destino.Nombre,
                    DebeMes = item.Amount,
                    TipoCosteCode = item.Destino.NivelAnalitico1?.Trim(),
                    TipoCosteNombre = item.Destino.TipoCentroCoste.Descripcion,
                    AddOrigen = listVerOrigen.Any(f => f.NivelAnalitico1 == item.Destino.NivelAnalitico1?.Trim())
                };

                InformeResumenEmpresaDto existente = null;
                switch (item.Destino.TipoCentroCoste.Tipo.ToUpper()?.Trim())
                {
                    case "D":
                        existente = listDirectos.FirstOrDefault(f => f.CentroCosteCode == dto.CentroCosteCode);
                        if (existente == null)
                            listDirectos.Add(dto);
                        else
                            existente.DebeMes += dto.DebeMes;
                        break;
                    case "I":
                        existente = listIndirectos.FirstOrDefault(f => f.TipoCosteCode == dto.TipoCosteCode && f.CentroCosteCode == dto.CentroCosteCode);
                        if (existente == null)
                            listIndirectos.Add(dto);
                        else
                            existente.DebeMes += dto.DebeMes;
                        break;
                    case "E":
                        existente = listEstructuras.FirstOrDefault(f => f.TipoCosteCode == dto.TipoCosteCode && f.CentroCosteCode == dto.CentroCosteCode);
                        if (existente == null)
                            listEstructuras.Add(dto);
                        else
                            existente.DebeMes += dto.DebeMes;
                        break;
                    case "Z":
                        existente = listE9.FirstOrDefault(f => f.TipoCosteCode == dto.TipoCosteCode && f.CentroCosteCode == dto.CentroCosteCode);
                        if (existente == null)
                            listE9.Add(dto);
                        else
                            existente.DebeMes += dto.DebeMes;
                        break;
                }

                //Haber => Se inserta en el centro de coste del registro
                if (a == null || a.Id != item.ArticulosAlmacenId)
                    a = _db.ArticulosAlmacen.Include(f => f.CentroCoste.TipoCentroCoste).SingleOrDefault(f => f.Id == item.ArticulosAlmacenId);

                if (a == null)
                    continue;

                dto = new InformeResumenEmpresaDto()
                {
                    CentroCosteCode = a.CentroCoste.NivelAnalitico2?.Trim(),
                    CentroCosteNombre = a.CentroCoste.Nombre,
                    HaberMes = item.Amount,
                    TipoCosteCode = a.CentroCoste.NivelAnalitico1?.Trim(),
                    TipoCosteNombre = a.CentroCoste.TipoCentroCoste.Descripcion,
                    AddOrigen = listVerOrigen.Any(f => f.NivelAnalitico1 == a.CentroCoste.NivelAnalitico1?.Trim())
                };

                switch (a.CentroCoste.TipoCentroCoste.Tipo.ToUpper()?.Trim())
                {
                    case "D":
                        existente = listDirectos.FirstOrDefault(f => f.TipoCosteCode == dto.TipoCosteCode && f.CentroCosteCode == dto.CentroCosteCode);
                        if (existente == null)
                            listDirectos.Add(dto);
                        else
                            existente.HaberMes += dto.HaberMes;
                        break;
                    case "I":
                        existente = listIndirectos.FirstOrDefault(f => f.TipoCosteCode == dto.TipoCosteCode && f.CentroCosteCode == dto.CentroCosteCode);
                        if (existente == null)
                            listIndirectos.Add(dto);
                        else
                            existente.HaberMes += dto.HaberMes;
                        break;
                    case "E":
                        existente = listEstructuras.FirstOrDefault(f => f.TipoCosteCode == dto.TipoCosteCode && f.CentroCosteCode == dto.CentroCosteCode);
                        if (existente == null)
                            listEstructuras.Add(dto);
                        else
                            existente.HaberMes += dto.HaberMes;
                        break;
                    case "Z":
                        existente = listE9.FirstOrDefault(f => f.TipoCosteCode == dto.TipoCosteCode && f.CentroCosteCode == dto.CentroCosteCode);
                        if (existente == null)
                            listE9.Add(dto);
                        else
                            existente.HaberMes += dto.HaberMes;
                        break;
                }
            }

            
            foreach (GrabacionE9 e9 in listE9Mes)
            {
                var destino = listDirectos.FirstOrDefault(f =>
                    f.TipoCosteCode == e9.Destino.NivelAnalitico1 &&
                    f.CentroCosteCode == e9.Destino.NivelAnalitico2);
                if (destino == null)
                {
                    destino = listIndirectos.FirstOrDefault(f =>
                        f.TipoCosteCode == e9.Destino.NivelAnalitico1 &&
                        f.CentroCosteCode == e9.Destino.NivelAnalitico2);
                }
                if (destino == null)
                {
                    destino = listEstructuras.FirstOrDefault(f =>
                        f.TipoCosteCode == e9.Destino.NivelAnalitico1 &&
                        f.CentroCosteCode == e9.Destino.NivelAnalitico2);
                }
                if (destino == null)
                {
                    InformeResumenEmpresaDto dto = new InformeResumenEmpresaDto()
                    {
                        CentroCosteCode = e9.Destino.NivelAnalitico2.Trim(),
                        CentroCosteNombre = e9.Destino.Nombre,
                        CuentaCode = string.Empty,
                        CuentaNombre = string.Empty,
                        DebeMes = e9.Importe,
                        TipoCosteCode = e9.Destino.NivelAnalitico1.Trim(),
                        TipoCosteNombre = string.Empty,
                        AddOrigen = listVerOrigen.Any(f => f.NivelAnalitico1 == e9.Destino.NivelAnalitico1.Trim())
                    };
                    switch (e9.Destino.TipoCentroCoste.Tipo)
                    {
                        case "D":
                            listDirectos.Add(dto);
                            break;
                        case "I":
                            listIndirectos.Add(dto);
                            break;
                        case "E":
                            listEstructuras.Add(dto);
                            break;
                        case "Z":
                            listE9.Add(dto);
                            break;
                    }
                }
                else
                {
                    destino.DebeMes += e9.Importe;
                }
            }

            //Cálculos para porcentaje de distribución
            decimal porcentajeDistribucion = 0;
            decimal totalCantidadDistribuida = 0;
            var tip = _db.TiposCentroCoste.FirstOrDefault(f => f.Tipo == "E");
            if (tip != null)
                porcentajeDistribucion = tip.PorcentajeDistribucion;
            foreach (var item in listDirectos)
            {
                decimal cantidadDistribuida = item.HaberMes * porcentajeDistribucion / 100;
                item.DebeMes += cantidadDistribuida;
                totalCantidadDistribuida += cantidadDistribuida;
            }

            InformeResumenEmpresaDto cargaestructura = new InformeResumenEmpresaDto() //totalCantidadDistribuida
            {
                CentroCosteCode = "90000",
                CentroCosteNombre = "REPARTO ESTRUCTURA",
                HaberMes = totalCantidadDistribuida,
                TipoCosteCode = "900",
                TipoCosteNombre = "CARGAS DE ESTRUCTURA",
                AddOrigen = listVerOrigen.Any(f => f.NivelAnalitico1 == "900")
            };
            if (cargaestructura.TasaRendimientoMes != 0) //Solo se mostrará en el informe si la tasa es distinta de 0%
                listEstructuras.Add(cargaestructura);            

            //Meter los costes del año que faltan porque en el mes consultado no aparecen
            var listOrigenAnio = _db.Origen.AsNoTracking().Where(f =>
                f.Anio == model.Year &&
                f.Mes < model.Month);
            foreach (var item in listOrigenAnio)
            {
                switch (item.Tipo)
                {
                    case TipoRegistro.Directo:
                        var directo = listDirectos.FirstOrDefault(f => f.TipoCosteCode == item.NivelAnalitico1?.Trim() && f.CentroCosteCode == item.NivelAnalitico2?.Trim());
                        if (directo == null)
                        {
                            listDirectos.Add(new InformeResumenEmpresaDto()
                            {
                                CentroCosteCode = item.NivelAnalitico2?.Trim(),
                                CentroCosteNombre = item.NombreNivelAnalitico2?.Trim(),
                                TipoCosteCode = item.NivelAnalitico1?.Trim(),
                                TipoCosteNombre = item.NombreNivelAnalitico1?.Trim(),
                                HaberAnio = item.Ingresos,
                                DebeAnio = item.Gastos,
                                AddOrigen = listVerOrigen.Any(f => f.NivelAnalitico1 == item.NivelAnalitico1?.Trim())
                            });
                        }
                        else
                        {
                            directo.HaberAnio += item.Ingresos;
                            directo.DebeAnio += item.Gastos;
                        }
                        break;
                    case TipoRegistro.Indirecto:
                        var indirecto = listIndirectos.FirstOrDefault(f => f.TipoCosteCode == item.NivelAnalitico1?.Trim() && f.CentroCosteCode == item.NivelAnalitico2?.Trim());
                        if (indirecto == null)
                        {
                            listIndirectos.Add(new InformeResumenEmpresaDto()
                            {
                                CentroCosteCode = item.NivelAnalitico2?.Trim(),
                                CentroCosteNombre = item.NombreNivelAnalitico2?.Trim(),
                                TipoCosteCode = item.NivelAnalitico1?.Trim(),
                                TipoCosteNombre = item.NombreNivelAnalitico1?.Trim(),
                                HaberAnio = item.Ingresos,
                                DebeAnio = item.Gastos,
                                AddOrigen = listVerOrigen.Any(f => f.NivelAnalitico1 == item.NivelAnalitico1?.Trim())
                            });
                        }  
                        else
                        {
                            indirecto.HaberAnio += item.Ingresos;
                            indirecto.DebeAnio += item.Gastos;
                        }
                        break;
                    case TipoRegistro.Estructuras:
                        var estructuras = listEstructuras.FirstOrDefault(f => f.TipoCosteCode == item.NivelAnalitico1?.Trim() && f.CentroCosteCode == item.NivelAnalitico2?.Trim());
                        if (estructuras == null) 
                        {
                            if (item.NivelAnalitico2?.Trim() != "90000") //El reparto de estructura no se muestra si no se ha mostrado el mes (tasa == 0)
                            {
                                listEstructuras.Add(new InformeResumenEmpresaDto()
                                {
                                    CentroCosteCode = item.NivelAnalitico2?.Trim(),
                                    CentroCosteNombre = item.NombreNivelAnalitico2?.Trim(),
                                    TipoCosteCode = item.NivelAnalitico1?.Trim(),
                                    TipoCosteNombre = item.NombreNivelAnalitico1?.Trim(),
                                    HaberAnio = item.Ingresos,
                                    DebeAnio = item.Gastos,
                                    AddOrigen = listVerOrigen.Any(f => f.NivelAnalitico1 == item.NivelAnalitico1?.Trim())
                                });
                            }
                        }
                        else
                        {
                            estructuras.HaberAnio += item.Ingresos;
                            estructuras.DebeAnio += item.Gastos;
                        }
                        break;
                    case TipoRegistro.E9:
                        var e9 = listE9.FirstOrDefault(f => f.TipoCosteCode == item.NivelAnalitico1?.Trim() && f.CentroCosteCode == item.NivelAnalitico2?.Trim());
                        if (e9 == null)
                        {
                            listE9.Add(new InformeResumenEmpresaDto()
                            {
                                CentroCosteCode = item.NivelAnalitico2?.Trim(),
                                CentroCosteNombre = item.NombreNivelAnalitico2?.Trim(),
                                TipoCosteCode = item.NivelAnalitico1?.Trim(),
                                TipoCosteNombre = item.NombreNivelAnalitico1?.Trim(),
                                HaberAnio = item.Ingresos,
                                DebeAnio = item.Gastos,
                                AddOrigen = listVerOrigen.Any(f => f.NivelAnalitico1 == item.NivelAnalitico1?.Trim())
                            });
                        }
                        else
                        {
                            e9.HaberAnio += item.Ingresos;
                            e9.DebeAnio += item.Gastos;
                        }
                        break;
                }
            }
            
            //Actualizar los valores del año sumando los del mes consultado 
            foreach (var item in listDirectos)
            {
                //Sumar al anio consultado
                item.DebeAnio += item.DebeMes;
                item.HaberAnio += item.HaberMes;

                if (!item.AddOrigen)
                    continue;

                //Aquí se obtienen los valores desde el origen
                //Origen (todos los registros desde origen incluso años anteriores hasta la fecha selecccionada)
                var listOrigen = _db.Origen.AsNoTracking().Where(f =>
                    f.NivelAnalitico1 == item.TipoCosteCode &&
                    f.NivelAnalitico2 == item.CentroCosteCode &&
                    (f.Anio < model.Year || (f.Anio == model.Year && f.Mes < model.Month)));

                item.DebeOrigen = listOrigen.Sum(f => f.Gastos) + item.DebeMes;
                item.HaberOrigen = listOrigen.Sum(f => f.Ingresos) + item.HaberMes;
            }
            foreach (var item in listIndirectos) //Sumar al anio consultado
            {
                //Sumar al anio consultado
                item.DebeAnio += item.DebeMes;
                item.HaberAnio += item.HaberMes;

                if (!item.AddOrigen)
                    continue;

                //Aquí se obtienen los valores desde el origen 
                //Origen (todos los registros desde origen incluso años anteriores hasta la fecha selecccionada)
                var listOrigen = _db.Origen.AsNoTracking().Where(f =>
                    f.NivelAnalitico1 == item.TipoCosteCode &&
                    f.NivelAnalitico2 == item.CentroCosteCode &&
                    (f.Anio < model.Year || (f.Anio == model.Year && f.Mes < model.Month)));

                item.DebeOrigen = listOrigen.Sum(f => f.Gastos) + item.DebeMes;
                item.HaberOrigen = listOrigen.Sum(f => f.Ingresos) + item.HaberMes;
            }
            foreach (var item in listEstructuras) //Sumar al anio consultado
            {
                //Sumar al anio consultado
                item.DebeAnio += item.DebeMes;
                item.HaberAnio += item.HaberMes;

                if (!item.AddOrigen)
                    continue;

                //Aquí se obtienen los valores desde el origen
                //Origen (todos los registros desde origen incluso años anteriores hasta la fecha selecccionada)
                var listOrigen = _db.Origen.AsNoTracking().Where(f =>
                    f.NivelAnalitico1 == item.TipoCosteCode &&
                    f.NivelAnalitico2 == item.CentroCosteCode &&
                    (f.Anio < model.Year || (f.Anio == model.Year && f.Mes < model.Month)));

                item.DebeOrigen = listOrigen.Sum(f => f.Gastos) + item.DebeMes;
                item.HaberOrigen = listOrigen.Sum(f => f.Ingresos) + item.HaberMes;
            }
            foreach (var item in listE9) //Sumar al anio consultado
            {
                //Sumar al anio consultado
                item.DebeAnio += item.DebeMes;
                item.HaberAnio += item.HaberMes;

                if (!item.AddOrigen)
                    continue;

                //Aquí se obtienen los valores desde el origen
                //Origen (todos los registros desde origen incluso años anteriores hasta la fecha selecccionada)
                var listOrigen = _db.Origen.AsNoTracking().Where(f =>
                    f.NivelAnalitico1 == item.TipoCosteCode &&
                    f.NivelAnalitico2 == item.CentroCosteCode &&
                    (f.Anio < model.Year || (f.Anio == model.Year && f.Mes < model.Month)));

                item.DebeOrigen = listOrigen.Sum(f => f.Gastos) + item.DebeMes;
                item.HaberOrigen = listOrigen.Sum(f => f.Ingresos) + item.HaberMes;
            }

            model.ItemsDirectos = listDirectos;
            model.ItemsIndirectos = listIndirectos;
            model.ItemsEstructuras = listEstructuras;
            model.ItemsE9 = listE9;
                
            model.Calculate();

            //Guardar los datos a la tabla origen de este mes
            Origen origen;
            //Directos
            foreach(var item in listDirectos) 
            {   
                origen = new Origen() 
                { 
                    Anio = model.Year.Value,
                    FechaInforme = DateTime.Now,
                    Gastos = item.DebeMes,
                    Ingresos = item.HaberMes,
                    Mes = model.Month.Value,
                    NivelAnalitico1 = item.TipoCosteCode,
                    NivelAnalitico2 = item.CentroCosteCode,
                    NombreNivelAnalitico1 = item.TipoCosteNombre,
                    NombreNivelAnalitico2 = item.CentroCosteNombre,
                    Tipo = TipoRegistro.Directo                        
                };
                _db.Origen.Add(origen);                
            }
            //Indirectos
            foreach (var item in listIndirectos)
            {   
                origen = new Origen()
                {
                    Anio = model.Year.Value,
                    FechaInforme = DateTime.Now,
                    Gastos = item.DebeMes,
                    Ingresos = item.HaberMes,
                    Mes = model.Month.Value,
                    NivelAnalitico1 = item.TipoCosteCode,
                    NivelAnalitico2 = item.CentroCosteCode,
                    NombreNivelAnalitico1 = item.TipoCosteNombre,
                    NombreNivelAnalitico2 = item.CentroCosteNombre,
                    Tipo = TipoRegistro.Indirecto
                };
                _db.Origen.Add(origen);                
            }
            //Estructuras
            foreach (var item in listEstructuras)
            {                
                origen = new Origen()
                {
                    Anio = model.Year.Value,
                    FechaInforme = DateTime.Now,
                    Gastos = item.DebeMes,
                    Ingresos = item.HaberMes,
                    Mes = model.Month.Value,
                    NivelAnalitico1 = item.TipoCosteCode,
                    NivelAnalitico2 = item.CentroCosteCode,
                    NombreNivelAnalitico1 = item.TipoCosteNombre,
                    NombreNivelAnalitico2 = item.CentroCosteNombre,
                    Tipo = TipoRegistro.Estructuras
                };
                _db.Origen.Add(origen);                
            }
            //E9
            foreach (var item in listE9)
            {
                origen = new Origen()
                {
                    Anio = model.Year.Value,
                    FechaInforme = DateTime.Now,
                    Gastos = item.DebeMes,
                    Ingresos = item.HaberMes,
                    Mes = model.Month.Value,
                    NivelAnalitico1 = item.TipoCosteCode,
                    NivelAnalitico2 = item.CentroCosteCode,
                    NombreNivelAnalitico1 = item.TipoCosteNombre,
                    NombreNivelAnalitico2 = item.CentroCosteNombre,
                    Tipo = TipoRegistro.E9
                };
                _db.Origen.Add(origen);
            }
            _db.SaveChanges();

            return View(model);
        }
    }
}
