using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFCAT.Annotation {
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class Unique : ValidationAttribute {
        public Unique() {

        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext) {
            Console.WriteLine(validationContext.ToString());
            Console.WriteLine(value.ToString());
            return ValidationResult.Success;
        }
    }
}
