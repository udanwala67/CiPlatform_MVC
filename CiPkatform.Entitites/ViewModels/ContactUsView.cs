using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CiPlatform.Entitites.ViewModels
{
    public class ContactUsView
    {           
            public long UserId { get; set; }
            public string? Name { get; set; } = string.Empty;
            public string? Email { get; set; } = string.Empty;

            [Required(ErrorMessage = "Subject is required")]
            public string Subject { get; set; } = string.Empty;

            [Required(ErrorMessage = "Message is required")]
            public string Message { get; set; } = string.Empty;
        
    }
}

