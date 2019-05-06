Public Class HomeController
    Inherits System.Web.Mvc.Controller

    Function Index() As ActionResult

        Return View()
    End Function

    Function CsvReport() As ActionResult

        Response.ContentType = "text/plain"
        Response.Write("CustomerId, Name, OrderCount, SalesTotal" & vbCr)

        Using db As New AppDbContext()

            Dim customers = From c In db.Customers                            Order By c.CustomerId                            Group c By keys = New With {Key c.CustomerId, Key c.Name}                                Into gp = Group, Sales = Sum(c.Orders.Sum(Function(X) X.SalesTotal)), Count = Sum(c.Orders.Count)                            Select New With {                                .customerId = keys.CustomerId,                                .name = keys.Name,                                .orderCount = Count,                                .salesTotal = Sales                                 }            For Each c In customers                Response.Write(String.Format("{0}, {1}, {2}, {3}", c.customerId, c.name, c.orderCount, c.salesTotal.ToString("0.##")) & vbCr)            Next


        End Using

        Response.End()

        Return Nothing
    End Function

End Class
