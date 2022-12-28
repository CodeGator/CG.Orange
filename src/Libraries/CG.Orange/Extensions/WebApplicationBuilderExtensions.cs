﻿
namespace Microsoft.AspNetCore.Builder;

/// <summary>
/// This class contains extension methods related to the <see cref="WebApplicationBuilder"/>
/// type.
/// </summary>
public static class WebApplicationBuilderExtensions001
{
    // *******************************************************************
    // Public methods.
    // *******************************************************************

    #region Public methods

    /// <summary>
    /// This method adds the business logic layer (managers and directors) 
    /// for the <see cref="CG.Orange"/> project.
    /// </summary>
    /// <param name="webApplicationBuilder">The web application builder to
    /// use for the operation.</param>
    /// <param name="sectionName">The configuration section to use for the 
    /// operation. Defaults to <c>BLL</c>.</param>
    /// <param name="bootstrapLogger">The bootstrap logger to use for the 
    /// operation.</param>
    /// <returns>The value of the <paramref name="webApplicationBuilder"/>
    /// parameter, for chaining calls together, Fluent style.</returns>
    /// <exception cref="ArgumentException">This exception is thrown whenever
    /// one or more arguments are missing, or invalid.</exception>
    public static WebApplicationBuilder AddBusinessLayer(
        this WebApplicationBuilder webApplicationBuilder,
        string sectionName = "BLL",
        ILogger? bootstrapLogger = null
        )
    {
        // Validate the parameters before attempting to use them.
        Guard.Instance().ThrowIfNull(webApplicationBuilder, nameof(webApplicationBuilder));

        // Tell the world what we are about to do.
        bootstrapLogger?.LogDebug(
            "Configuring options from the {section} section",
            sectionName
            );

        // Configure the BLL options.
        webApplicationBuilder.Services.ConfigureOptions<OrangeBllOptions>(
            webApplicationBuilder.Configuration.GetSection(sectionName),
            out var options
            );

        // Tell the world what we are about to do.
        bootstrapLogger?.LogDebug(
            "Wiring up the BLL managers"
            );

        // Add the managers.
        webApplicationBuilder.Services.AddScoped<ISettingFileManager, SettingFileManager>();
                
        // Return the application builder.
        return webApplicationBuilder;
    }

    #endregion
}
