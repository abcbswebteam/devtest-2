Imports System.Data.Entity

Public Class HomeController
    Inherits System.Web.Mvc.Controller

    Function Index() As ActionResult

        Return View()
    End Function

    Function CsvReport() As ActionResult
        Dim SalesTotalInfo As StringBuilder = New StringBuilder()
        'Response.ContentType = "text/plain"
        'Response.Write("CustomerId, Name, OrderCount, SalesTotal" & vbCr)
        SalesTotalInfo.Append("CustomerId, Name, OrderCount, SalesTotal" & vbCr)
        Using db As New AppDbContext()

            'get customers
            Dim customers = (From c In db.Customers
                             Order By c.CustomerId
                             Select New With {c, c.Orders.Count, c.Orders.Select(Function(x) x.SalesTotal).Sum()}).ToList() ', c.Orders.Count()

            For Each cust In customers
                SalesTotalInfo.Append(String.Format("{0}, {1}, {2}, {3}", cust.c.CustomerId, cust.c.Name, cust.Count, cust.Sum.ToString("0.##") & vbCr))
            Next

        End Using
        Response.ContentType = "text/plain"
        Response.Write(SalesTotalInfo)
        Response.End()

        Return Nothing
    End Function

End Class
