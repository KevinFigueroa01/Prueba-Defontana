--Total de Ventas de los Últimos 30 Días
SELECT 
    SUM(Total) AS MontoTotal,
    COUNT(*) AS CantidadTotal
FROM [dbo].[Venta]
WHERE Fecha >= DATEADD(DAY, -30, GETDATE());

--Venta con el Monto Más Alto en los Últimos 30 Días
SELECT TOP 1
    Fecha,
    Total AS Monto
FROM [dbo].[Venta]
WHERE Fecha >= DATEADD(DAY, -30, GETDATE())
ORDER BY Total DESC;

--Producto con Mayor Monto Total de Ventas
SELECT TOP 1
    p.Nombre AS Producto,
    SUM(vd.TotalLinea) AS MontoTotal
FROM [dbo].[VentaDetalle] vd
JOIN [dbo].[Producto] p ON vd.ID_Producto = p.ID_Producto
GROUP BY p.Nombre
ORDER BY SUM(vd.TotalLinea) DESC;

--Local con Mayor Monto de Ventas
SELECT TOP 1
    l.Nombre AS Local,
    SUM(v.Total) AS MontoTotal
FROM [dbo].[Venta] v
JOIN [dbo].[Local] l ON v.ID_Local = l.ID_Local
GROUP BY l.Nombre
ORDER BY SUM(v.Total) DESC;

--Marca con Mayor Margen de Ganancias
SELECT TOP 1
    m.Nombre AS Marca,
    SUM((vd.Precio_Unitario - p.Costo_Unitario) * vd.Cantidad) AS Margen
FROM [dbo].[VentaDetalle] vd
JOIN [dbo].[Producto] p ON vd.ID_Producto = p.ID_Producto
JOIN [dbo].[Marca] m ON p.ID_Marca = m.ID_Marca
GROUP BY m.Nombre
ORDER BY SUM((vd.Precio_Unitario - p.Costo_Unitario) * vd.Cantidad) DESC;

--Producto Más Vendido en Cada Local
;WITH CTE_Productos AS (
    SELECT
        l.Nombre AS Local,
        p.Nombre AS Producto,
        SUM(vd.Cantidad) AS TotalVendido
    FROM [dbo].[VentaDetalle] vd
    JOIN [dbo].[Venta] v ON vd.ID_Venta = v.ID_Venta
    JOIN [dbo].[Local] l ON v.ID_Local = l.ID_Local
    JOIN [dbo].[Producto] p ON vd.ID_Producto = p.ID_Producto
    GROUP BY l.Nombre, p.Nombre
), CTE_Ranking AS (
    SELECT *,
           RANK() OVER (PARTITION BY Local ORDER BY TotalVendido DESC) AS 'Ranking'
    FROM CTE_Productos
)
SELECT Local, Producto, TotalVendido
FROM CTE_Ranking
WHERE Ranking = 1;



