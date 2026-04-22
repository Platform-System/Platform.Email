using Platform.Email.DependencyInjection;

var builder = Host.CreateApplicationBuilder(args);

// Worker này chỉ có nhiệm vụ boot DI, MassTransit và consumer
// để nhận event gửi invoice email từ RabbitMQ.
builder.Services.AddPlatformEmailProcessing(builder.Configuration);

var host = builder.Build();
await host.RunAsync();
