using Ferpuser.Data;
using Ferpuser.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Ferpuser.BLL.BusinessValidators
{
    public class CategoriaInscritoValidator
    {
        public ApplicationDbContext _db { get; set; }

        public CategoriaInscritoValidator(ApplicationDbContext dbContext)
        {
            _db = dbContext;
        }

        public IEnumerable<ValidationResult> Create(CategoriaInscrito model)
        {
            List<ValidationResult> list = new List<ValidationResult>();
            if (_db.CategoriasInscritos.Any(f => f.Id.Trim() == model.Id.Trim()))
                list.Add(new ValidationResult("Ya existe una categoría con ese Id.", new string[] { nameof(CategoriaInscrito.Id) }));
            if (_db.CategoriasInscritos.Any(f => f.Nombre.Trim() == model.Nombre.Trim()))
                list.Add(new ValidationResult("Ya existe una categoría con ese nombre.", new string[] { nameof(CategoriaInscrito.Nombre) }));
            return list;
        }

        public IEnumerable<ValidationResult> Edit(CategoriaInscrito model)
        {
            List<ValidationResult> list = new List<ValidationResult>();
            if (_db.CategoriasInscritos.Any(f => f.Id != model.Id && f.Nombre.Trim() == model.Nombre.Trim()))
                list.Add(new ValidationResult("Ya existe una categoría con ese nombre.", new string[] { nameof(CategoriaInscrito.Nombre) }));
            return list;
        }

        public IEnumerable<ValidationResult> Delete(object id)
        {
            List<ValidationResult> list = new List<ValidationResult>();
            return list;
        }
    }
}