﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CrossExchange
{
    public class Portfolio
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
        
        public List<Trade> Trade { get; set; }

    }
}
