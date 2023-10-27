using OPTATIVOIII3ERPARCIAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.AspNetCore.Http.HttpResults;
namespace OPTATIVOIII3ERPARCIAL.Models
{
    public class Movimientos
    {
        public int idMovimiento { get; set; }
        public int idCuenta { get; set; }
        public DateTime Fecha_Movimiento { get; set; }
        public string TipoMovimiento { get; set; }
        public decimal SaldoAnterior { get; set; }
        public decimal SaldoActual { get; set; }
        public decimal MontoMovimiento { get; set; }
        public decimal CuentaOrigen { get; set; }
        public decimal CuentaDestino { get; set; }
        public decimal Canal { get; set; }

        // Relación con la tabla Cuentas
        public Cuentas Cuentas { get; set; }

    }


public static class MovimientosEndpoints
{
	public static void MapMovimientosEndpoints (this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/Movimientos").WithTags(nameof(Movimientos));

        group.MapGet("/", async (AppDbContext db) =>
        {
            return await db.Movimientos.ToListAsync();
        })
        .WithName("GetAllMovimientos")
        .WithOpenApi();

        group.MapGet("/{id}", async Task<Results<Ok<Movimientos>, NotFound>> (int idmovimiento, AppDbContext db) =>
        {
            return await db.Movimientos.AsNoTracking()
                .FirstOrDefaultAsync(model => model.idMovimiento == idmovimiento)
                is Movimientos model
                    ? TypedResults.Ok(model)
                    : TypedResults.NotFound();
        })
        .WithName("GetMovimientosById")
        .WithOpenApi();

        group.MapPut("/{id}", async Task<Results<Ok, NotFound>> (int idmovimiento, Movimientos movimientos, AppDbContext db) =>
        {
            var affected = await db.Movimientos
                .Where(model => model.idMovimiento == idmovimiento)
                .ExecuteUpdateAsync(setters => setters
                  .SetProperty(m => m.idMovimiento, movimientos.idMovimiento)
                  .SetProperty(m => m.idCuenta, movimientos.idCuenta)
                  .SetProperty(m => m.Fecha_Movimiento, movimientos.Fecha_Movimiento)
                  .SetProperty(m => m.TipoMovimiento, movimientos.TipoMovimiento)
                  .SetProperty(m => m.SaldoAnterior, movimientos.SaldoAnterior)
                  .SetProperty(m => m.SaldoActual, movimientos.SaldoActual)
                  .SetProperty(m => m.MontoMovimiento, movimientos.MontoMovimiento)
                  .SetProperty(m => m.CuentaOrigen, movimientos.CuentaOrigen)
                  .SetProperty(m => m.CuentaDestino, movimientos.CuentaDestino)
                  .SetProperty(m => m.Canal, movimientos.Canal)
                );

            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("UpdateMovimientos")
        .WithOpenApi();

        group.MapPost("/", async (Movimientos movimientos, AppDbContext db) =>
        {
            db.Movimientos.Add(movimientos);
            await db.SaveChangesAsync();
            return TypedResults.Created($"/api/Movimientos/{movimientos.idMovimiento}",movimientos);
        })
        .WithName("CreateMovimientos")
        .WithOpenApi();

        group.MapDelete("/{id}", async Task<Results<Ok, NotFound>> (int idmovimiento, AppDbContext db) =>
        {
            var affected = await db.Movimientos
                .Where(model => model.idMovimiento == idmovimiento)
                .ExecuteDeleteAsync();

            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("DeleteMovimientos")
        .WithOpenApi();
    }
}}
