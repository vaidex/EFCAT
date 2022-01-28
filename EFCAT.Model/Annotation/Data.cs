using EFCAT.Model.Data.Annotation;
using EFCAT.Model.Data.Validation;
using System.ComponentModel.DataAnnotations;

namespace EFCAT.Model.Annotation;

// STRING DATA TYPES

public class CharAttribute : StringAttribute {
    public CharAttribute(int size = 1) : base("char", size) {
        if (size > 255) MaxSizeReached("255");
    }
}

public class VarcharAttribute : StringAttribute {
    public VarcharAttribute(int size = 32) : base("varchar", size) {
        if (size > 65535) MaxSizeReached("65535");
    }
}

public class BinaryAttribute : StringAttribute {
    public BinaryAttribute(int size = 1) : base("binary", size) {
        if (size > 255) MaxSizeReached("255");
    }
}

public class VarbinaryAttribute : StringAttribute {
    public VarbinaryAttribute(int size = 32) : base("varbinary", size) {
        if (size > 65535) MaxSizeReached("65535");
    }
}

public class TinyblobAttribute : StringAttribute {
    public TinyblobAttribute(int size = 32) : base("tinyblob", size) {
        if (size > 255) MaxSizeReached("255");
    }
}

public class TinytextAttribute : StringAttribute {
    public TinytextAttribute(int size = 32) : base("tinytext", size) {
        if (size > 255) MaxSizeReached("255");
    }
}

public class TextAttribute : StringAttribute {
    public TextAttribute(int size = 32) : base("text", size) {
        if (size > 65535) MaxSizeReached("65535");
    }
}

public class BlobAttribute : StringAttribute {
    public BlobAttribute(int size = 32) : base("blob", size) {
        if (size > 65535) MaxSizeReached("65535");
    }
}

public class MediumtextAttribute : StringAttribute {
    public MediumtextAttribute(int size = 32) : base("mediumtext", size) {
        if (size > 16777215) MaxSizeReached("16777215");
    }
}

public class MediumblobAttribute : StringAttribute {
    public MediumblobAttribute(int size = 32) : base("mediumblob", size) {
        if (size > 16777215) MaxSizeReached("16777215");
    }
}

public class LongtextAttribute : StringAttribute {
    public LongtextAttribute(long size = 32) : base("longtext", size) {
        if (size > 4294967295) MaxSizeReached("4294967295");
    }
}

public class LongblobAttribute : StringAttribute {
    public LongblobAttribute(long size = 32) : base("longblob", size) {
        if (size > 4294967295) MaxSizeReached("4294967295");
    }
}

// NUMERIC DATA TYPES

public class BitAttribute : Int64Attribute {
    public BitAttribute(int size = 32) : base("bit", size) {
        if (size < 0) MaxSizeReached("0");
        if (size > 64) MaxSizeReached("64");
    }
}

public class TinyintAttribute : Int64Attribute {
    public TinyintAttribute(int size = 32) : base("tinyint", size) {
        if (size < -128) MaxSizeReached("-128");
        if (size > 127) MaxSizeReached("127");
    }
}

public class BoolAttribute : TypeAttribute {
    public BoolAttribute() : base("bit", "") { }

    public override bool Nullable { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
}

public class BooleanAttribute : BoolAttribute {
    public BooleanAttribute() { }
}

public class SmallintAttribute : Int32Attribute {
    public SmallintAttribute(int size = 32) : base("smallint", size) {
        if (size < -32768) MaxSizeReached("-32768");
        if (size > 32767) MaxSizeReached("32767");
    }
}

public class MediumintAttribute : Int32Attribute {
    public MediumintAttribute(int size = 32) : base("mediumint", size) {
        if (size < -8388608) MaxSizeReached("-8388608");
        if (size > 8388607) MaxSizeReached("8388607");
    }
}

public class IntAttribute : Int32Attribute {
    public IntAttribute(int size = 32) : base("int", size) {
        if (size < -2147483648) MaxSizeReached("-2147483648 ");
        if (size > 2147483647) MaxSizeReached("2147483647");
    }
}

public class IntegerAttribute : IntAttribute {
    public IntegerAttribute(int size = 32) : base(size) { }
}

public class BigintAttribute : Int64Attribute {
    public BigintAttribute(long size = 32) : base("bigint", size) {
        if (size < -9223372036854775808) MaxSizeReached("-9223372036854775808");
        if (size > 9223372036854775807) MaxSizeReached("9223372036854775807");
    }
}

public class NumberAttribute : PrecisionAttribute {
    private DoubleValidation validation = new DoubleValidation();

    public double Min { get => validation.Min; set => validation.Min = value; }
    public double Max { get => validation.Max; set => validation.Max = value; }
    public override bool Nullable { get => validation.Nullable; set => validation.Nullable = value; }

    public string? ErrorMessage { get => validation.ErrorMessage; set => validation.ErrorMessage = value; }

    public NumberAttribute(int digits = 3, int decimals = 2) : base(digits, decimals) { }

    protected override ValidationResult? IsValid(object? value, ValidationContext context) => validation.IsValid(value, context);
}

public class NaturalAttribute : PrecisionAttribute {
    private FloatValidation validation = new FloatValidation();

    public float Min { get => validation.Min; set => validation.Min = value; }
    public float Max { get => validation.Max; set => validation.Max = value; }
    public override bool Nullable { get => validation.Nullable; set => validation.Nullable = value; }

    public string? ErrorMessage { get => validation.ErrorMessage; set => validation.ErrorMessage = value; }

    public NaturalAttribute(int digits = 3, int decimals = 2) : base(digits, decimals) { }

    protected override ValidationResult? IsValid(object? value, ValidationContext context) => validation.IsValid(value, context);
}

public abstract class PrecisionAttribute : TypeAttribute {
    public int Digits { get; set; }
    public int Decimals { get; set; }
    public override bool Nullable { get; set; }

    public PrecisionAttribute(int digits, int decimals) : base("", $"({digits},{decimals})") {
        Digits = digits;
        Decimals = decimals;
    }
}