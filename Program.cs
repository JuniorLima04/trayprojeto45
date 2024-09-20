using trayprojeto45.Endpoint;
using trayprojeto45;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Configura��o do servi�o de banco de dados
        builder.Services.AddDbContext<trayprojeto45DbContext>();

        // Configura��o do CORS
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("PermitirTodasOrigens",
                builder =>
                {
                    builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
                });
        });

        var app = builder.Build();

        app.UseCors("PermitirTodasOrigens");

        // Registro dos endpoints
        app.RegistrarEndpointsPessoa();
        app.RegistrarEndpointsCompra();

        app.Run();
    }
}
