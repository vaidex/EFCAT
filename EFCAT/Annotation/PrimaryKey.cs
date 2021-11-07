using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFCAT.Annotation {
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class PrimaryKey : ValidationAttribute {

    }
}
