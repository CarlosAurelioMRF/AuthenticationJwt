using AuthenticationJwt.Core.Base.Interface;

namespace AuthenticationJwt.Core.Base
{
    public class DatabaseSettings : IDatabaseSettings
    {
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }
}
