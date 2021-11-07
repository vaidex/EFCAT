using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFCAT.Annotation {
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class ForeignColumn : Attribute {
        public readonly ForeignType type;
        public readonly string[] keys;
        public readonly DeleteBehavior onDelete;

        public ForeignColumn([NotNullAttribute] ForeignType type, string key = "", DeleteBehavior onDelete = DeleteBehavior.Cascade) {
            this.type = type;
            this.keys = key != "" ? key.Replace(" ", "").Split(",") : null;
            this.onDelete = onDelete;
        }

        
    }

    public enum ForeignType { ONE_TO_ONE, MANY_TO_ONE }
}
