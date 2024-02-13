namespace TP10.Servicios
{
    public class ServicioRol: IServicioRol
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ServicioRol(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public bool IsAdmin()
        {
            var session = _httpContextAccessor.HttpContext?.Session;
            return session != null && session.GetString("Rol") == "Admin";
        }
    }
}
