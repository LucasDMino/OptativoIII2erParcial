using OPTATIVOIII3ERPARCIAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.AspNetCore.Http.HttpResults;
namespace OPTATIVOIII3ERPARCIAL.Models
{
    public class Persona
    {
        public int idPersona { get; set; }
        public int idCiudad { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string TipoDocumento { get; set; }
        public string NroDocumento { get; set; }
        public string Direccion { get; set; }
        public string Telefono { get; set; }
        public string Email { get; set; }
        public string Estado { get; set; }

        // Relación con la tabla Ciudad
        public Ciudad Ciudad { get; set; }

    }


public static class PersonaEndpoints
{
	public static void MapPersonaEndpoints (this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/Persona").WithTags(nameof(Persona));

        group.MapGet("/", async (AppDbContext db) =>
        {
            return await db.Personas.ToListAsync();
        })
        .WithName("GetAllPersonas")
        .WithOpenApi();

        group.MapGet("/{id}", async Task<Results<Ok<Persona>, NotFound>> (int idpersona, AppDbContext db) =>
        {
            return await db.Personas.AsNoTracking()
                .FirstOrDefaultAsync(model => model.idPersona == idpersona)
                is Persona model
                    ? TypedResults.Ok(model)
                    : TypedResults.NotFound();
        })
        .WithName("GetPersonaById")
        .WithOpenApi();

        group.MapPut("/{id}", async Task<Results<Ok, NotFound>> (int idpersona, Persona persona, AppDbContext db) =>
        {
            var affected = await db.Personas
                .Where(model => model.idPersona == idpersona)
                .ExecuteUpdateAsync(setters => setters
                  .SetProperty(m => m.idPersona, persona.idPersona)
                  .SetProperty(m => m.idCiudad, persona.idCiudad)
                  .SetProperty(m => m.Nombre, persona.Nombre)
                  .SetProperty(m => m.Apellido, persona.Apellido)
                  .SetProperty(m => m.TipoDocumento, persona.TipoDocumento)
                  .SetProperty(m => m.NroDocumento, persona.NroDocumento)
                  .SetProperty(m => m.Direccion, persona.Direccion)
                  .SetProperty(m => m.Telefono, persona.Telefono)
                  .SetProperty(m => m.Email, persona.Email)
                  .SetProperty(m => m.Estado, persona.Estado)
                );

            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("UpdatePersona")
        .WithOpenApi();

        group.MapPost("/", async (Persona persona, AppDbContext db) =>
        {
            db.Personas.Add(persona);
            await db.SaveChangesAsync();
            return TypedResults.Created($"/api/Persona/{persona.idPersona}",persona);
        })
        .WithName("CreatePersona")
        .WithOpenApi();

        group.MapDelete("/{id}", async Task<Results<Ok, NotFound>> (int idpersona, AppDbContext db) =>
        {
            var affected = await db.Personas
                .Where(model => model.idPersona == idpersona)
                .ExecuteDeleteAsync();

            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("DeletePersona")
        .WithOpenApi();
    }
}}
