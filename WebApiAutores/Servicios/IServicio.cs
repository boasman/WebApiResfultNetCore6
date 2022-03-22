namespace WebApiAutores.Servicios
{
    public interface IServicio
    {
        Guid ObtenerScoped();
        Guid ObtenerSingleton();
        Guid ObtenerTransient();
        void RealizarTarea();
    }

    public class ServicioA : IServicio
    {
        private readonly ILogger<ServicioA> logger;
        private readonly ServicioTransient servicioTransient;
        private readonly ServicioScoped servicioScoped;
        private readonly ServicioSinleton servicioSinleton;

        public ServicioA(ILogger<ServicioA> logger, ServicioTransient servicioTransient, ServicioScoped servicioScoped,
            ServicioSinleton servicioSinleton)
        {            
            this.logger = logger;
            this.servicioTransient = servicioTransient;
            this.servicioScoped = servicioScoped;
            this.servicioSinleton = servicioSinleton;
        }


        public Guid ObtenerTransient() { return servicioTransient.Guid; }
        public Guid ObtenerScoped() { return servicioScoped.Guid; }
        public Guid ObtenerSingleton() { return servicioSinleton.Guid; }
        

        public void RealizarTarea()
        {
           
        }
    }
    public class ServicioB : IServicio
    {
        public Guid ObtenerScoped()
        {
            throw new NotImplementedException();
        }

        public Guid ObtenerSingleton()
        {
            throw new NotImplementedException();
        }

        public Guid ObtenerTransient()
        {
            throw new NotImplementedException();
        }

        public void RealizarTarea()
        {
           
        }
    }

    public class ServicioTransient
    {
        public Guid Guid = Guid.NewGuid();  
    }

    public class ServicioScoped
    {
        public Guid Guid = Guid.NewGuid();
    }

    public class ServicioSinleton
    {
        public Guid Guid = Guid.NewGuid();
    }
}
