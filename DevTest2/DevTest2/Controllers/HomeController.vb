Imports System.Data.Entity

Public Class HomeController
    Inherits System.Web.Mvc.Controller

    Function Index() As ActionResult

        Return View()
    End Function

    Function CsvReport() As ActionResult

        Response.ContentType = "text/plain"
        Response.Write("CustomerId, Name, OrderCount, SalesTotal" & vbCr)

        Using db As New AppDbContext()

            'get customers
            Dim customerOrders = (From customer In db.Customers.Include(Function(o) o.Orders) Select customer).ToList()

            For Each c In customerOrders
                Response.Write(String.Format("{0}, {1}, ", c.CustomerId, c.Name))

                'get orders
                Dim orders = From o In db.Orders
                             Where o.Customer.CustomerId = c.CustomerId
                             Select o

                Dim orderCount As Integer = 0
                Dim salesTotal As Decimal = 0

                orderCount = c.orders.Count
                salesTotal = c.orders.Sum(Function(st) st.salesTotal)

                Response.Write(String.Format("{0}, {1}", orderCount, salesTotal.ToString("0.##")) & vbCr)
            Next

        End Using

        Response.End()

        Return Nothing
    End Function

End Class
