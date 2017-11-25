namespace Skeleton.Web.Serialization.Protobuf.Configuration
{
    using System;
    using Formatters.Surrogates;
    using ProtoBuf.Meta;

    public static class RuntimeTypeModelExtensions
    {
        private static void ValidateModel(RuntimeTypeModel model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));
        }

        public static RuntimeTypeModel WithDefaultValuesHandling(this RuntimeTypeModel model, bool useImplicitDefaults)
        {
            ValidateModel(model);

            model.UseImplicitZeroDefaults = useImplicitDefaults;
            return model;
        }

        public static RuntimeTypeModel WithDataContractsHandling(this RuntimeTypeModel model, bool supportDataContracts)
        {
            ValidateModel(model);

            model.AutoAddProtoContractTypesOnly = !supportDataContracts;
            return model;
        }

        public static RuntimeTypeModel WithDateTimeKindHandling(this RuntimeTypeModel model, bool includeDateTimeKind)
        {
            ValidateModel(model);

            model.IncludeDateTimeKind = includeDateTimeKind;
            return model;
        }

        public static RuntimeTypeModel WithTypeSurrogate<TType, TTypeSurrogate>(this RuntimeTypeModel model)
        {
            ValidateModel(model);

            model.Add(typeof(TType), false).SetSurrogate(typeof(TTypeSurrogate));
            return model;
        }

        public static RuntimeTypeModel WithDefaultSettings(this RuntimeTypeModel model)
        {
            ValidateModel(model);

            return model
                .WithDefaultValuesHandling(false)
                .WithTypeSurrogate<DateTimeOffset, DateTimeOffsetSurrogate>();
        }
    }
}