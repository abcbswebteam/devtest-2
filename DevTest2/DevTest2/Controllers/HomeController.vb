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
            Dim customers = From c In db.Customers _
                            .Include("Orders") 'pull in Orders property based on foreign key
                            Order By c.CustomerId
                            Select c

            For Each c In customers
                Response.Write(String.Format("{0}, {1}, ", c.CustomerId, c.Name))

                'get orders
                'Dim orders = From o In db.Orders
                '             Where o.Customer.CustomerId = c.CustomerId
                '             Select o

                Dim orderCount As Integer = 0
                Dim salesTotal As Decimal = 0
                'For Each o In orders
                For Each o In c.Orders
                    orderCount += 1
                    salesTotal += o.SalesTotal
                Next

                Response.Write(String.Format("{0}, {1}", orderCount, salesTotal.ToString("0.##")) & vbCr)
            Next

        End Using

        Response.End()

        Return Nothing
    End Function

End Class
