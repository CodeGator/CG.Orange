﻿
namespace CG.Orange.Directors;

/// <summary>
/// This class is a default implementation of the <see cref="ISeedDirector"/>
/// interface.
/// </summary>
public class SeedDirector : SeedDirectorBase<SeedDirector>
{
    // *******************************************************************
    // Fields.
    // *******************************************************************

    #region Fields

    /// <summary>
    /// This field contains the provider manager for this director.
    /// </summary>
    internal protected readonly IProviderManager _providerManager = null!;

    /// <summary>
    /// This field contains the provider manager for this director.
    /// </summary>
    internal protected readonly IProviderPropertyManager _providerPropertyManager = null!;

    /// <summary>
    /// This field contains the setting file manager for this director.
    /// </summary>
    internal protected readonly ISettingFileManager _settingFileManager = null!;

    #endregion

    // *******************************************************************
    // Constructors.
    // *******************************************************************

    #region Constructors

    /// <summary>
    /// This constructor creates a new instance of the <see cref="SeedDirector"/>
    /// class.
    /// </summary>
    /// <param name="providerManager">The provider manager to use with 
    /// this director.</param>
    /// <param name="providerPropertyManager">The provider parameter manager
    /// to use with this director.</param>
    /// <param name="settingFileManager">The setting file manager to use 
    /// with this director.</param>
    /// <param name="logger">The logger to use with this director.</param>
    public SeedDirector(
        IProviderManager providerManager,
        IProviderPropertyManager providerPropertyManager,
        ISettingFileManager settingFileManager,
        ILogger<SeedDirector> logger
        ) : base(logger)
    {
        // Validate the parameters before attempting to use them.
        Guard.Instance().ThrowIfNull(providerManager, nameof(providerManager))
            .ThrowIfNull(providerPropertyManager, nameof(providerPropertyManager))
            .ThrowIfNull(settingFileManager, nameof(settingFileManager));

        // Save the reference(s).
        _providerManager = providerManager;
        _providerPropertyManager = providerPropertyManager;
        _settingFileManager = settingFileManager;
    }

    #endregion

    // *******************************************************************
    // Protected methods.
    // *******************************************************************

    #region Protected methods

    /// <inheritdoc/>
    protected override async Task SeedFromConfiguration(
        string objectName,
        IConfiguration dataSection,
        bool force,
        string userName,
        CancellationToken cancellationToken = default
        )
    {
        // Decide what to do with the incoming data.
        switch (objectName.ToLower().Trim())
        {
            case "providers":
                await SeedProvidersAsync(
                    dataSection,
                    force,
                    userName,
                    cancellationToken
                    ).ConfigureAwait(false);
                break;
            case "settingfiles":
                await SeedSettingFilesAsync(
                    dataSection,
                    force,
                    userName,
                    cancellationToken
                    ).ConfigureAwait(false);
                break;
            default:
                throw new ArgumentException(
                    $"Don't know how to seed '{objectName}' types!"
                    );
        }
    }

    #endregion

    // *******************************************************************
    // Private methods.
    // *******************************************************************

    #region Private methods

    /// <summary>
    /// This method performs a seeding operation for <see cref="ProviderModel"/>
    /// objects.
    /// </summary>
    /// <param name="dataSection">The data to use for the operation.</param>
    /// <param name="force"><c>true</c> to force the seeding operation when data
    /// already exists in the associated table(s), <c>false</c> to stop the 
    /// operation whenever data is detected in the associated table(s).</param>
    /// <param name="userName">The user name of the person performing the 
    /// operation.</param>
    /// <param name="cancellationToken">A cancellation token that is monitored
    /// for the lifetime of the method.</param>
    /// <returns>A task to perform the operation.</returns>
    private async Task SeedProvidersAsync(
        IConfiguration dataSection,
        bool force,
        string userName,
        CancellationToken cancellationToken = default
        )
    {
        // Log what we are about to do.
        _logger.LogDebug(
            "Checking the force flag"
            );

        // Should we check for existing data?
        if (!force)
        {
            // Log what we are about to do.
            _logger.LogDebug(
                "Checking for existing providers"
                );

            // Are there existing providers?
            var hasExistingData = await _providerManager.AnyAsync(
                cancellationToken
                ).ConfigureAwait(false);

            // Should we stop?
            if (hasExistingData)
            {
                // Log what we didn't do.
                _logger.LogWarning(
                    "Skipping seeding providers because the 'force' flag " +
                    "was not specified and there are existing rows in the " +
                    "database."
                    );
                return; // Nothing else to do!
            }

            // Are there existing properties?
            hasExistingData = await _providerPropertyManager.AnyAsync(
                cancellationToken
                ).ConfigureAwait(false);

            // Should we stop?
            if (hasExistingData)
            {
                // Log what we didn't do.
                _logger.LogWarning(
                    "Skipping seeding provider properties because the 'force' flag " +
                    "was not specified and there are existing rows in the " +
                    "database."
                    );
                return; // Nothing else to do!
            }
        }

        try
        {
            // Start by counting how many providers are already there.
            var originalProviderCount = await _providerManager.CountAsync(
                cancellationToken
                ).ConfigureAwait(false);

            // Start by counting how many properties are already there.
            var originalPropertyCount = await _providerPropertyManager.CountAsync(
                cancellationToken
                ).ConfigureAwait(false);

            // Log what we are about to do.
            _logger.LogDebug(
                "Binding the incoming seed data to our options"
                );

            // Bind the incoming data to our options.
            var options = new ProviderOptions();
            dataSection.Bind(options);

            // Log what we are about to do.
            _logger.LogDebug(
                "Validating the incoming seed data"
                );

            // Validate the options.
            Guard.Instance().ThrowIfInvalidObject(options, nameof(options));

            // Log what we are about to do.
            _logger.LogDebug(
                "Seeding '{count}' providers",
                options.Providers
                );

            // Loop through the objects.
            foreach (var provider in options.Providers)
            {
                // Log what we are about to do.
                _logger.LogDebug(
                    "Creating provider: {name} with {count} properties",
                    provider.Name,
                    provider.Properties.Count()
                    );

                // Create the provider.
                _ = await _providerManager.CreateAsync(
                    provider,
                    userName,
                    cancellationToken
                    ).ConfigureAwait(false);
            }

            // Count how many providers are there now.
            var finalProviderCount = await _providerManager.CountAsync(
                cancellationToken
                ).ConfigureAwait(false);

            // Count how many properties are there now.
            var finalPropertyCount = await _providerPropertyManager.CountAsync(
                cancellationToken
                ).ConfigureAwait(false);

            // Log what we did.
            _logger.LogInformation(
                "Seeded a total of {count} providers and {count} properties",
                finalProviderCount - originalProviderCount,
                finalPropertyCount - originalPropertyCount
                );
        }
        catch (Exception ex)
        {
            // Log what happened.
            _logger.LogError(
                ex,
                "Failed to seed one or more providers / properties!"
                );
        }
    }

    // *******************************************************************

    /// <summary>
    /// This method performs a seeding operation for <see cref="SettingFileModel"/>
    /// objects.
    /// </summary>
    /// <param name="dataSection">The data to use for the operation.</param>
    /// <param name="force"><c>true</c> to force the seeding operation when data
    /// already exists in the associated table(s), <c>false</c> to stop the 
    /// operation whenever data is detected in the associated table(s).</param>
    /// <param name="userName">The user name of the person performing the 
    /// operation.</param>
    /// <param name="cancellationToken">A cancellation token that is monitored
    /// for the lifetime of the method.</param>
    /// <returns>A task to perform the operation.</returns>
    private async Task SeedSettingFilesAsync(
        IConfiguration dataSection,
        bool force,
        string userName,
        CancellationToken cancellationToken = default
        )
    {
        // Log what we are about to do.
        _logger.LogDebug(
            "Checking the force flag"
            );

        // Should we check for existing data?
        if (!force)
        {
            // Log what we are about to do.
            _logger.LogDebug(
                "Checking for existing setting files"
                );

            // Are there existing setting files?
            var hasExistingData = await _settingFileManager.AnyAsync(
                cancellationToken
                ).ConfigureAwait(false);

            // Should we stop?
            if (hasExistingData)
            {
                // Log what we didn't do.
                _logger.LogWarning(
                    "Skipping seeding setting files because the 'force' flag " +
                    "was not specified and there are existing rows in the " +
                    "database."
                    );
                return; // Nothing else to do!
            }
        }

        try
        {
            // Start by counting how many objects are already there.
            var originalCount = await _settingFileManager.CountAsync(
                cancellationToken
                ).ConfigureAwait(false);

            // Log what we are about to do.
            _logger.LogDebug(
                "Binding the incoming seed data to our options"
                );

            // Bind the incoming data to our options.
            var options = new SettingFileOptions();
            dataSection.Bind(options);

            // Log what we are about to do.
            _logger.LogDebug(
                "Validating the incoming seed data"
                );

            // Validate the options.
            Guard.Instance().ThrowIfInvalidObject(options, nameof(options));

            // Log what we are about to do.
            _logger.LogDebug(
                "Seeding '{count}' setting files",
                options.SettingFiles
                );

            // Loop through the objects.
            foreach (var settingFile in options.SettingFiles)
            {
                // Log what we are about to do.
                _logger.LogDebug(
                    "Creating a setting file"
                    );

                // Create the setting file.
                _ = await _settingFileManager.CreateAsync(
                    settingFile,
                    userName,
                    cancellationToken
                    ).ConfigureAwait(false);
            }

            // Count how many objects are there now.
            var finalCount = await _settingFileManager.CountAsync(
                cancellationToken
                ).ConfigureAwait(false);

            // Log what we did.
            _logger.LogInformation(
                "Seeded a total of {count} setting files",
                finalCount - originalCount
                );
        }
        catch (Exception ex)
        {
            // Log what happened.
            _logger.LogError(
                ex,
                "Failed to seed one or more setting files!"
                );
        }
    }

    #endregion
}
