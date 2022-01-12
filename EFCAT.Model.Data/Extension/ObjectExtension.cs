using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFCAT.Model.Data.Extension {
    public static class ObjectExtension {
        public static bool IfNotNull<T>(this T obj, Predicate<T> predicate, Action? actionIfNull = null) {
            if (obj != null) return predicate(obj);
            return false;
        }
    }
}
