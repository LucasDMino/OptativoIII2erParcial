using OPTATIVOIII3ERPARCIAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.AspNetCore.Http.HttpResults;
namespace OPTATIVOIII3ERPARCIAL.Models
{
    public class Cliente
    {
        public int idCliente { get; set; }
        public int idPersona { get; set; }
        public DateTime FechaIngreso { get; set; }
        public string Calificacion { get; set; }
        public string Estado { get; set; }

        // Relación con la tabla Persona
        public Persona Persona { get; set; }

    }


public static class ClienteEndpoints
{
	public static void MapClienteEndpoints (this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/Cliente").WithTags(nameof(Cliente));

        group.MapGet("/", async (AppDbContext db) =>
        {
            return await db.Clientes.ToListAsync();
        })
        .WithName("GetAllClientes")
        .WithOpenApi();

        group.MapGet("/{id}", async Task<Results<Ok<Cliente>, NotFound>> (int idcliente, AppDbContext db) =>
        {
            return await db.Clientes.AsNoTracking()
                .FirstOrDefaultAsync(model => model.idCliente == idcliente)
                is Cliente model
                    ? TypedResults.Ok(model)
                    : TypedResults.NotFound();
        })
        .WithName("GetClienteById")
        .WithOpenApi();

        group.MapPut("/{id}", async Task<Results<Ok, NotFound>> (int idcliente, Cliente cliente, AppDbContext db) =>
        {
            var affected = await db.Clientes
                .Where(model => model.idCliente == idcliente)
                .ExecuteUpdateAsync(setters => setters
                  .SetProperty(m => m.idCliente, cliente.idCliente)
                  .SetProperty(m => m.idPersona, cliente.idPersona)
                  .SetProperty(m => m.FechaIngreso, cliente.FechaIngreso)
                  .SetProperty(m => m.Calificacion, cliente.Calificacion)
                  .SetProperty(m => m.Estado, cliente.Estado)
                );

            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("UpdateCliente")
        .WithOpenApi();

        group.MapPost("/", async (Cliente cliente, AppDbContext db) =>
        {
            db.Clientes.Add(cliente);
            await db.SaveChangesAsync();
            return TypedResults.Created($"/api/Cliente/{cliente.idCliente}",cliente);
        })
        .WithName("CreateCliente")
        .WithOpenApi();

        group.MapDelete("/{id}", async Task<Results<Ok, NotFound>> (int idcliente, AppDbContext db) =>
        {
            var affected = await db.Clientes
                .Where(model => model.idCliente == idcliente)
                .ExecuteDeleteAsync();

            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("DeleteCliente")
        .WithOpenApi();
    }
}}
