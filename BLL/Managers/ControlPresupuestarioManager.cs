using Ferpuser.BLL.Filters;
using Ferpuser.Data;
using Ferpuser.Models;
using Ferpuser.Models.Dtos;
using Ferpuser.Models.Enums;
using Ferpuser.Transfer;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;

namespace Ferpuser.BLL.Managers
{
    public class ControlPresupuestarioManager
    {
        private SageContextFactoryHelper _sageContextFactoryHelper;
        private readonly ApplicationDbContext _dbContext;

        public ControlPresupuestarioManager(SageContextFactoryHelper sageContextFactoryHelper, ApplicationDbContext dbContext)
        {
            _sageContextFactoryHelper = sageContextFactoryHelper;
            _dbContext = dbContext;
        }        

        public List<ControlPresupuestarioDto> Get(ControlPresupuestarioFilter filter)
        {
            if (filter.FechaHasta < filter.FechaDesde)
                return new List<ControlPresupuestarioDto>();

            switch (filter.Tipo)
            {
                case TipoInformeControlPresupuestario.N1:
                    return GetN1(filter);
                case TipoInformeControlPresupuestario.N2:
                    return GetN2(filter);
                case TipoInformeControlPresupuestario.N3:
                    return GetN3(filter);
                case TipoInformeControlPresupuestario.N4:
                    return GetN4(filter);
            }
            throw new Exception("No se ha seleccionado un tipo de informe válido");
        }

        private List<ControlPresupuestarioDto> GetN1(ControlPresupuestarioFilter filter)
        {
            List<ControlPresupuestarioDto> list = new List<ControlPresupuestarioDto>();

            string select = "SELECT A.SECUNDAR AS TipoCosteCode, C.NOMBRE AS DefinicionTipo, A.SECNIVEL2 AS " +
                " CentroCosteCode, D.NOMBRE AS DefinicionCentro, SUM(A.DEBE) AS debe, SUM(A.HABER) AS haber " +
                " FROM otrasien AS A LEFT OUTER JOIN " +
                " secundar AS D ON A.SECNIVEL2 = D.CODIGO LEFT OUTER JOIN " +
                " cuentas AS E ON A.CODCUEN = E.CODIGO INNER JOIN " +
                " secundar AS C ON A.SECUNDAR = C.CODIGO LEFT OUTER JOIN " +
                " nivel3 AS F ON LEFT(A.CODCUEN, 3) = F.CODIGO ";

            DbParameter parameter = null;

            List<int> ejercicios = GetEjercicios(filter.FechaDesde, filter.FechaHasta);

            foreach (int ejercicio in ejercicios)
            {
                SageContext _sageContext = _sageContextFactoryHelper.CreateSageContext(ejercicio);
                if (_sageContext == null)
                    continue;

                using (var command = _sageContext.Database.GetDbConnection().CreateCommand())
                {
                    List<string> listWhere = new List<string>();
                    listWhere.Add("A.EMPRESA IN ('01')");
                    listWhere.Add("LEFT(A.CODCUEN, 1) IN('6', '7')");
                    if (filter.FechaDesde.HasValue)
                    {
                        listWhere.Add("A.FECHA >= @FechaDesde");
                        parameter = command.CreateParameter();
                        parameter.ParameterName = "@FechaDesde";
                        parameter.Value = filter.FechaDesde.Value;
                        command.Parameters.Add(parameter);
                    }
                    if (filter.FechaHasta.HasValue)
                    {
                        listWhere.Add("A.FECHA <= @FechaHasta");
                        parameter = command.CreateParameter();
                        parameter.ParameterName = "@FechaHasta";
                        parameter.Value = filter.FechaHasta.Value;
                        command.Parameters.Add(parameter);
                    }
                    if (!string.IsNullOrWhiteSpace(filter.TipoCosteCode))
                    {
                        listWhere.Add("A.SECUNDAR = @TipoCosteCode");
                        parameter = command.CreateParameter();
                        parameter.ParameterName = "@TipoCosteCode";
                        parameter.Value = filter.TipoCosteCode;
                        command.Parameters.Add(parameter);
                    }
                    if (!string.IsNullOrWhiteSpace(filter.CentroCosteCode))
                    {
                        listWhere.Add("A.SECNIVEL2 = @CentroCosteCode");
                        parameter = command.CreateParameter();
                        parameter.ParameterName = "@CentroCosteCode";
                        parameter.Value = filter.CentroCosteCode;
                        command.Parameters.Add(parameter);
                    }
                    if (!string.IsNullOrWhiteSpace(filter.PrefijoCuenta))
                    {
                        listWhere.Add("LEFT(A.CODCUEN, 8) LIKE @PrefijoCuenta + '%'");
                        parameter = command.CreateParameter();
                        parameter.ParameterName = "@PrefijoCuenta";
                        parameter.Value = filter.PrefijoCuenta;
                        command.Parameters.Add(parameter);
                    }

                    string where = " WHERE " + String.Join(" AND ", listWhere);
                    string groupby = " GROUP BY A.PLANCONT, A.SECUNDAR, C.NOMBRE, A.SECNIVEL2,D.NOMBRE ";
                    string orderby = " ORDER BY TipoCosteCode, CentroCosteCode, DefinicionTipo, DefinicionCentro";

                    string query = select + where + groupby + orderby;

                    command.CommandText = query;

                    _sageContext.Database.OpenConnection();
                    using (var result = command.ExecuteReader())
                    {
                        // do something with result
                        while (result.Read())
                        {
                            ControlPresupuestarioDto dto = new ControlPresupuestarioDto()
                            {
                                TipoCosteCode = result[0].ToString(),
                                TipoCosteNombre = result[1].ToString(),
                                CentroCosteCode = result[2].ToString(),
                                CentroCosteNombre = result[3].ToString(),
                                Debe = Convert.ToDecimal(result[4]),
                                Haber = Convert.ToDecimal(result[5])
                            };

                            var item = list.FirstOrDefault(f =>
                                f.TipoCosteCode == dto.TipoCosteCode &&
                                f.CentroCosteCode == dto.CentroCosteCode &&
                                f.CuentaCode == dto.CuentaCode);

                            if (item == null)
                                list.Add(dto);
                            else
                            {
                                item.Debe += dto.Debe;
                                item.Haber += dto.Haber;
                            }
                        }
                    }
                }
            }

            return list.OrderBy(f => f.TipoCosteCode).ThenBy(f => f.CentroCosteCode).ThenBy(f => f.CuentaCode)?.ToList();
        }

        private List<ControlPresupuestarioDto> GetN2(ControlPresupuestarioFilter filter)
        {
            List<ControlPresupuestarioDto> list = new List<ControlPresupuestarioDto>();

            string select = "SELECT A.SECUNDAR AS TipoCosteCode, C.NOMBRE AS DefinicionTipo, A.SECNIVEL2 AS " +
                " CentroCosteCode, D.NOMBRE AS DefinicionCentro, LEFT(A.CODCUEN, 3) AS cuenta, F.NOMBRE AS definicion, SUM(A.DEBE) AS debe, SUM(A.HABER) AS haber " +
                " FROM otrasien AS A LEFT OUTER JOIN " +
                " secundar AS D ON A.SECNIVEL2 = D.CODIGO LEFT OUTER JOIN " +
                " cuentas AS E ON A.CODCUEN = E.CODIGO INNER JOIN " +
                " secundar AS C ON A.SECUNDAR = C.CODIGO LEFT OUTER JOIN " +
                " nivel3 AS F ON LEFT(A.CODCUEN, 3) = F.CODIGO ";

            DbParameter parameter = null;

            List<int> ejercicios = GetEjercicios(filter.FechaDesde, filter.FechaHasta);

            foreach (int ejercicio in ejercicios)
            {
                SageContext _sageContext = _sageContextFactoryHelper.CreateSageContext(ejercicio);
                if (_sageContext == null)
                    continue;

                using (var command = _sageContext.Database.GetDbConnection().CreateCommand())
                {
                    List<string> listWhere = new List<string>();
                    listWhere.Add("A.EMPRESA IN ('01')");
                    listWhere.Add("LEFT(A.CODCUEN, 1) IN('6', '7')");
                    if (filter.FechaDesde.HasValue)
                    {
                        listWhere.Add("A.FECHA >= @FechaDesde");
                        parameter = command.CreateParameter();
                        parameter.ParameterName = "@FechaDesde";
                        parameter.Value = filter.FechaDesde.Value;
                        command.Parameters.Add(parameter);
                    }
                    if (filter.FechaHasta.HasValue)
                    {
                        listWhere.Add("A.FECHA <= @FechaHasta");
                        parameter = command.CreateParameter();
                        parameter.ParameterName = "@FechaHasta";
                        parameter.Value = filter.FechaHasta.Value;
                        command.Parameters.Add(parameter);
                    }
                    if (!string.IsNullOrWhiteSpace(filter.TipoCosteCode))
                    {
                        listWhere.Add("A.SECUNDAR = @TipoCosteCode");
                        parameter = command.CreateParameter();
                        parameter.ParameterName = "@TipoCosteCode";
                        parameter.Value = filter.TipoCosteCode;
                        command.Parameters.Add(parameter);
                    }
                    if (!string.IsNullOrWhiteSpace(filter.CentroCosteCode))
                    {
                        listWhere.Add("A.SECNIVEL2 = @CentroCosteCode");
                        parameter = command.CreateParameter();
                        parameter.ParameterName = "@CentroCosteCode";
                        parameter.Value = filter.CentroCosteCode;
                        command.Parameters.Add(parameter);
                    }
                    if (!string.IsNullOrWhiteSpace(filter.PrefijoCuenta))
                    {
                        listWhere.Add("LEFT(A.CODCUEN, 8) LIKE @PrefijoCuenta + '%'");
                        parameter = command.CreateParameter();
                        parameter.ParameterName = "@PrefijoCuenta";
                        parameter.Value = filter.PrefijoCuenta;
                        command.Parameters.Add(parameter);
                    }

                    string where = " WHERE " + String.Join(" AND ", listWhere);
                    string groupby = " GROUP BY LEFT(A.CODCUEN, 3), A.PLANCONT, A.SECUNDAR, C.NOMBRE, A.SECNIVEL2,D.NOMBRE, F.NOMBRE ";
                    string orderby = " ORDER BY TipoCosteCode, CentroCosteCode, cuenta";

                    string query = select + where + groupby + orderby;

                    command.CommandText = query;

                    _sageContext.Database.OpenConnection();
                    using (var result = command.ExecuteReader())
                    {
                        // do something with result
                        while (result.Read())
                        {
                            ControlPresupuestarioDto dto = new ControlPresupuestarioDto()
                            {
                                TipoCosteCode = result[0].ToString().Trim(),
                                TipoCosteNombre = result[1].ToString(),
                                CentroCosteCode = result[2].ToString().Trim(),
                                CentroCosteNombre = result[3].ToString(),
                                CuentaCode = result[4].ToString(),
                                CuentaNombre = result[5].ToString(),
                                Debe = Convert.ToDecimal(result[6]),
                                Haber = Convert.ToDecimal(result[7])
                            };

                            var item = list.FirstOrDefault(f =>
                                f.TipoCosteCode == dto.TipoCosteCode &&
                                f.CentroCosteCode == dto.CentroCosteCode &&
                                f.CuentaCode == dto.CuentaCode);

                            if (item == null)
                                list.Add(dto);
                            else
                            {
                                item.Debe += dto.Debe;
                                item.Haber += dto.Haber;
                            }

                        }
                    }
                }
            }

            list.AddRange(GetPartePersonal(filter));
            list.AddRange(GetParteAlmacen(filter));
            list.AddRange(GetParteE9(filter));

            return list.OrderBy(f => f.TipoCosteCode).ThenBy(f => f.CentroCosteCode).ThenBy(f => f.CuentaCode)?.ToList();
        }

        private List<ControlPresupuestarioDto> GetN3(ControlPresupuestarioFilter filter)
        {
            List<ControlPresupuestarioDto> list = new List<ControlPresupuestarioDto>();

            string select = "SELECT A.SECUNDAR AS TipoCosteCode, C.NOMBRE AS DefinicionTipo, A.SECNIVEL2 AS " +
                    " CentroCosteCode, D.NOMBRE AS DefinicionCentro, A.CODCUEN AS cuenta, E.NOMBRE AS definicion, SUM(A.DEBE) AS debe, SUM(A.HABER) AS haber " +
                    " FROM otrasien AS A LEFT OUTER JOIN " +
                    " plans AS B ON A.PLANCONT = B.CODIGO LEFT OUTER JOIN " +
                    " secundar AS C ON A.SECUNDAR = C.CODIGO LEFT OUTER JOIN " +
                    " secundar AS D ON A.SECNIVEL2 = D.CODIGO LEFT OUTER JOIN " +
                    " cuentas AS E ON A.CODCUEN = E.CODIGO ";

            DbParameter parameter = null;

            List<int> ejercicios = GetEjercicios(filter.FechaDesde, filter.FechaHasta);

            foreach (int ejercicio in ejercicios)
            {
                SageContext _sageContext = _sageContextFactoryHelper.CreateSageContext(ejercicio);
                if (_sageContext == null)
                    continue;

                using (var command = _sageContext.Database.GetDbConnection().CreateCommand())
                {
                    List<string> listWhere = new List<string>();
                    listWhere.Add("A.EMPRESA IN ('01')");
                    listWhere.Add("LEFT(A.CODCUEN, 1) IN('6', '7')");
                    if (filter.FechaDesde.HasValue)
                    {
                        listWhere.Add("A.FECHA >= @FechaDesde");
                        parameter = command.CreateParameter();
                        parameter.ParameterName = "@FechaDesde";
                        parameter.Value = filter.FechaDesde.Value;
                        command.Parameters.Add(parameter);
                    }
                    if (filter.FechaHasta.HasValue)
                    {
                        listWhere.Add("A.FECHA <= @FechaHasta");
                        parameter = command.CreateParameter();
                        parameter.ParameterName = "@FechaHasta";
                        parameter.Value = filter.FechaHasta.Value;
                        command.Parameters.Add(parameter);
                    }
                    if (!string.IsNullOrWhiteSpace(filter.TipoCosteCode))
                    {
                        listWhere.Add("A.SECUNDAR = @TipoCosteCode");
                        parameter = command.CreateParameter();
                        parameter.ParameterName = "@TipoCosteCode";
                        parameter.Value = filter.TipoCosteCode;
                        command.Parameters.Add(parameter);
                    }
                    if (!string.IsNullOrWhiteSpace(filter.CentroCosteCode))
                    {
                        listWhere.Add("A.SECNIVEL2 = @CentroCosteCode");
                        parameter = command.CreateParameter();
                        parameter.ParameterName = "@CentroCosteCode";
                        parameter.Value = filter.CentroCosteCode;
                        command.Parameters.Add(parameter);
                    }
                    if (!string.IsNullOrWhiteSpace(filter.PrefijoCuenta))
                    {
                        listWhere.Add("LEFT(A.CODCUEN, 8) LIKE @PrefijoCuenta + '%'");
                        parameter = command.CreateParameter();
                        parameter.ParameterName = "@PrefijoCuenta";
                        parameter.Value = filter.PrefijoCuenta;
                        command.Parameters.Add(parameter);
                    }

                    string where = " WHERE " + String.Join(" AND ", listWhere);
                    string groupby = " GROUP BY A.CODCUEN, A.PLANCONT, B.NOMBRE, A.SECUNDAR, C.NOMBRE, A.SECNIVEL2, D.NOMBRE, E.NOMBRE ";
                    string orderby = " ORDER BY TipoCosteCode, CentroCosteCode, cuenta";

                    string query = select + where + groupby + orderby;

                    command.CommandText = query;

                    _sageContext.Database.OpenConnection();
                    using (var result = command.ExecuteReader())
                    {
                        // do something with result
                        while (result.Read())
                        {
                            ControlPresupuestarioDto dto = new ControlPresupuestarioDto()
                            {
                                TipoCosteCode = result[0].ToString().Trim(),
                                TipoCosteNombre = result[1].ToString(),
                                CentroCosteCode = result[2].ToString().Trim(),
                                CentroCosteNombre = result[3].ToString(),
                                CuentaCode = result[4].ToString(),
                                CuentaNombre = result[5].ToString(),
                                Debe = Convert.ToDecimal(result[6]),
                                Haber = Convert.ToDecimal(result[7])
                            };

                            var item = list.FirstOrDefault(f =>
                                f.TipoCosteCode == dto.TipoCosteCode &&
                                f.CentroCosteCode == dto.CentroCosteCode &&
                                f.CuentaCode == dto.CuentaCode);

                            if (item == null)
                                list.Add(dto);
                            else
                            {
                                item.Debe += dto.Debe;
                                item.Haber += dto.Haber;
                            }
                        }
                    }
                }
            }

            list.AddRange(GetPartePersonal(filter));
            list.AddRange(GetParteAlmacen(filter));
            list.AddRange(GetParteE9(filter));

            return list.OrderBy(f => f.TipoCosteCode).ThenBy(f => f.CentroCosteCode).ThenBy(f => f.CuentaCode)?.ToList(); 
        }

        private List<ControlPresupuestarioDto> GetN4(ControlPresupuestarioFilter filter)
        {
            List<ControlPresupuestarioDto> list = new List<ControlPresupuestarioDto>();            

            string select = "SELECT A.SECUNDAR AS TipoCosteCode, C.NOMBRE AS DefinicionTipo, A.SECNIVEL2 AS " +
                    " CentroCosteCode, D.NOMBRE AS DefinicionCentro, A.CODCUEN AS cuenta, E.NOMBRE AS definicion, " +
                    " asientos.DEFINICION AS DefApunte, asientos.FECHA AS FechaApunte, A.DEBE AS debe, A.HABER AS haber, " +
                    " asientos.FACTURA AS Factura, asientos.PROVEEDOR AS CodigoProveedor " +
                    " FROM otrasien AS A INNER JOIN " +
                    " asientos ON A.ASI = asientos.ASI LEFT OUTER JOIN " +
                    " plans AS B ON A.PLANCONT = B.CODIGO LEFT OUTER JOIN " +
                    " secundar AS C ON A.SECUNDAR = C.CODIGO LEFT OUTER JOIN " +
                    " secundar AS D ON A.SECNIVEL2 = D.CODIGO LEFT OUTER JOIN " +
                    " cuentas AS E ON A.CODCUEN = E.CODIGO ";

            DbParameter parameter = null;

            List<int> ejercicios = GetEjercicios(filter.FechaDesde, filter.FechaHasta);

            foreach (int ejercicio in ejercicios)
            {
                SageContext _sageContext = _sageContextFactoryHelper.CreateSageContext(ejercicio);
                if (_sageContext == null)
                    continue;

                using (var command = _sageContext.Database.GetDbConnection().CreateCommand())
                {
                    List<string> listWhere = new List<string>();
                    listWhere.Add("A.EMPRESA IN ('01')");
                    listWhere.Add("LEFT(A.CODCUEN, 1) IN('6', '7')");
                    if (filter.FechaDesde.HasValue)
                    {
                        listWhere.Add("A.FECHA >= @FechaDesde");
                        parameter = command.CreateParameter();
                        parameter.ParameterName = "@FechaDesde";
                        parameter.Value = filter.FechaDesde.Value;
                        command.Parameters.Add(parameter);
                    }
                    if (filter.FechaHasta.HasValue)
                    {
                        listWhere.Add("A.FECHA <= @FechaHasta");
                        parameter = command.CreateParameter();
                        parameter.ParameterName = "@FechaHasta";
                        parameter.Value = filter.FechaHasta.Value;
                        command.Parameters.Add(parameter);
                    }
                    if (!string.IsNullOrWhiteSpace(filter.TipoCosteCode))
                    {
                        listWhere.Add("A.SECUNDAR = @TipoCosteCode");
                        parameter = command.CreateParameter();
                        parameter.ParameterName = "@TipoCosteCode";
                        parameter.Value = filter.TipoCosteCode;
                        command.Parameters.Add(parameter);
                    }
                    if (!string.IsNullOrWhiteSpace(filter.CentroCosteCode))
                    {
                        listWhere.Add("A.SECNIVEL2 = @CentroCosteCode");
                        parameter = command.CreateParameter();
                        parameter.ParameterName = "@CentroCosteCode";
                        parameter.Value = filter.CentroCosteCode;
                        command.Parameters.Add(parameter);
                    }
                    if (!string.IsNullOrWhiteSpace(filter.PrefijoCuenta))
                    {
                        listWhere.Add("LEFT(A.CODCUEN, 8) LIKE @PrefijoCuenta + '%'");
                        parameter = command.CreateParameter();
                        parameter.ParameterName = "@PrefijoCuenta";
                        parameter.Value = filter.PrefijoCuenta;
                        command.Parameters.Add(parameter);
                    }

                    string where = " WHERE " + String.Join(" AND ", listWhere);
                    string orderby = " ORDER BY TipoCosteCode, CentroCosteCode, cuenta";

                    string query = select + where + orderby;

                    command.CommandText = query;

                    _sageContext.Database.OpenConnection();
                    using (var result = command.ExecuteReader())
                    {
                        // do something with result
                        while (result.Read())
                        {
                            ControlPresupuestarioDto dto = new ControlPresupuestarioDto()
                            {
                                TipoCosteCode = result[0].ToString().Trim(),
                                TipoCosteNombre = result[1].ToString(),
                                CentroCosteCode = result[2].ToString().Trim(),
                                CentroCosteNombre = result[3].ToString(),
                                CuentaCode = result[4].ToString(),
                                CuentaNombre = result[5].ToString(),
                                DefApunte = result[6].ToString(),
                                FechaApunte = Convert.ToDateTime(result[7]),
                                Debe = Convert.ToDecimal(result[8]),
                                Haber = Convert.ToDecimal(result[9]),
                                Factura = result[10].ToString(),
                                CodigoProveedor = result[11].ToString()
                            };

                            if (!string.IsNullOrWhiteSpace(dto.Factura) && !string.IsNullOrWhiteSpace(dto.CodigoProveedor))
                            {
                                var factura = _dbContext.CompraFacturas.FirstOrDefault(f =>
                                    f.NumeroFactura.Trim() == dto.Factura.Trim() &&
                                    f.CodigoProveedor.Trim() == dto.CodigoProveedor.Trim());

                                if (factura != null && !string.IsNullOrWhiteSpace(factura.FicheroUrl))
                                {
                                    dto.UrlDocumento = factura.FicheroUrl;
                                    dto.NombreDocumento = factura.FicheroNombre;
                                }
                            }

                            list.Add(dto);
                        }
                    }
                }
            }

            list.AddRange(GetPartePersonal(filter));
            list.AddRange(GetParteAlmacen(filter));
            list.AddRange(GetParteE9(filter));

            return list.OrderBy(f => f.TipoCosteCode).ThenBy(f => f.CentroCosteCode).ThenBy(f => f.CuentaCode).ThenBy(f => f.FechaApunte)?.ToList();
        }

        private List<int> GetEjercicios(DateTime? desde, DateTime? hasta)
        {
            List<int> ejercicios = new List<int>();
            if (!desde.HasValue)
                throw new Exception("Es necesario un valor fecha desde.");

            if (desde.HasValue && hasta.HasValue && desde.Value > hasta.Value)
                return ejercicios;

            int yearHasta = hasta.HasValue ? hasta.Value.Year : DateTime.Now.Year;
            int yearDesde = desde.Value.Year;

            int i = yearDesde;
            while (i <= yearHasta)
            {
                ejercicios.Add(i);
                i++;
            }

            return ejercicios;
        }


        private List<ControlPresupuestarioDto> GetPartePersonal(ControlPresupuestarioFilter filter)
        {
            string cuentaNombre = "Personal de obra";
            List<ControlPresupuestarioDto> list = new List<ControlPresupuestarioDto>();

            var listPartesPersonal = _dbContext.PartePersonal
                .Include(f => f.CentroCoste).ThenInclude(f => f.TipoCentroCoste)
                .Where(f =>
                    (filter.FechaDesde.HasValue ? f.Fecha >= filter.FechaDesde : true) &&
                    (filter.FechaHasta.HasValue ? f.Fecha <= filter.FechaHasta : true)
                )
                .OrderBy(f => f.PersonalId);

            //var listPartesPersonal = _dbContext.PartePersonal
            //    .Include(f => f.CentroCoste).ThenInclude(f => f.TipoCentroCoste)
            //    .Where(f => 
            //        (filter.FechaDesde.HasValue ? f.Fecha >= filter.FechaDesde : true) &&
            //        (filter.FechaHasta.HasValue ? f.Fecha <= filter.FechaHasta : true) && 
            //        (string.IsNullOrWhiteSpace(filter.TipoCosteCode) ? true : f.CentroCoste.NivelAnalitico1.Trim() == filter.TipoCosteCode.Trim()) &&
            //        (string.IsNullOrWhiteSpace(filter.CentroCosteCode) ? true : f.CentroCoste.NivelAnalitico2.Trim() == filter.CentroCosteCode.Trim())
            //    )
            //    .OrderBy(f => f.PersonalId);

            foreach (var parte in listPartesPersonal)
            {
                //Parte
                ControlPresupuestarioDto dtoParte = new ControlPresupuestarioDto()
                {
                    CentroCosteCode = parte.CentroCoste.NivelAnalitico2?.Trim(),
                    CentroCosteNombre = parte.CentroCoste.Nombre,
                    TipoCosteCode = parte.CentroCoste.NivelAnalitico1?.Trim(),
                    TipoCosteNombre = parte.CentroCoste.TipoCentroCoste.Descripcion,
                    CuentaNombre = cuentaNombre,
                    Debe = parte.Importe
                };

                var existente = list.FirstOrDefault(f =>
                    f.TipoCosteCode == dtoParte.TipoCosteCode &&
                    f.CentroCosteCode == dtoParte.CentroCosteCode &&
                    f.CuentaNombre == dtoParte.CuentaNombre);

                if (existente == null)
                    list.Add(dtoParte);
                else
                    existente.Debe += dtoParte.Debe;

                //Personal
                Personal personal = _dbContext.Personal
                    .Include(f => f.CentroCoste).ThenInclude(f => f.TipoCentroCoste)
                    .SingleOrDefault(f => f.Id == parte.PersonalId);
                if (personal == null)
                    continue;

                ControlPresupuestarioDto dtoPersonal = new ControlPresupuestarioDto()
                {
                    CentroCosteCode = personal.CentroCoste.NivelAnalitico2?.Trim(),
                    CentroCosteNombre = personal.CentroCoste.Nombre,
                    TipoCosteCode = personal.CentroCoste.NivelAnalitico1?.Trim(),
                    TipoCosteNombre = personal.CentroCoste.TipoCentroCoste.Descripcion,
                    CuentaNombre = cuentaNombre,
                    Haber = parte.Importe
                };
                existente = list.FirstOrDefault(f =>
                    f.TipoCosteCode == dtoPersonal.TipoCosteCode &&
                    f.CentroCosteCode == dtoPersonal.CentroCosteCode &&
                    f.CuentaNombre == dtoPersonal.CuentaNombre);

                //Parte
                if (existente == null)
                    list.Add(dtoPersonal);
                else
                    existente.Haber += dtoPersonal.Haber;
            }

            if (!string.IsNullOrWhiteSpace(filter.TipoCosteCode))
                list.RemoveAll(f => f.TipoCosteCode.Trim() != filter.TipoCosteCode.Trim());
            if (!string.IsNullOrWhiteSpace(filter.CentroCosteCode))
                list.RemoveAll(f => f.CentroCosteCode.Trim() != filter.CentroCosteCode.Trim());

            return list;
        }

        private List<ControlPresupuestarioDto> GetParteAlmacen(ControlPresupuestarioFilter filter)
        {
            string cuentaNombre = "Almacén";
            List<ControlPresupuestarioDto> list = new List<ControlPresupuestarioDto>();

            var listAlmacen = _dbContext.PartesInternosAlmacen
                .Include(f => f.Destino).ThenInclude(f => f.TipoCentroCoste)
                .Where(f =>
                    (filter.FechaDesde.HasValue ? f.fecha >= filter.FechaDesde : true) &&
                    (filter.FechaHasta.HasValue ? f.fecha <= filter.FechaHasta : true)
                )
                .OrderBy(f => f.ArticulosAlmacenId);

            //var listAlmacen = _dbContext.PartesInternosAlmacen
            //    .Include(f => f.Destino).ThenInclude(f => f.TipoCentroCoste)
            //    .Where(f =>
            //        (filter.FechaDesde.HasValue ? f.fecha >= filter.FechaDesde : true) &&
            //        (filter.FechaHasta.HasValue ? f.fecha <= filter.FechaHasta : true) &&
            //        (string.IsNullOrWhiteSpace(filter.TipoCosteCode) ? true : f.Destino.NivelAnalitico1.Trim() == filter.TipoCosteCode.Trim()) &&
            //        (string.IsNullOrWhiteSpace(filter.CentroCosteCode) ? true : f.Destino.NivelAnalitico2.Trim() == filter.CentroCosteCode.Trim())
            //    )
            //    .OrderBy(f => f.ArticulosAlmacenId);

            foreach (var parte in listAlmacen)
            {
                //Parte
                ControlPresupuestarioDto dtoParte = new ControlPresupuestarioDto()
                {
                    CentroCosteCode = parte.Destino.NivelAnalitico2?.Trim(),
                    CentroCosteNombre = parte.Destino.Nombre,
                    TipoCosteCode = parte.Destino.NivelAnalitico1?.Trim(),
                    TipoCosteNombre = parte.Destino.TipoCentroCoste.Descripcion,
                    CuentaNombre = cuentaNombre,
                    Debe = parte.Amount
                };

                var existente = list.FirstOrDefault(f =>
                    f.TipoCosteCode == dtoParte.TipoCosteCode &&
                    f.CentroCosteCode == dtoParte.CentroCosteCode &&
                    f.CuentaNombre == dtoParte.CuentaNombre);

                if (existente == null)
                    list.Add(dtoParte);
                else
                    existente.Debe += dtoParte.Debe;

                //Almacén
                ArticulosAlmacen almacen = _dbContext.ArticulosAlmacen
                    .Include(f => f.CentroCoste).ThenInclude(f => f.TipoCentroCoste)
                    .SingleOrDefault(f => f.Id == parte.ArticulosAlmacenId); ;
                if (almacen == null)
                    continue;

                ControlPresupuestarioDto dtoAlmacen = new ControlPresupuestarioDto()
                {
                    CentroCosteCode = almacen.CentroCoste.NivelAnalitico2?.Trim(),
                    CentroCosteNombre = almacen.CentroCoste.Nombre,
                    TipoCosteCode = almacen.CentroCoste.NivelAnalitico1?.Trim(),
                    TipoCosteNombre = almacen.CentroCoste.TipoCentroCoste.Descripcion,
                    CuentaNombre = cuentaNombre,
                    Haber = parte.Amount
                };
                existente = list.FirstOrDefault(f =>
                    f.TipoCosteCode == dtoAlmacen.TipoCosteCode &&
                    f.CentroCosteCode == dtoAlmacen.CentroCosteCode &&
                    f.CuentaNombre == dtoAlmacen.CuentaNombre);

                //Parte
                if (existente == null)
                    list.Add(dtoAlmacen);
                else
                    existente.Haber += dtoParte.Haber;
            }

            if (!string.IsNullOrWhiteSpace(filter.TipoCosteCode))
                list.RemoveAll(f => f.TipoCosteCode.Trim() != filter.TipoCosteCode.Trim());
            if (!string.IsNullOrWhiteSpace(filter.CentroCosteCode))
                list.RemoveAll(f => f.CentroCosteCode.Trim() != filter.CentroCosteCode.Trim());
            return list;
        }

        private List<ControlPresupuestarioDto> GetParteE9(ControlPresupuestarioFilter filter)
        {
            string cuentaNombre = "E9";
            List<ControlPresupuestarioDto> list = new List<ControlPresupuestarioDto>();

            var listE9 = _dbContext.GrabacionE9
                .Include(f => f.CentroCoste).ThenInclude(f => f.TipoCentroCoste)
                .Include(f => f.Destino).ThenInclude(f => f.TipoCentroCoste)
                .Where(f =>
                    (filter.FechaDesde.HasValue ? f.Fecha >= filter.FechaDesde : true) &&
                    (filter.FechaHasta.HasValue ? f.Fecha <= filter.FechaHasta : true)
                )
                .OrderBy(f => f.CentroCosteId);

            //var listE9 = _dbContext.GrabacionE9
            //    .Include(f => f.CentroCoste).ThenInclude(f => f.TipoCentroCoste)
            //    .Include(f => f.Destino).ThenInclude(f => f.TipoCentroCoste)
            //    .Where(f =>
            //        (filter.FechaDesde.HasValue ? f.Fecha >= filter.FechaDesde : true) &&
            //        (filter.FechaHasta.HasValue ? f.Fecha <= filter.FechaHasta : true) &&
            //        (string.IsNullOrWhiteSpace(filter.TipoCosteCode) ? true :
            //            f.CentroCoste.NivelAnalitico1.Trim() == filter.TipoCosteCode.Trim() || f.Destino.NivelAnalitico1.Trim() == filter.TipoCosteCode.Trim()) &&
            //        (string.IsNullOrWhiteSpace(filter.CentroCosteCode) ? true :
            //            f.CentroCoste.NivelAnalitico2.Trim() == filter.CentroCosteCode.Trim() || f.Destino.NivelAnalitico2.Trim() == filter.CentroCosteCode.Trim())
            //    )
            //    .OrderBy(f => f.CentroCosteId);

            foreach (var parte in listE9)
            {
                //Parte gasto (Destino)
                ControlPresupuestarioDto dtoParteGasto = new ControlPresupuestarioDto()
                {
                    CentroCosteCode = parte.Destino.NivelAnalitico2?.Trim(),
                    CentroCosteNombre = parte.Destino.Nombre,
                    TipoCosteCode = parte.Destino.NivelAnalitico1?.Trim(),
                    TipoCosteNombre = parte.Destino.TipoCentroCoste.Descripcion,
                    CuentaNombre = cuentaNombre,
                    Debe = parte.Importe
                };

                var existente = list.FirstOrDefault(f =>
                    f.TipoCosteCode == dtoParteGasto.TipoCosteCode &&
                    f.CentroCosteCode == dtoParteGasto.CentroCosteCode &&
                    f.CuentaNombre == dtoParteGasto.CuentaNombre);

                if (existente == null)
                    list.Add(dtoParteGasto);
                else
                    existente.Debe += dtoParteGasto.Debe;

                //Parte ingreso (Centro de coste)
                ControlPresupuestarioDto dtoParteIngreso = new ControlPresupuestarioDto()
                {
                    CentroCosteCode = parte.CentroCoste.NivelAnalitico2?.Trim(),
                    CentroCosteNombre = parte.CentroCoste.Nombre,
                    TipoCosteCode = parte.CentroCoste.NivelAnalitico1?.Trim(),
                    TipoCosteNombre = parte.CentroCoste.TipoCentroCoste.Descripcion,
                    CuentaNombre = cuentaNombre,
                    Haber = parte.Importe
                };

                existente = list.FirstOrDefault(f =>
                    f.TipoCosteCode == dtoParteIngreso.TipoCosteCode &&
                    f.CentroCosteCode == dtoParteIngreso.CentroCosteCode &&
                    f.CuentaNombre == dtoParteIngreso.CuentaNombre);

                if (existente == null)
                    list.Add(dtoParteIngreso);
                else
                    existente.Haber += dtoParteIngreso.Haber;
            }

            if (!string.IsNullOrWhiteSpace(filter.TipoCosteCode))            
                list.RemoveAll(f => f.TipoCosteCode.Trim() != filter.TipoCosteCode.Trim());
            if (!string.IsNullOrWhiteSpace(filter.CentroCosteCode))
                list.RemoveAll(f => f.CentroCosteCode.Trim() != filter.CentroCosteCode.Trim());

            return list;
        }

    }
}
