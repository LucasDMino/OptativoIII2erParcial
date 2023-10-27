using OPTATIVOIII3ERPARCIAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.AspNetCore.Http.HttpResults;
namespace OPTATIVOIII3ERPARCIAL.Models
{
    public class Ciudad
    {
        public int idCiudad { get; set; }
        public string CiudadNombre { get; set; }
        public string Departamento { get; set; }
        public int PostalCode { get; set; }

    }


public static class CiudadEndpoints
{
	public static void MapCiudadEndpoints (this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/Ciudad").WithTags(nameof(Ciudad));

        group.MapGet("/", async (AppDbContext db) =>
        {
            return await db.Ciudades.ToListAsync();
        })
        .WithName("GetAllCiudads")
        .WithOpenApi();

        group.MapGet("/{id}", async Task<Results<Ok<Ciudad>, NotFound>> (int idciudad, AppDbContext db) =>
        {
            return await db.Ciudades.AsNoTracking()
                .FirstOrDefaultAsync(model => model.idCiudad == idciudad)
                is Ciudad model
                    ? TypedResults.Ok(model)
                    : TypedResults.NotFound();
        })
        .WithName("GetCiudadById")
        .WithOpenApi();

        group.MapPut("/{id}", async Task<Results<Ok, NotFound>> (int idciudad, Ciudad ciudad, AppDbContext db) =>
        {
            var affected = await db.Ciudades
                .Where(model => model.idCiudad == idciudad)
                .ExecuteUpdateAsync(setters => setters
                  .SetProperty(m => m.idCiudad, ciudad.idCiudad)
                  .SetProperty(m => m.CiudadNombre, ciudad.CiudadNombre)
                  .SetProperty(m => m.Departamento, ciudad.Departamento)
                  .SetProperty(m => m.PostalCode, ciudad.PostalCode)
                );

            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("UpdateCiudad")
        .WithOpenApi();

        group.MapPost("/", async (Ciudad ciudad, AppDbContext db) =>
        {
            db.Ciudades.Add(ciudad);
            await db.SaveChangesAsync();
            return TypedResults.Created($"/api/Ciudad/{ciudad.idCiudad}",ciudad);
        })
        .WithName("CreateCiudad")
        .WithOpenApi();

        group.MapDelete("/{id}", async Task<Results<Ok, NotFound>> (int idciudad, AppDbContext db) =>
        {
            var affected = await db.Ciudades
                .Where(model => model.idCiudad == idciudad)
                .ExecuteDeleteAsync();

            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("DeleteCiudad")
        .WithOpenApi();
    }
}}
