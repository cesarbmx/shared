﻿


namespace CesarBmx.Shared.Settings
{
    public  class RabbitMqSettings
    {
        public string Host { get; set; }
        public string VirtualHost { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public ushort Port { get; set; }
    }
}
