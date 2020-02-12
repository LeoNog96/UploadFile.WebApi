using System;
using System.Collections.Generic;

namespace UploadFile.WebApi.Models
{
    public class User
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool? Removed { get; set; }
        
        // classe "virtual" que quando usado include na query retorna todas os documentos
        // que estao relacionados com o usuario
        public virtual ICollection<Document> Documents { get; set; }
    }
}