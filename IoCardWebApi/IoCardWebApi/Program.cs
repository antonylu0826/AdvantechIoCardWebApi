using Serilog;

var builder = WebApplication.CreateBuilder(args);
Log.Logger = new LoggerConfiguration()
    .MinimumLevel
    .Information()
    .WriteTo
    .File("logs/IoCardApi_.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

Log.Information("The API application Start.");

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet(
    "/GetSignal/{pin}",
    (HttpContext context, int pin) =>
    {
        try
        {
            Log.Information("-----------Start Call Get Signal function.-----------");
            Log.Information($"Client IP: {context.Connection.RemoteIpAddress}");

            string cardID = builder.Configuration.GetConnectionString("DiCardID");
            int totalPort = Convert.ToInt32(builder.Configuration.GetConnectionString("DiCardTotalPort"));

            Log.Information($"DiCardID: {cardID}");
            Log.Information($"DiCardTotalPort: {totalPort}");

            string[]? signals = IoCardWebApi.DiCard.GetSignal(cardID, totalPort);
            if (signals != null)
            {
                string? pinSignal = signals[pin];
                Log.Information($"Pin {pin} Signal: {pinSignal}");
                Log.Information("-----------End Call Get Signal function.-----------");
                return pinSignal;
            }
            else
            {
                Log.Error("Signal: null");
                Log.Information("-----------End Call Get Signal function.-----------");
                return string.Empty;
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error");
            return string.Empty;
        }
    })
    .WithName("GetSignal");

app.MapGet(
    "/GetSignals",
    (HttpContext context) =>
    {
        try
        {
            Log.Information("-----------Start Call Get Signal function.-----------");
            Log.Information($"Client IP: {context.Connection.RemoteIpAddress}");

            string cardID = builder.Configuration.GetConnectionString("DiCardID");
            int totalPort = Convert.ToInt32(builder.Configuration.GetConnectionString("DiCardTotalPort"));

            Log.Information($"DiCardID: {cardID}");
            Log.Information($"DiCardTotalPort: {totalPort}");

            string[]? signals = IoCardWebApi.DiCard.GetSignal(cardID, totalPort);
            if (signals != null)
            {
                Log.Information($"Signals: {signals}");
                Log.Information("-----------End Call Get Signal function.-----------");
                return signals;
            }
            else
            {
                Log.Error("Signal: null");
                Log.Information("-----------End Call Get Signal function.-----------");
                return new string[] { };
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error");
            return new string[] { };
        }
    })
    .WithName("GetSignals");

app.Run();
