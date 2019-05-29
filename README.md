# Dev Test 2

## Scenario
You have taken up maintenance on an application created by another developer.  
This application has a function that generates a CSV report based on customer order from a SQL database.  
The users are complaining that the CSV report takes 10 seconds or more to generate.  
They would like report to generate in 2 seconds or less.

## Your Task
In your pull request
* Describe in detail the reason that the CSV report is generating slowly.
* Optimize the code to generate the report in under 2 seconds.

## Constraints
* Do not change the database.
* Do not change or remove the NetworkLatencySimulationDbInterceptor.
* Do not use any additional libraries (no additional nuget packages).

## FYI
The first time you start the app it may take a few seconds to generate the database.

## Explanation
In HomeController.cs, the provided code is pulling in a list of Customers. 
    Then it iterates over those customers and pulls in each customer's Orders based on the order's CustomerId
    Then it performs the logic on the Orders' values.
Iterating over the customers and pulling in the orders each time makes a call to the database on each iteration.
    This can be memory intensive because a new query must be generated every time.
    However, this can be avoided by including the Orders in the query when we pull in the Customers.
    Since there is a defined foreign key relationship between the two tables, we can use the Include() method
    to pull in the Customer.Orders property's values. This limits our database call to 1 query.
Thus, this solution should decrease run-time by eliminating unnecessary calls to the database.
