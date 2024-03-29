﻿using LdapForNet.Native;

namespace SysAdmin.ActiveDirectory.Services.Ldap
{
    public class Server : IServer
    {

        public Server() { }

        public Server(string server)
        {
            ServerName = server;
        }

        public Server(string server, int port)
        {
            ServerName = server;
            Port = port;
        }

        public string ServerName { get; set; } = string.Empty;
        public int Port { get; set; } = 389;
        public bool IsSSL { get; set; } = false;
    }
}