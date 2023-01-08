
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
    /// This method adds required services for this in-memory provider.
    /// </summary>
    /// <param name="webApplicationBuilder">The web application builder to
    /// use for the operation.</param>
    /// <param name="sectionName">The configuration section to use for the 
    /// operation. Defaults to <c>DAL:InMemory</c>.</param>
    /// <param name="bootstrapLogger">A bootstrap logger to use for the
    /// operation.</param>
    /// <returns>The value of the <paramref name="webApplicationBuilder"/>
    /// parameter, for chaining calls together, Fluent style.</returns>
    /// <exception cref="ArgumentException">This exception is thrown whenever
    /// one or more arguments are missing, or invalid.</exception>
    /// <remarks>
    /// <para>
    /// This method must NOT have its signature changed! The method follows 
    /// a convention used by the <see cref="CG.EntityFrameworkCore"/> 
    /// package.
    /// </para>
    /// </remarks>
    public static WebApplicationBuilder AddInMemoryDataAccess(
        this WebApplicationBuilder webApplicationBuilder,
        string sectionName = "DAL:InMemory",
        ILogger? bootstrapLogger = null
        )
    {
        // Validate the parameters before attempting to use them.
        Guard.Instance().ThrowIfNull(webApplicationBuilder, nameof(webApplicationBuilder))
            .ThrowIfNullOrEmpty(sectionName, nameof(sectionName));

        // Register the data-context.
        webApplicationBuilder.Services.AddDbContext<OrangeDbContext>(options =>
        {
            // Get the configuration section.
            var section = webApplicationBuilder.Configuration.GetSection(sectionName);

            // Get the database name,
            var databaseName = section["DatabaseName"];

            // Sanity check the database name.
            if (string.IsNullOrEmpty(databaseName))
            {
                // Panic!!
                throw new ArgumentException(
                    message: $"The database name at '{sectionName}:DatabaseName', " +
                    "in the configuration, is required for migrations but is " +
                    "currently missing, or empty!"
                    );
            }

            // Use the in-memory provider with our database name.
            options.UseInMemoryDatabase(
                databaseName,
                inMemoryOptions =>
                {
                    
                });
        });

        // Return the application builder.
        return webApplicationBuilder;
    }

    #endregion
}
