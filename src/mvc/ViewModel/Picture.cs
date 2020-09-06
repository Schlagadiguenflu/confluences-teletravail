using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace mvc.ViewModel
{
    public class Picture
    {
        public List<IFormFile> Pictures { get; set; }
    }
}
