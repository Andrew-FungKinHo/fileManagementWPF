using System.Collections.Generic;
using System.IO.Compression;

namespace windowsApp.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string EmailAddress { get; set; } = null!;

        public string Phone { get; set; } = null!;
        public AppFile? FileAssigned { get; set; }
    }
}