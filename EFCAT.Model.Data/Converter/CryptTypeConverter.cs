using System.ComponentModel;
using System.Globalization;

namespace EFCAT.Model.Data.Converter;

public sealed class CryptTypeConverter<TAlgorithm> : TypeConverter where TAlgorithm : IAlgorithm, new() {
    public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType) {
        if (sourceType == typeof(string)) return true;
        return base.CanConvertFrom(context, sourceType);
    }

    public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value) {
        if (value != null)
            if (value is string str) return new Crypt<TAlgorithm>(str, isCrypted: false);
        return base.ConvertFrom(context, culture, value);
    }

    public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType) {
        if (destinationType == typeof(string)) return true;
        return base.CanConvertTo(context, destinationType);
    }

    public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType) {
        if (value is Crypt<TAlgorithm> crypt)
            if(destinationType == typeof(string)) return crypt.ToString();
        return base.ConvertTo(context, culture, value, destinationType);
    }
}

public class CryptTypeDescriptor : CustomTypeDescriptor {
    private Type objectType;

    public CryptTypeDescriptor(Type objectType) {
        this.objectType = objectType;
    }

    public override TypeConverter GetConverter() {
        var genericArg = objectType.GenericTypeArguments[0];
        var converterType = typeof(CryptTypeConverter<>).MakeGenericType(genericArg);
        return (TypeConverter)Activator.CreateInstance(converterType);
    }
}

public class CryptTypeDescriptionProvider : TypeDescriptionProvider {
    public override ICustomTypeDescriptor GetTypeDescriptor(Type objectType, object instance) {
        return new CryptTypeDescriptor(objectType);
    }
}