using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.CrossCuttingConcerns.Logging.Models
{
    public class LogDetailWithException:LogDetail
    {
        public string TraceIdentifier { get; set; }
        public string ExceptionMessage { get; set; }
    }
}
