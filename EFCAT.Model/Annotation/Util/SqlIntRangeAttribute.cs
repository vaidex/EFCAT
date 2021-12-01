using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFCAT.Model.Annotation.Util {
    public class SqlIntRangeAttribute : SqlAttribute {
        int min = 0;
        public bool IsMinNull { get; private set; } = true;
        public int Min {
            get => min;
            set {
                IsMinNull = false;
                min = value;
            }
        }

        int max = 0;
        public bool IsMaxNull { get; private set; } = true;
        public int Max {
            get => max;
            set {
                IsMaxNull = false;
                max = value;
            }
        }

        public SqlIntRangeAttribute(string type, string size = "") : base(type, size) { }
        public SqlIntRangeAttribute(string type, int size = 0) : base(type, size) { }

        protected override ValidationResult? IsValid(object? value, ValidationContext context) {
            if (ErrorMessage == null) ErrorMessage = $"The field {context.DisplayName} needs to be in the range of {Min} and {Max}.";
            ErrorMessage = ErrorMessage.Replace("@min", $"{Min}").Replace("@max", $"{Max}");
            if (value == null) throw new ArgumentNullException();
            int v = Convert.ToInt32(value);
            return ((IsMinNull ? true : v >= Min) && (IsMaxNull ? true : v <= Max)) ? ValidationResult.Success : new ValidationResult(ErrorMessage);
        }
    }
}
