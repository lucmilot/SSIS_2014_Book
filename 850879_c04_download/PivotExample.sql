SELECT p.[Name] as ProductName, p.ProductNumber,
datename(mm, t.TransactionDate) as TransMonth,
sum(t.quantity) as TotQuantity
FROM production.product p
INNER JOIN production.transactionhistory t
ON t.productid = p.productid
WHERE t.transactiondate between '01/01/07' and '12/31/07'
GROUP BY p.[name], p.productnumber, datename(mm,t.transactiondate)
ORDER BY productname, datename(mm, t.transactiondate)
