﻿


namespace CesarBmx.Shared.Settings
{
    public  class AuthenticationSettings
    {
        public bool Enabled { get; set; }
        public string AuthenticationType { get; set; }
        public string TestUser { get; set; }
        public string ApiKey { get; set; }
        public string Secret { get; set; }
        public string Issuer { get; set; }
    }
}
