Imports System.Linq
Public Class HomeController
    Inherits System.Web.Mvc.Controller

    Function Index() As ActionResult

        Return View()
    End Function

    Function CsvReport() As ActionResult

        Response.ContentType = "text/plain"
        Response.Write("CustomerId, Name, OrderCount, SalesTotal" & vbCr)

        Using db As New AppDbContext()

            Dim result = From c In db.Customers
                         From o In db.Orders.Where(Function(o) o.Customer.CustomerId = c.CustomerId).DefaultIfEmpty()
                         Order By c.CustomerId
                         Select New MyModel With {.CustomerID = c.CustomerId,
                                                  .Name = c.Name,
                                                  .OrderID = o.OrderId,
                                                  .TotalSale = o.SalesTotal
                                                 }


            Dim summaryResult = From cust In result
                                Group cust By keys = New With {Key cust.CustomerID, Key cust.Name}
                                Into Group
                                Select New With
                                {
                                      keys.CustomerID, keys.Name,
                                     .TotalSales = Group.Sum(Function(x) x.TotalSale),
                                     .OrderCount = Group.Count()
                                }

            For Each item In summaryResult
                Console.WriteLine(item)
                Response.Write(String.Format("{0}, {1},{2},{3} ", item.CustomerID, item.Name, item.OrderCount, item.TotalSales.ToString()) & vbCr)
            Next


        End Using

        Response.End()

        Return Nothing
    End Function

End Class

Public Class MyModel

    Public Property CustomerID As String
    Public Property Name As String
    'Public Property OrderID As Integer?

    Public Property OrderID As Integer?

    Public Property TotalSale As Decimal?
    'Public Property OrderDate As Date?

End Class

