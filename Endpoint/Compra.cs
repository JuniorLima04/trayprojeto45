using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace trayprojeto45.Endpoint
{
    public static class Compras
    {
        public static void RegistrarEndpointsCompra(this IEndpointRouteBuilder rotas)
        {
            // Grupamento de rotas
            var rotaCompras = rotas.MapGroup("/Compra");

            // GET      /Compra
            rotaCompras.MapGet("/", async context =>
            {
                try
                {
                    using var dbContext = context.RequestServices.GetRequiredService<trayprojeto45DbContext>();

                    // Parâmetros de filtro
                    var produtoCompra = context.Request.Query["produto"].ToString();
                    var precoCompraStr = context.Request.Query["preco"].ToString();
                    var cidadeCompra = context.Request.Query["cidade"].ToString();
                    var estadoCompra = context.Request.Query["estado"].ToString();

                    // Converte o preço para decimal, se possível
                    decimal precoCompra;
                    if (!decimal.TryParse(precoCompraStr, out precoCompra))
                    {
                        precoCompra = 0; // Ou um valor padrão, se preferir
                    }

                    // Query inicial de compras
                    var comprasFiltradas = dbContext.Compra.AsQueryable();

                    // Aplicando filtros conforme os parâmetros recebidos
                    if (!string.IsNullOrEmpty(produtoCompra))
                    {
                        comprasFiltradas = comprasFiltradas
                            .Where(c => c.Produto.Contains(produtoCompra, StringComparison.OrdinalIgnoreCase));
                    }

                    if (!string.IsNullOrEmpty(precoCompraStr))
                    {
                        comprasFiltradas = comprasFiltradas
                            .Where(c => c.Preco == precoCompra);
                    }

                    if (!string.IsNullOrEmpty(cidadeCompra))
                    {
                        comprasFiltradas = comprasFiltradas
                            .Where(c => c.Cidade.Equals(cidadeCompra, StringComparison.OrdinalIgnoreCase));
                    }

                    if (!string.IsNullOrEmpty(estadoCompra))
                    {
                        comprasFiltradas = comprasFiltradas
                            .Where(c => c.Estado.Contains(estadoCompra, StringComparison.OrdinalIgnoreCase));
                    }

                    // Retornando o resultado como JSON
                    var result = await comprasFiltradas.ToListAsync();
                    await context.Response.WriteAsJsonAsync(result);
                }
                catch (Exception ex)
                {
                    context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                    await context.Response.WriteAsync($"Erro ao buscar compras: {ex.Message}");
                }
            });

            // POST     /Compra
            rotaCompras.MapPost("/", async context =>
            {
                try
                {
                    using var dbContext = context.RequestServices.GetRequiredService<trayprojeto45DbContext>();
                    var compra = await context.Request.ReadFromJsonAsync<Compra>();

                    if (compra != null)
                    {
                        dbContext.Add(compra);
                        await dbContext.SaveChangesAsync();
                        context.Response.StatusCode = StatusCodes.Status201Created;
                        await context.Response.WriteAsJsonAsync(compra);
                    }
                    else
                    {
                        context.Response.StatusCode = StatusCodes.Status400BadRequest;
                        await context.Response.WriteAsync("Dados da compra inválidos.");
                    }
                }
                catch (Exception ex)
                {
                    context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                    await context.Response.WriteAsync($"Erro ao cadastrar compra: {ex.Message}");
                }
            });
        }
    }
}
