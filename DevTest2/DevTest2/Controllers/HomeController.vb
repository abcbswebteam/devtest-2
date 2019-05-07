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
        'Instead of doing Lazy Loading, we will do Eager Loading where we will get Customer and related Order data in one db call.
        'We can loop through for each customer, but now all the data are in system memory so we can avoid db call.
        Dim customerOrder = From c In db.Customers
                            join o in db.Orders on o.CustomerID = c.CustomerID
                            Order By c.CustomerId
                            Select c
            'Declaring a variable to store the previouscustomerID to avoid the loop for same Id
            Dim previousCustomerID As Integer

            For Each c In customerOrder
            'Checking if it is a different customerid then only go further
            If previousCustomerID <> c.CustomerID Then
                Response.Write(String.Format("{0}, {1}, ", c.CustomerId, c.Name))

                'get orders
                'get all the order for a particular customer
                Dim orders = customers.Where(Function(x) x.CustomerId = cutomerID).ToList()

                Dim orderCount As Integer = 0  
                Dim salesTotal As Decimal = 0
                For Each o In orders
                    orderCount += 1
                    salesTotal += o.SalesTotal
                Next
                Response.Write(String.Format("{0}, {1}", orderCount, salesTotal.ToString("0.##")) & vbCr)
                'assigning the customerid to variable
                previousCustomerID = customerID
                
                EndIF
            Next

        End Using

        Response.End()

        Return Nothing
    End Function

End Class
