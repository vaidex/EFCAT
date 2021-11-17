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

        public ForeignColumn(ForeignType type = ForeignType.ONE_TO_ONE, string keys = "", DeleteBehavior onDelete = DeleteBehavior.Cascade) {
            this.type = type;
            this.keys = (keys != "" ? keys.Replace(" ", "").Split(",") : new string[0]);
            this.onDelete = onDelete;
        }


    }

    public enum ForeignType { ONE_TO_ONE, MANY_TO_ONE }
}

