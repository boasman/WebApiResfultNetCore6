namespace WebApiAutores.Servicios
{
    public class EscribirEnArchivo : IHostedService
    {
        private readonly IWebHostEnvironment env;
        private readonly string nombreArchivo = "archivo.txt";
        private Timer timer;
        public EscribirEnArchivo(IWebHostEnvironment env)
        {
            this.env = env;
        }
        public Task StartAsync(CancellationToken cancellationToken)
        {
            timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromSeconds(5));
            Escribir("Proces Iniciado");
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            timer.Dispose();            
            Escribir("Proces Finalizado");
            return Task.CompletedTask;
        }

        private void Escribir(string mensaje)
        {

            var ruta = $@"{env.ContentRootPath}\wwwroot\{nombreArchivo}";

            using StreamWriter sw = new StreamWriter(ruta, append: true);
            sw.WriteLine(mensaje);
        }

        private void DoWork(object state)
        {
            Escribir("Proceso en ejecucion" + DateTime.Now.ToString("dd/MM/yy hh:mm:ss"));
        }
    }
}
