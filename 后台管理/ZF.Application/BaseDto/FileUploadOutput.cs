using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZF.Application.BaseDto
{
   public class FileUploadOutput
    {
        public bool Success { get; set; }

        public List<string> fileUploadPath { get; set; }

        public string Message { get; set; }
    }
}
