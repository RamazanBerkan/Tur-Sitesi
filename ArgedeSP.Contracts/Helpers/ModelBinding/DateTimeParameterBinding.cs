using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArgedeSP.Contracts.Helpers.ModelBinding
{
    public class DateTimeModelBinder : IModelBinder
    {
        public static readonly Type[] SUPPORTED_TYPES = new Type[] { typeof(DateTime), typeof(DateTime?) };

        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext == null)
            {
                throw new ArgumentNullException(nameof(bindingContext));
            }

            if (!SUPPORTED_TYPES.Contains(bindingContext.ModelType))
            {
                return Task.CompletedTask;
            }

            var modelName = GetModelName(bindingContext);

            var valueProviderResult = bindingContext.ValueProvider.GetValue(modelName);
            if (valueProviderResult == ValueProviderResult.None)
            {
                return Task.CompletedTask;
            }

            bindingContext.ModelState.SetModelValue(modelName, valueProviderResult);

            var dateToParse = valueProviderResult.FirstValue;

            if (string.IsNullOrEmpty(dateToParse))
            {
                return Task.CompletedTask;
            }

            var dateTime = ParseDate(bindingContext, dateToParse);

            bindingContext.Result = ModelBindingResult.Success(dateTime);

            return Task.CompletedTask;
        }

        private DateTime? ParseDate(ModelBindingContext bindingContext, string dateToParse)
        {
            var attribute = GetDateTimeModelBinderAttribute(bindingContext);
            var dateFormat = attribute?.DateFormat;

            if (string.IsNullOrEmpty(dateFormat))
            {
                return Global.ParseDateTime(dateToParse);
            }

            return Global.ParseDateTime(dateToParse, new string[] { dateFormat });
        }

        private DateTimeModelBinderAttribute GetDateTimeModelBinderAttribute(ModelBindingContext bindingContext)
        {
            var modelName = GetModelName(bindingContext);

            var paramDescriptor = bindingContext.ActionContext.ActionDescriptor.Parameters
                .Where(x => x.ParameterType == typeof(DateTime?))
                .Where((x) =>
                {
                    // See comment in GetModelName() on why we do this.
                    var paramModelName = x.BindingInfo?.BinderModelName ?? x.Name;
                    return paramModelName.Equals(modelName);
                })
                .FirstOrDefault();

            var ctrlParamDescriptor = paramDescriptor as ControllerParameterDescriptor;
            if (ctrlParamDescriptor == null)
            {
                return null;
            }

            var attribute = ctrlParamDescriptor.ParameterInfo
                .GetCustomAttributes(typeof(DateTimeModelBinderAttribute), false)
                .FirstOrDefault();

            return (DateTimeModelBinderAttribute)attribute;
        }

        private string GetModelName(ModelBindingContext bindingContext)
        {
            // The "Name" property of the ModelBinder attribute can be used to specify the
            // route parameter name when the action parameter name is different from the route parameter name.
            // For instance, when the route is /api/{birthDate} and the action parameter name is "date".
            // We can add this attribute with a Name property [DateTimeModelBinder(Name ="birthDate")]
            // Now bindingContext.BinderModelName will be "birthDate" and bindingContext.ModelName will be "date"
            if (!string.IsNullOrEmpty(bindingContext.BinderModelName))
            {
                return bindingContext.BinderModelName;
            }

            return bindingContext.ModelName;
        }


    }

    public class DateTimeModelBinderAttribute : ModelBinderAttribute
    {
        public string DateFormat { get; set; }

        public DateTimeModelBinderAttribute()
            : base(typeof(DateTimeModelBinder))
        {
        }
    }

    public class CustomDateTimeConverter : DateTimeConverterBase
    {
        private readonly string dateFormat = null;
        private readonly DateTimeConverterBase innerConverter = null;

        public CustomDateTimeConverter()
            : this(dateFormat: null) { }

        public CustomDateTimeConverter(string dateFormat = null)
            : this(dateFormat, innerConverter: new IsoDateTimeConverter()) { }

        public CustomDateTimeConverter(string dateFormat = null, DateTimeConverterBase innerConverter = null)
        {
            this.dateFormat = dateFormat;
            this.innerConverter = innerConverter ?? new IsoDateTimeConverter();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var isNullableType = Global.IsNullableType(objectType);

            if (reader.TokenType == JsonToken.Null)
            {
                if (isNullableType)
                {
                    return null;
                }

                throw new JsonSerializationException($"Cannot convert null value to {objectType}.");
            }

            if (reader.TokenType != JsonToken.String)
            {
                throw new JsonSerializationException($"Unexpected token parsing date. Expected {nameof(String)}, got {reader.TokenType}.");
            }

            var dateToParse = reader.Value.ToString();

            if (isNullableType && string.IsNullOrWhiteSpace(dateToParse))
            {
                return null;
            }

            if (string.IsNullOrEmpty(this.dateFormat))
            {
                return Global.ParseDateTime(dateToParse);
            }

            return Global.ParseDateTime(dateToParse, new string[] { this.dateFormat });
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }

    public class DateTimeModelBinderProvider : IModelBinderProvider
    {
        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            if (DateTimeModelBinder.SUPPORTED_TYPES.Contains(context.Metadata.ModelType))
            {
                return new BinderTypeModelBinder(typeof(DateTimeModelBinder));
            }

            return null;
        }
    }

}
