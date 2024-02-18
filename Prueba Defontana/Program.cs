using Microsoft.EntityFrameworkCore;
using Prueba_Defontana.Context;

using (var _context = new VentasContext())
{
    //Obtener la fecha 30 días atrás desde hoy
    var fechaLimite = DateTime.Now.AddDays(-30);

    //Obtener todas las ventas y detalles de ventas de los últimos 30 días
    var ventas = _context.Venta
                         .Where(v => v.Fecha >= fechaLimite)
                         .Include(v => v.VentaDetalles)
                             .ThenInclude(vd => vd.IdProductoNavigation)
                             .ThenInclude(p => p.IdMarcaNavigation)
                         .Include(v => v.IdLocalNavigation)
                         .ToList();

    //Procesamos la información con LINQ

    //Total de ventas de los últimos 30 días
    var totalVentas30Dias = ventas.Sum(v => v.Total);
    var cantidadTotalVentas = ventas.Count;

    //Venta con el monto más alto
    var ventaMontoAlto = ventas.OrderByDescending(v => v.Total).FirstOrDefault();

    //Producto con mayor monto total de ventas
    var productoMayorMonto = ventas
                             .SelectMany(v => v.VentaDetalles)
                             .GroupBy(vd => vd.IdProductoNavigation.Nombre)
                             .Select(g => new { Producto = g.Key, MontoTotal = g.Sum(vd => vd.TotalLinea) })
                             .OrderByDescending(g => g.MontoTotal)
                             .FirstOrDefault();

    //Local con mayor monto de ventas
    var localMayorVentas = ventas
                           .GroupBy(v => v.IdLocalNavigation.Nombre)
                           .Select(g => new { Local = g.Key, MontoTotal = g.Sum(v => v.Total) })
                           .OrderByDescending(g => g.MontoTotal)
                           .FirstOrDefault();

    //Marca con mayor margen de ganancias
    var marcaMayorMargen = ventas
                           .SelectMany(v => v.VentaDetalles)
                           .GroupBy(vd => vd.IdProductoNavigation.IdMarcaNavigation.Nombre)
                           .Select(g => new
                           {
                               Marca = g.Key,
                               Margen = g.Sum(vd => (vd.PrecioUnitario - vd.IdProductoNavigation.CostoUnitario) * vd.Cantidad)
                           })
                           .OrderByDescending(g => g.Margen)
                           .FirstOrDefault();

    //Producto más vendido en cada local
    var productosMasVendidosPorLocal = ventas
    .SelectMany(v => v.VentaDetalles)
    .GroupBy(vd => new { Producto = vd.IdProductoNavigation.Nombre, Local = vd.IdVentaNavigation.IdLocalNavigation.Nombre })
    .Select(g => new
    {
        Local = g.Key.Local,
        Producto = g.Key.Producto,
        Cantidad = g.Sum(vd => vd.Cantidad)
    })
    .OrderBy(g => g.Local).ThenByDescending(g => g.Cantidad)
    .GroupBy(p => p.Local)
    .Select(g => g.First())
    .ToList();

    //Impresión de resultados
    Console.WriteLine($"Total de ventas últimos 30 días: {totalVentas30Dias}");

    Console.WriteLine($"Cantidad de ventas últimos 30 días: {cantidadTotalVentas}");

    if (ventaMontoAlto != null)
        Console.WriteLine($"Venta más alta: {ventaMontoAlto.Total} el día {ventaMontoAlto.Fecha}");
    
    if (productoMayorMonto != null)
        Console.WriteLine($"Producto con mayor monto de ventas: {productoMayorMonto.Producto} con un total de {productoMayorMonto.MontoTotal}");
    
    if (localMayorVentas != null)
        Console.WriteLine($"Local con mayor monto de ventas: {localMayorVentas.Local} con un total de {localMayorVentas.MontoTotal}");

    if (marcaMayorMargen != null)
        Console.WriteLine($"Marca con mayor margen de ganancias: {marcaMayorMargen.Marca} con un margen de {marcaMayorMargen.Margen}");

    Console.WriteLine("---------------------------------------Productos más vendidos de cada local---------------------------------------");
    foreach (var productoMasVendido in productosMasVendidosPorLocal)
    {
        Console.WriteLine($"Producto más vendido en {productoMasVendido.Local}: {productoMasVendido.Producto}");
    }
}
