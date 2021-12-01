using EFCAT.Model.Annotation.Util;

namespace EFCAT.Model.Annotation;

// STRING DATA TYPES

public class CharAttribute : SqlStringAttribute {
    public CharAttribute(int size = 1) : base("char", size) {
        if (size > 255) MaxSizeReached("255");
    }
}

public class VarcharAttribute : SqlStringAttribute {
    public VarcharAttribute(int size = 32) : base("varchar", size) {
        if (size > 65535) MaxSizeReached("65535");
    }
}

public class BinaryAttribute : SqlStringAttribute {
    public BinaryAttribute(int size = 1) : base("binary", size) {
        if (size > 255) MaxSizeReached("255");
    }
}

public class VarbinaryAttribute : SqlStringAttribute {
    public VarbinaryAttribute(int size = 32) : base("varbinary", size) {
        if (size > 65535) MaxSizeReached("65535");
    }
}

public class TinyblobAttribute : SqlStringAttribute {
    public TinyblobAttribute(int size = 32) : base("tinyblob", size) {
        if (size > 255) MaxSizeReached("255");
    }
}

public class TinytextAttribute : SqlStringAttribute {
    public TinytextAttribute(int size = 32) : base("tinytext", size) {
        if (size > 255) MaxSizeReached("255");
    }
}

public class TextAttribute : SqlStringAttribute {
    public TextAttribute(int size = 32) : base("text", size) {
        if (size > 65535) MaxSizeReached("65535");
    }
}

public class BlobAttribute : SqlStringAttribute {
    public BlobAttribute(int size = 32) : base("blob", size) {
        if (size > 65535) MaxSizeReached("65535");
    }
}

public class MediumtextAttribute : SqlStringAttribute {
    public MediumtextAttribute(int size = 32) : base("mediumtext", size) {
        if (size > 16777215) MaxSizeReached("16777215");
    }
}

public class MediumblobAttribute : SqlStringAttribute {
    public MediumblobAttribute(int size = 32) : base("mediumblob", size) {
        if (size > 16777215) MaxSizeReached("16777215");
    }
}

public class LongtextAttribute : SqlStringAttribute {
    public LongtextAttribute(long size = 32) : base("longtext", size) {
        if (size > 4294967295) MaxSizeReached("4294967295");
    }
}

public class LongblobAttribute : SqlStringAttribute {
    public LongblobAttribute(long size = 32) : base("longblob", size) {
        if (size > 4294967295) MaxSizeReached("4294967295");
    }
}

// NUMERIC DATA TYPES

public class BitAttribute : SqlIntRangeAttribute {
    public BitAttribute(int size = 32) : base("bit", size) {
        if (size < 0) MaxSizeReached("0");
        if (size > 64) MaxSizeReached("64");
    }
}

public class TinyintAttribute : SqlIntRangeAttribute {
    public TinyintAttribute(int size = 32) : base("tinyint", size) {
        if (size < -128) MaxSizeReached("-128");
        if (size > 127) MaxSizeReached("127");
    }
}

public class BoolAttribute : SqlAttribute {
    public BoolAttribute() : base("bool", "") { }
}

public class BooleanAttribute : SqlAttribute {
    public BooleanAttribute() : base("bool", "") { }
}

public class SmallintAttribute : SqlIntRangeAttribute {
    public SmallintAttribute(int size = 32) : base("smallint", size) {
        if (size < -32768) MaxSizeReached("-32768");
        if (size > 32767) MaxSizeReached("32767");
    }
}

public class MediumintAttribute : SqlIntRangeAttribute {
    public MediumintAttribute(int size = 32) : base("mediumint", size) {
        if (size < -8388608) MaxSizeReached("-8388608");
        if (size > 8388607) MaxSizeReached("8388607");
    }
}

public class IntAttribute : SqlIntRangeAttribute {
    public IntAttribute(int size = 32) : base("int", size) {
        if (size < -2147483648) MaxSizeReached("-2147483648 ");
        if (size > 2147483647) MaxSizeReached("2147483647");
    }
}

public class IntegerAttribute : IntAttribute {
    public IntegerAttribute(int size = 32) : base(size) { }
}

public class BigintAttribute : SqlLongRangeAttribute {
    public BigintAttribute(long size = 32) : base("bigint", size) {
        if (size < -9223372036854775808) MaxSizeReached("-9223372036854775808");
        if (size > 9223372036854775807) MaxSizeReached("9223372036854775807");
    }
}

public class FloatAttribute : SqlDoubleRangeAttribute {
    public FloatAttribute(int digits = 3, int decimals = 2) : base("float", $"{digits + decimals},{decimals}") { Max = (Math.Pow(10, digits) - 1) + ((Math.Pow(10, decimals) - 1) / Math.Pow(10, decimals)); }
}

public class DoubleAttribute : SqlDoubleRangeAttribute {
    public DoubleAttribute(int digits = 3, int decimals = 2) : base("double", $"{digits + decimals},{decimals}") { Max = (Math.Pow(10, digits) - 1) + ((Math.Pow(10, decimals) - 1) / Math.Pow(10, decimals)); }
}

public class DecimalAttribute : SqlDoubleRangeAttribute {
    public DecimalAttribute(int digits = 3, int decimals = 2) : base("decimal", $"{digits + decimals},{decimals}") { Max = (Math.Pow(10, digits) - 1) + ((Math.Pow(10, decimals) - 1) / Math.Pow(10, decimals)); }
}

