Public Class HomeController
    Inherits System.Web.Mvc.Controller

    Function Index() As ActionResult

        Return View()
    End Function

    Function CsvReport() As ActionResult

        Response.ContentType = "text/plain"
        Response.Write("CustomerId, Name, OrderCount, SalesTotal" & vbCr)

        Using db As New AppDbContext()

            Dim sortedCustomer = db.Customers.OrderBy(Function(item) item.CustomerId)
            Dim InnerjoinCustomerAndOrder = sortedCustomer.Join(db.Orders,
                                    Function(customer) customer.CustomerId,
                                    Function(order) order.Customer.CustomerId,
                                    Function(customer, order) New With
                                    {
                                        .orderCount = db.Orders.Where(Function(t) t.Customer.CustomerId = customer.CustomerId).Count(Function(k) k.Customer.CustomerId),
                                        .customerId = customer.CustomerId,
                                        .salesTotal = db.Orders.Where(Function(t) t.Customer.CustomerId = customer.CustomerId).Sum(Function(k) k.SalesTotal),
                                        .customerName = customer.Name
                                    })

            Dim salesTotal As Decimal = 0
            Dim orderCount As Integer = 0
            Dim Name As String = String.Empty
            Dim customerId As Integer = 0
            For Each obj In InnerjoinCustomerAndOrder.Distinct

                customerId = obj.customerId
                Name = obj.customerName
                orderCount = obj.orderCount
                salesTotal = obj.salesTotal
                Response.Write(String.Format("{0}, {1} {2} {3} ", customerId, Name, orderCount, salesTotal.ToString("0.##")) & vbCr)
            Next

        End Using

        Response.End()

        Return Nothing
    End Function

End Class
