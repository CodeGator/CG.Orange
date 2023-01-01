
namespace Microsoft.AspNetCore.Builder;

/// <summary>
/// This class contains extension methods related to the <see cref="WebApplicationBuilder"/>
/// type.
/// </summary>
public static class WebApplicationBuilderExtensions003
{
    // *******************************************************************
    // Public methods.
    // *******************************************************************

    #region Public methods

    /// <summary>
    /// This method adds repositories and related services, for the <see cref="CG.Orange"/>
    /// data-access layer.
    /// </summary>
    /// <param name="webApplicationBuilder">The web application builder to
    /// use for the operation.</param>
    /// <param name="sectionName">The configuration section to use for the 
    /// operation. Defaults to <c>DAL</c>.</param>
    /// <param name="bootstrapLogger">A bootstrap logger to use for the
    /// operation.</param>
    /// <returns>The value of the <paramref name="webApplicationBuilder"/>
    /// parameter, for chaining calls together, Fluent style.</returns>
    /// <exception cref="ArgumentException">This exception is thrown whenever
    /// one or more arguments are missing, or invalid.</exception>
    public static WebApplicationBuilder AddRepositories(
        this WebApplicationBuilder webApplicationBuilder,
        string sectionName = "DAL",
        ILogger? bootstrapLogger = null
        )
    {
        // Validate the parameters before attempting to use them.
        Guard.Instance().ThrowIfNull(webApplicationBuilder, nameof(webApplicationBuilder))
            .ThrowIfNullOrEmpty(sectionName, nameof(sectionName));

        // Tell the world what we are about to do.
        bootstrapLogger?.LogDebug(
            "Wiring up the Orange repositories"
            );

        // Add the repositories
        webApplicationBuilder.Services.AddScoped<ISettingFileRepository, SettingFileRepository>();
        webApplicationBuilder.Services.AddScoped<IProviderRepository, ProviderRepository>();
        webApplicationBuilder.Services.AddScoped<IProviderPropertyRepository, ProviderPropertyRepository>();

        // Tell the world what we are about to do.
        bootstrapLogger?.LogDebug(
            "Wiring up the auto-mapper"
            );

        // Wire up the auto-mapper.
        webApplicationBuilder.Services.AddAutoMapper(cfg =>
        {
            // Wire up the conversion maps.
            cfg.CreateMap<SettingFileEntity, SettingFileModel>().ReverseMap();
            cfg.CreateMap<ProviderEntity, ProviderModel>().ReverseMap();
            cfg.CreateMap<ProviderPropertyEntity, ProviderPropertyModel>().ReverseMap();
        });

        // Return the application builder.
        return webApplicationBuilder;
    }

    #endregion
}
