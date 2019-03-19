Public Class HomeController
    Inherits System.Web.Mvc.Controller

    Function Index() As ActionResult

        Return View()
    End Function

    Function CsvReport() As ActionResult

        Dim sb As New StringBuilder()

        sb.AppendLine("CustomerId, Name, OrderCount, SalesTotal")

        Using db As New AppDbContext()

            'get customers with orders
            Dim customersOrders = From c In db.Customers
                                  Group Join o In db.Orders On o.Customer.CustomerId Equals c.CustomerId
                                  Into customList = Group, orderCount = Count, salesTotal = Sum(o.SalesTotal)
                                  Select c.CustomerId, c.Name, orderCount, salesTotal

            For Each c In customersOrders
                sb.AppendLine($"{c.CustomerId}, {c.Name}, {c.orderCount}, {c.salesTotal.ToString("0.##")}")
            Next

        End Using

        Response.ContentType = "text/plain"
        Response.Write(sb.ToString())
        Response.End()

        Return Nothing
    End Function

End Class
