namespace Skeleton.Web.Serialization.Protobuf.Configuration
{
    using System;
    using Formatters.Surrogates;
    using ProtoBuf.Meta;

    public static class RuntimeTypeModelExtensions
    {
        public static RuntimeTypeModel WithDefaultValuesHandling(this RuntimeTypeModel model, bool useImplicitDefaults)
        {
            if(model == null)
                throw new ArgumentNullException(nameof(model));

            model.UseImplicitZeroDefaults = useImplicitDefaults;
            return model;
        }

        public static RuntimeTypeModel WithDataContractsHandling(this RuntimeTypeModel model, bool supportDataContracts)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            model.AutoAddProtoContractTypesOnly = !supportDataContracts;
            return model;
        }

        public static RuntimeTypeModel WithDateTimeKindHandling(this RuntimeTypeModel model, bool includeDateTimeKind)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            model.IncludeDateTimeKind = includeDateTimeKind;
            return model;
        }

        public static RuntimeTypeModel WithTypeSurrogate<TType, TTypeSurrogate>(this RuntimeTypeModel model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            model.Add(typeof(TType), false).SetSurrogate(typeof(TTypeSurrogate));
            return model;
        }

        public static RuntimeTypeModel WithDefaultSettings(this RuntimeTypeModel model)
        {
            return model
                .WithDefaultValuesHandling(false)
                .WithTypeSurrogate<DateTimeOffset, DateTimeOffsetSurrogate>();
        }
    }
}