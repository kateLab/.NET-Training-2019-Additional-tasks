use [Northwind];

--1
SELECT [CustomerID]
      ,[CompanyName]
  FROM [dbo].[Customers]
  ORDER BY [CustomerID];
  
--2
SELECT TOP 1 [EmployeeID]      
  FROM [dbo].[Employees]
  ORDER BY [HireDate] DESC;
  
--3
SELECT DISTINCT [Country] 
  FROM [dbo].[Customers]
  ORDER BY [Country];
  
--4
SELECT [CompanyName]
  FROM [dbo].[Customers]
  WHERE [City] IN ('Berlin', 'London', 'Madrid', 'Bruxelles', 'Paris')
  ORDER BY [CustomerID] DESC;
  
--5
SELECT [CustomerID]
  FROM [dbo].[Orders]
  WHERE [RequiredDate] >='1996-01-09' and [RequiredDate] < '1996-01-10'
  ORDER BY [CustomerID];

--6
SELECT [ContactName]
  FROM [dbo].[Customers]
  WHERE [Phone] LIKE '(171)%' AND [Phone] LIKE '%77%' AND [Fax] LIKE '(171)%' AND [Fax] LIKE '%50';
  
--7
SELECT [City]
      ,COUNT ([CompanyName]) [CustomerCount]
  FROM [dbo].[Customers]
  WHERE [Country] IN ('Norway','Sweden','Denmark')
  GROUP BY [City];
  
--8
SELECT [Country]
      ,COUNT ([CompanyName]) [CustomerCount]
  FROM [dbo].[Customers]
  GROUP BY [Country]
  HAVING COUNT([CompanyName]) > '9'
  ORDER BY [CustomerCount] DESC;
  
--9
SELECT [CustomerID]
      ,ROUND (AVG ([Freight]), 0) [FreightAvg]
  FROM [dbo].[Orders]
  WHERE [ShipCountry] IN ('UK','Canada') 
  GROUP BY [CustomerID]
  HAVING AVG([Freight]) >= '100' OR AVG([Freight]) < '10'
  ORDER BY [FreightAvg] DESC;
  
--10
SELECT TOP 1 [EmployeeID]
  FROM [dbo].[Employees]
  WHERE [HireDate] < (SELECT MAX ([HireDate]) FROM [dbo].[Employees])
  ORDER BY [EmployeeID] DESC;
  
--11
SELECT  [EmployeeID]
  FROM [dbo].[Employees]
  ORDER BY [EmployeeID] DESC
  OFFSET 1 ROW
  FETCH FIRST 1 ROW ONLY;
  
--12
SELECT [CustomerID]
      ,SUM([Freight]) [FreightSum]
  FROM [dbo].[Orders]
  WHERE [RequiredDate]>='1996-16-07' AND [RequiredDate]<'1996-01-08'
  GROUP BY [CustomerID]
  HAVING SUM([Freight]) >= AVG([Freight])
  ORDER BY [FreightSum];
  
--13
SELECT TOP 3 [CustomerID]
	    ,[ShipCountry]
            ,([Order Details].[Quantity]*[Order Details].[UnitPrice]*(1-[Order Details].[Discount])) [OrderPrice]
  FROM [dbo].[Orders], [dbo].[Order Details]
  GROUP BY [CustomerID],[ShipCountry],[ShippedDate],[Order Details].[Quantity],[Order Details].[UnitPrice],[Order Details].[Discount]
  HAVING ShippedDate >= '1997-01-09' AND ShipCountry IN('Argentina', 'Bolivia', 'Brazil', 'Chile',
														'Colombia','Ecuador','Guyana', 'Paraguay', 
														'Peru','Suriname', 'Uruguay', 'Venezuela' )
  ORDER BY [OrderPrice] DESC;
  
--14
SELECT DISTINCT [CompanyName],
(SELECT MIN([UnitPrice]) FROM [dbo].[Products] WHERE [Suppliers].[SupplierID] = [Products].[SupplierID]) as [MinPrice],
(SELECT MAX([UnitPrice]) FROM [dbo].[Products] WHERE [Suppliers].[SupplierID] = [Products].[SupplierID]) as [MaxPrice]
FROM [dbo].[Products], [dbo].[Suppliers]
GROUP BY [CompanyName], [Products].[SupplierID], [Suppliers].[SupplierID]
ORDER BY [CompanyName];

--15
SELECT [Customers].[CompanyName] as [Customer]
      ,([Employees].[FirstName]+[Employees].[LastName]) [Employee]
  FROM [dbo].[Customers], [dbo].[Employees], [dbo].[Shippers]
  WHERE [Customers].[City] = 'London' AND [Employees].[City] = 'London' AND [Shippers].[CompanyName] = 'Speedy Express';
  
--16
SELECT DISTINCT [ProductName]
	       ,[UnitsInStock]
	       ,[ContactName]
	       ,[Phone]
  FROM [dbo].[Products], [dbo].[Categories], [dbo].[Suppliers]
  WHERE [CategoryName] IN ('Beverages', 'Seafood') AND [Discontinued] = '1' AND [UnitsInStock] < '20'
  ORDER BY [ContactName]