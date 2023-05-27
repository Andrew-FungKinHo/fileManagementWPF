using System.Collections.Generic;
using System;

namespace windowsApp.Models
{
    public class AppFile
    {
        public int Id { get; set; }
        public string FileName { get; set; } = null!;
        public string FileExtension { get; set; } = null!;
        public DateTime FileCreatedTime { get; set; }
        public DateTime FileLastModifiedTime { get; set; }
        public List<User>? UsersAssigned { get; set; }
    }
}