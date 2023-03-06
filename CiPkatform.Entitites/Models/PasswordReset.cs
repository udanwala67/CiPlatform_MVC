using System;
using System.Collections.Generic;

namespace CiPlatform.Entitites.Models
{
    public partial class PasswordReset
    {
        public string Email { get; set; } = null!;
        public string? Token { get; set; }
        public DateTime? CreatedAt { get; set; }
        public long Id { get; set; }
    }
}
