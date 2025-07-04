﻿using Dynamiq.API.Extension.Enums;
using Microsoft.EntityFrameworkCore;

namespace Dynamiq.API.DAL.Models
{
    [Index(nameof(Email), IsUnique = true)]
    public class User
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public RoleEnum Role { get; set; }
        public bool ConfirmedEmail { get; set; }
        public RefreshToken RefreshToken { get; set; } = null!;
        public ICollection<PaymentHistory> PaymentHistories { get; set; }
    }
}
