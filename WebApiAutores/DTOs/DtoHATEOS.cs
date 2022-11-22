namespace WebApiAutores.DTOs
{
    public class DtoHATEOS
    {

        public string Enlace { get; set; }
        public string  Description { get; set; }
        public string  Metodo { get; set; }

        public DtoHATEOS(string enlace, string description, string metodo)
        {
            Enlace = enlace;
            Description = description;
            Metodo = metodo;
        }
    }
}
