
### CG.Orange.Clients - README

This project contains configuration client(s) for the **CG.Orange** microservice.

#### Notes

The client requires the following information to function correctly:

- Application
: The application settings you want to receive from the **CG.Orange** microservice. This value must match the application name on the corresponding settings, in **CG.Orange**.

The client may also require the following optional properties:

- Environment
: An optional environment name, such as 'development'.

- ClientId
: An optional client identifier for your application. The content of this property will vary greatly, depending on how you have your authentication configured.

- ClientSecret
: An optional client secret for your application. If supplied, you should consider this property to be sensitive information and protect it accordingly.

### Examples

This example demonstrates integrating with the **CG.Orange** configuration microservice using hard coded options: 
```
var builder = WebApplication.CreateBuilder(args);
builder.AddOrangeConfiguration(options =>
{
   options.Application = "yourappname";
   options.ClientId = "yourclientid";
   options.ClientSecret = "yoursecret";
});

var app = builder.Build();

app.Run();
```

This example demonstrates integrating with the **CG.Orange** configuration microservice using options from the local configuration: 
```
var builder = WebApplication.CreateBuilder(args);
builder.AddOrangeConfiguration(options =>
   builder.Configuration.GetSection("yoursection").Bind(options)
);

var app = builder.Build();

app.Run();
```




