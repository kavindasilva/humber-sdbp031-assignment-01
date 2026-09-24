
using AgentAssignment.Server.Contracts;

namespace AgentAssignment.Server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var port = builder.Configuration.GetValue<int?>("Server:Port") ?? 5080;
            builder.WebHost.ConfigureKestrel(options => options.ListenLocalhost(port));

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            builder.Services.AddSingleton<IChatCompletionService, FoundryLocalChatCompletionService>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            _ = Task.Run(async () =>
            {
                try
                {
                    //
                    var foundryService = app.Services.GetRequiredService<FoundryLocalChatCompletionService>();
                    foundryService.EnsureStartedAsync();
                }
                catch (Exception ex)
                {
                    //
                }
            });
            //app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
