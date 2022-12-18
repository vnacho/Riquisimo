namespace Ferpuser.Models.Dtos
{
    public class SerieDto
    {
        public string Codigo { get; set; }
        public string Nombre { get; set; }

        public string Display => string.IsNullOrWhiteSpace(Nombre) ? Codigo : $"{Codigo} - {Nombre}";
    }
}
