using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFCAT.Model.Annotation.Util {
    public class SqlDoubleRangeAttribute : SqlAttribute {
        double min = 0;
        public bool IsMinNull { get; private set; } = true;
        public double Min {
            get => min;
            set {
                IsMinNull = false;
                min = value;
            }
        }

        double max = 0;
        public bool IsMaxNull { get; private set; } = true;
        public double Max {
            get => max;
            set {
                IsMaxNull = false;
                max = value;
            }
        }

        public SqlDoubleRangeAttribute(string type, string size = "") : base(type, size) { }
        public SqlDoubleRangeAttribute(string type, int size = 0) : base(type, size) { }

        protected override ValidationResult? IsValid(object? value, ValidationContext context) {
            if (ErrorMessage == null) ErrorMessage = $"The field {context.DisplayName} needs to be in the range of {Min} and {Max}.";
            ErrorMessage = ErrorMessage.Replace("@min", $"{Min}").Replace("@max", $"{Max}");
            if (value == null) throw new ArgumentNullException();
            double v = Convert.ToDouble(value);
            return ((IsMinNull ? true : v >= Min) && (IsMaxNull ? true : v <= Max)) ? ValidationResult.Success : new ValidationResult(ErrorMessage);
        }

    }
}
