select sc.EnglishProductSubcategoryName, p.EnglishProductName
from dbo.DimProductSubcategory sc
inner join dbo.DimProduct p
on sc.ProductSubcategoryKey = p.ProductSubcategoryKey;


Select
--columns from Sales.SalesOrderHeader
oh.SalesOrderID, oh.OrderDate, oh.CustomerID,
--columns from Sales.Customer
c.AccountNumber,
--columns from Sales.SalesOrderDetail
od.SalesOrderDetailID, od.ProductID, od.OrderQty, od.UnitPrice, od.LineTotal,
--columns from Production.Product
p.ProductNumber
from Sales.SalesOrderHeader as oh
inner join Sales.Customer as c on (oh.CustomerID = c.CustomerID)
left join Sales.SalesOrderDetail as od on (oh.SalesOrderID = od.SalesOrderID)
inner join Production.Product as p on (od.ProductID = p.ProductID);


select ProductID, ProductNumber
from Production.Product;


select SalesOrderID, OrderDate, CustomerID
from Sales.SalesOrderHeader;


select SalesOrderID, SalesOrderDetailID, ProductID, OrderQty, UnitPrice, LineTotal
from Sales.SalesOrderDetail
order by SalesOrderID, SalesOrderDetailID, ProductID;


select CustomerID, AccountNumber
from Sales.Customer;


waitfor delay '00:00:059'; --Wait 5 seconds before returning any rows
select CustomerID, AccountNumber
from Sales.Customer;


select CustomerID, AccountNumber
from Sales.Customer
where CustomerID % 7 <> 0; --Remove 1/7 of the rows


