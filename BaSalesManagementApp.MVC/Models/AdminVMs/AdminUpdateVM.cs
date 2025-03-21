﻿namespace BaSalesManagementApp.MVC.Models.AdminVMs
{
    public class AdminUpdateVM
    {
        public Guid Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public IFormFile? Photo { get; set; }
        public byte[]? PhotoData { get; set; }
        public string? PhotoUrl { get; set; }
        public bool IsPhotoRemoved { get; set; }  // Fotoğrafı kaldırma isteği
    }
}
