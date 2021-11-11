using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFCAT.Configuration {
    internal class Key {
        public string Name { get; set; }
        public Type Type { get; set; }
        public Key(string name, Type type) {
            Name = name;
            Type = type;
        }
    }
}
