
namespace CG.Orange;

/// <summary>
/// This class contains global compile-time constants for the <see cref="CG.Orange"/>
/// microservice.
/// </summary>
public static class Globals
{
    /// <summary>
    /// This class contains model property sizes.
    /// </summary>
    public static class Models
    {
        /// <summary>
        /// This class contains sizes for <see cref="AuditedModelBase"/> properties.
        /// </summary>
        public static class AuditedModelBases
        {
            /// <summary>
            /// This constant represents the length of the <see cref="AuditedModelBase.CreatedBy"/> 
            /// property.
            /// </summary>
            public const int CreatedByLength = 32;

            /// <summary>
            /// This constant represents the length of the <see cref="AuditedModelBase.CreatedBy"/> 
            /// property.
            /// </summary>
            public const int LastUpdatedByLength = 32;
        }

        // *******************************************************************

        /// <summary>
        /// This class contains sizes for <see cref="ProviderModel"/> properties.
        /// </summary>
        public static class Providers
        {
            /// <summary>
            /// This constant represents the length of the <see cref="ProviderModel.Tag"/> 
            /// property.
            /// </summary>
            public const int TagLength = 12;

            /// <summary>
            /// This constant represents the length of the <see cref="ProviderModel.Name"/> 
            /// property.
            /// </summary>
            public const int NameLength = 32;

            /// <summary>
            /// This constant represents the length of the <see cref="ProviderModel.Description"/> 
            /// property.
            /// </summary>
            public const int DescriptionLength = 255;

            /// <summary>
            /// This constant represents the length of the <see cref="ProviderModel.ProcessorType"/> 
            /// property.
            /// </summary>
            public const int ProcessorTypeLength = 2048;

            /// <summary>
            /// This constant represents the length of the <see cref="ProviderModel.ProviderType"/> 
            /// property.
            /// </summary>
            public const int ProviderTypeLength = 32;
        }

        // *******************************************************************

        /// <summary>
        /// This class contains sizes for <see cref="SettingFileModel"/> properties.
        /// </summary>
        public static class SettingFiles
        {
            /// <summary>
            /// This constant represents the length of the <see cref="SettingFileModel.ApplicationName"/> 
            /// property.
            /// </summary>
            public const int ApplicationNameLength = 32;

            /// <summary>
            /// This constant represents the length of the <see cref="SettingFileModel.EnvironmentName"/> 
            /// property.
            /// </summary>
            public const int EnvironmentNameLength = 32;
        }

        // *******************************************************************

        /// <summary>
        /// This class contains sizes for <see cref="SettingFileCountModel"/> properties.
        /// </summary>
        public static class SettingFileCounts
        {
            /// <summary>
            /// This constant represents the length of the <see cref="SettingFileCountModel.CreatedBy"/> 
            /// property.
            /// </summary>
            public const int CreatedByLength = 32;
        }
    }
}
