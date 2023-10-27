using OPTATIVOIII3ERPARCIAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.AspNetCore.Http.HttpResults;
namespace OPTATIVOIII3ERPARCIAL.Models
{
    public class Cuentas
    {
        public int idCuenta { get; set; }
        public int idCliente { get; set; }
        public string NroCuenta { get; set; }
        public DateTime FechaAlta { get; set; }
        public string TipoCuenta { get; set; }
        public string Estado { get; set; }
        public decimal Saldo { get; set; }
        public string Nro_Contrato { get; set; }
        public decimal CostoMantenimiento { get; set; }
        public string PromedioAcreditacion { get; set; }
        public string Moneda { get; set; }

        // Relación con la tabla Cliente
        public Cliente Cliente { get; set; }

    }


public static class CuentasEndpoints
{
	public static void MapCuentasEndpoints (this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/Cuentas").WithTags(nameof(Cuentas));

        group.MapGet("/", async (AppDbContext db) =>
        {
            return await db.Cuentas.ToListAsync();
        })
        .WithName("GetAllCuentas")
        .WithOpenApi();

        group.MapGet("/{id}", async Task<Results<Ok<Cuentas>, NotFound>> (int idcuenta, AppDbContext db) =>
        {
            return await db.Cuentas.AsNoTracking()
                .FirstOrDefaultAsync(model => model.idCuenta == idcuenta)
                is Cuentas model
                    ? TypedResults.Ok(model)
                    : TypedResults.NotFound();
        })
        .WithName("GetCuentasById")
        .WithOpenApi();

        group.MapPut("/{id}", async Task<Results<Ok, NotFound>> (int idcuenta, Cuentas cuentas, AppDbContext db) =>
        {
            var affected = await db.Cuentas
                .Where(model => model.idCuenta == idcuenta)
                .ExecuteUpdateAsync(setters => setters
                  .SetProperty(m => m.idCuenta, cuentas.idCuenta)
                  .SetProperty(m => m.idCliente, cuentas.idCliente)
                  .SetProperty(m => m.NroCuenta, cuentas.NroCuenta)
                  .SetProperty(m => m.FechaAlta, cuentas.FechaAlta)
                  .SetProperty(m => m.TipoCuenta, cuentas.TipoCuenta)
                  .SetProperty(m => m.Estado, cuentas.Estado)
                  .SetProperty(m => m.Saldo, cuentas.Saldo)
                  .SetProperty(m => m.Nro_Contrato, cuentas.Nro_Contrato)
                  .SetProperty(m => m.CostoMantenimiento, cuentas.CostoMantenimiento)
                  .SetProperty(m => m.PromedioAcreditacion, cuentas.PromedioAcreditacion)
                  .SetProperty(m => m.Moneda, cuentas.Moneda)
                );

            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("UpdateCuentas")
        .WithOpenApi();

        group.MapPost("/", async (Cuentas cuentas, AppDbContext db) =>
        {
            db.Cuentas.Add(cuentas);
            await db.SaveChangesAsync();
            return TypedResults.Created($"/api/Cuentas/{cuentas.idCuenta}",cuentas);
        })
        .WithName("CreateCuentas")
        .WithOpenApi();

        group.MapDelete("/{id}", async Task<Results<Ok, NotFound>> (int idcuenta, AppDbContext db) =>
        {
            var affected = await db.Cuentas
                .Where(model => model.idCuenta == idcuenta)
                .ExecuteDeleteAsync();

            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("DeleteCuentas")
        .WithOpenApi();
    }
}}
