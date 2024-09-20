using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace trayprojeto45.Endpoint
{
    public static class Pessoas
    {
        public static void RegistrarEndpointsPessoa(this IEndpointRouteBuilder rotas)
        {
            // Grupamento de rotas
            RouteGroupBuilder rotaPessoas = rotas.MapGroup("/Pessoa");

            // GET      /pessoa?nome={nome}&email={email}
            rotaPessoas.MapGet("/", async context =>
            {
                using var dbContext = context.RequestServices.GetRequiredService<trayprojeto45DbContext>();

                string nomePessoa = context.Request.Query["nome"];
                string emailPessoa = context.Request.Query["email"];

                IQueryable<Pessoa> pessoasFiltradas = dbContext.Pessoas;

                // Verifica se foi passado o nome da pessoa como parâmetro de busca
                if (!string.IsNullOrEmpty(nomePessoa))
                {
                    // Filtra as pessoas por Nome
                    pessoasFiltradas = pessoasFiltradas
                        .Where(p => p.Nome.Contains(nomePessoa, StringComparison.OrdinalIgnoreCase));
                }

                // Verifica se foi passado o email da pessoa como parâmetro de busca
                if (!string.IsNullOrEmpty(emailPessoa))
                {
                    // Filtra as pessoas por Email
                    pessoasFiltradas = pessoasFiltradas
                        .Where(p => p.Email.Equals(emailPessoa, StringComparison.OrdinalIgnoreCase));
                }

                // Retorna as pessoas filtradas
                await context.Response.WriteAsJsonAsync(pessoasFiltradas.ToList());
            });

            // GET      /pessoa/{Id}
            rotaPessoas.MapGet("/{Id}", async context =>
            {
                using var dbContext = context.RequestServices.GetRequiredService<trayprojeto45DbContext>();

                string Id = context.Request.RouteValues["Id"] as string;

                // Procura pela pessoa com o Id recebido
                Pessoa? pessoa = await dbContext.Pessoas.FindAsync(Id);
                if (pessoa is null)
                {
                    // Indica que a pessoa não foi encontrada
                    context.Response.StatusCode = StatusCodes.Status404NotFound;
                    return;
                }

                // Devolve a pessoa encontrada
                await context.Response.WriteAsJsonAsync(pessoa);
            });

            // POST     /pessoa
            rotaPessoas.MapPost("/", async context =>
            {
                using var dbContext = context.RequestServices.GetRequiredService<trayprojeto45DbContext>();

                var pessoa = await context.Request.ReadFromJsonAsync<Pessoa>();

                dbContext.Pessoas.Add(pessoa);
                await dbContext.SaveChangesAsync();

                context.Response.StatusCode = StatusCodes.Status201Created;
                context.Response.Headers["Location"] = $"/pessoa/{pessoa.Id}";
                await context.Response.WriteAsJsonAsync(pessoa);
            });

            // PUT      /pessoa/{Id}
            rotaPessoas.MapPut("/{Id}", async context =>
            {
                using var dbContext = context.RequestServices.GetRequiredService<trayprojeto45DbContext>();

                string Id = context.Request.RouteValues["Id"] as string;

                // Encontra a pessoa especificada buscando pelo Id enviado
                Pessoa? pessoaEncontrada = await dbContext.Pessoas.FindAsync(Id);
                if (pessoaEncontrada is null)
                {
                    // Indica que a pessoa não foi encontrada
                    context.Response.StatusCode = StatusCodes.Status404NotFound;
                    return;
                }

                // Atualiza os dados da pessoa
                var pessoaAtualizada = await context.Request.ReadFromJsonAsync<Pessoa>();
                pessoaAtualizada.Id = pessoaEncontrada.Id;
                dbContext.Entry(pessoaEncontrada).CurrentValues.SetValues(pessoaAtualizada);
                await dbContext.SaveChangesAsync();

                context.Response.StatusCode = StatusCodes.Status204NoContent;
            });

            // DELETE   /pessoa/{Id}
            rotaPessoas.MapDelete("/{Id}", async context =>
            {
                using var dbContext = context.RequestServices.GetRequiredService<trayprojeto45DbContext>();

                string Id = context.Request.RouteValues["Id"] as string;

                // Encontra a pessoa especificada buscando pelo Id enviado
                Pessoa? pessoaEncontrada = await dbContext.Pessoas.FindAsync(Id);
                if (pessoaEncontrada is null)
                {
                    // Indica que a pessoa não foi encontrada
                    context.Response.StatusCode = StatusCodes.Status404NotFound;
                    return;
                }

                // Remove a pessoa encontrada
                dbContext.Pessoas.Remove(pessoaEncontrada);
                await dbContext.SaveChangesAsync();

                context.Response.StatusCode = StatusCodes.Status204NoContent;
            });

            // PATCH    /pessoa/{Id}
            rotaPessoas.MapPatch("/{Id}", async context =>
            {
                using var dbContext = context.RequestServices.GetRequiredService<trayprojeto45DbContext>();

                string Id = context.Request.RouteValues["Id"] as string;

                Pessoa? pessoaEncontrada = await dbContext.Pessoas.FindAsync(Id);
                if (pessoaEncontrada is null)
                {
                    context.Response.StatusCode = StatusCodes.Status404NotFound;
                    return;
                }

                var pessoaAtualizada = await context.Request.ReadFromJsonAsync<Pessoa>();
                dbContext.Entry(pessoaEncontrada).CurrentValues.SetValues(pessoaAtualizada);
                await dbContext.SaveChangesAsync();

                context.Response.StatusCode = StatusCodes.Status204NoContent;
            });
        }
    }
}
