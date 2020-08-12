using System;

namespace ShadAhm.Katalalu.App.Models
{
    public class PasswordItem
    {
        public string Service { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public bool IsEncrypted { get; set; }
    }
}