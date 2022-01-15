using EFCAT.Model.Data;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Globalization;
using System.Reflection;

namespace EFCAT.Model.Data.Converter;

public sealed class CryptTypeConverter<TAlgorithm> : TypeConverter where TAlgorithm : IAlgorithm, new() {
    public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType) {
        if (sourceType == typeof(string)) return true;
        return base.CanConvertFrom(context, sourceType);
    }

    public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value) {
        string stringValue = value as string;
        if (stringValue != null) return new Crypt<TAlgorithm>(stringValue, isCrypted: false);
        return base.ConvertFrom(context, culture, value);
    }

    public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType) {
        if (destinationType == typeof(InstanceDescriptor)) return true;
        return base.CanConvertTo(context, destinationType);
    }

    public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType) {
        if (destinationType == typeof(InstanceDescriptor) && value is Crypt<TAlgorithm>) {
            Crypt<TAlgorithm> obj = value as Crypt<TAlgorithm>;

            ConstructorInfo ctor = typeof(Crypt<TAlgorithm>).GetConstructor(new Type[] { typeof(string) });

            if (ctor != null) return new InstanceDescriptor(ctor, new object[] { obj.ToString() });
        }
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