Public Class HomeController
    Inherits System.Web.Mvc.Controller

    Function Index() As ActionResult

        Return View()
    End Function

    Function CsvReport() As ActionResult

        Response.ContentType = "text/plain"
        Response.Write("CustomerId, Name, OrderCount, SalesTotal" & vbCr)

        Using db As New AppDbContext()

            ' Create a StringBuilder (potentiallly increase speed)
            Dim responseWrite = New StringBuilder()

            ' Get Aggregate of Orders by CustomerId
            Dim groups =
                From ordByCust In db.Orders
                Group By cust = New With {Key .CustomerId = ordByCust.Customer.CustomerId} Into orderAggregate = Group
                Select New With {
                    .CustomerId = cust.CustomerId,
                    .SalesTotal = orderAggregate.Sum(Function(r) r.SalesTotal),
                    .OrderCount = orderAggregate.Count()
                    }

            ' Join to Customers to get name
            Dim oByCWithName = From oByC In groups
                               Join custs In db.Customers
                                   On oByC.CustomerId Equals custs.CustomerId
                               Select oByC.CustomerId, custs.Name, oByC.OrderCount, oByC.SalesTotal

            ' Append to our StringBuilder from our oByCWithName result set
            For Each ordGroup In oByCWithName
                responseWrite.Append(String.Format("{0}, {1}, {2}, {3}", ordGroup.CustomerId, ordGroup.Name, ordGroup.OrderCount, ordGroup.SalesTotal.ToString("0.##")) & vbCr)
            Next

            ' Write our StringBuilder Data to Response
            Response.Write(responseWrite.ToString())
        End Using

        ' Close Response
        Response.End()

        Return Nothing
    End Function

End Class
