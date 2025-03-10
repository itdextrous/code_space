using System;
using System.ComponentModel;
using System.Globalization;
using SAASCLOUDAPP.API.Models;

namespace SAASCLOUDAPP.API.TypeConverters
{
    public class FiscalQuarterDtoConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType) =>
            sourceType == typeof(string);

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value) =>
            FiscalQuarterDto.Parse((string)value);
    }
}