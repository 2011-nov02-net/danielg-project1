# Dumb Mcdonald's
## Project Description
Dumb McDonald's is an ASP.NET MVC Web Application that mimics an online ordering experience from McDonald's. Based off of "Dumb Starbucks," a popular company famous for it's similar name and branding to Starbucks.
[Project Requirements](https://github.com/2011-nov02-net/trainer-code/wiki/Project-1-requirements)

## Technologies Used
- ASP.NET Core 5.0
- Azure App Service
- Azure Pipelines
- SQL Server
- Microsoft Entity Framework 5.0.0 
- Serilog.AspNetCore 3.4.0
- SonarCloud

## Features
  - Managers can:
    - View separate store locations 
    - View orders placed at those locations
    - View specific details of those orders
  - Customers can:
    - Create accounts 
    - To sign in 
    - To place orders to locations choosing various amounts of various products
    - To view those orders placed as well as specifc details of those orders

- Persistent Data
- Logging 
- Input Validation
- Continuous Deployment
#### To - Do
 - Testing Database Methods
 - Split CSS up into .css files to promote reusability
 - Make a couple of routing paths more logical

## Getting Started
1) Run command: git clone https://github.com/2011-nov02-net/danielg-project1
2) Use database ".sql" file in repo to set up database with some initial data
3) Make sure you use your connection string to access your database 
4) Run in VS or VSCode

## Usage
[Click this link to use Dumb McDonald's yourself!](https://dumb-mcdonalds-webapp.azurewebsites.net/)
- Here you can see the home page, two main paths you can take as previously mentioned.
![](/Screenshots/HomeScreen.png)
- Here you can see the table of users, where you can search for your account or pick one without searching.
![](/Screenshots/CustomersSignInTable.png)
- This is what a customer sees when he or she logs in. Past orders and a way to create a new order. Take note of the CSS that I wrote specifically for the tables in this application.
![](/Screenshots/CustomersHome.png)
- Here is what it looks like to create an order. The stores stock is iterated over and the customer is given the option to choose amounts of products they want. Also pictured is one example of validation. The user tried to create an order with a negative amounts of products.
![](/Screenshots/PlaceOrder.png)
- Here is what it looks like when the Manager picks a location to view. Notice the table is similar to the customer table, but the location is replaced with the customer, because the manager picked the location but would also like to know who placed an order.
![](/Screenshots/ManagerViewLocation.png)
## License
Dumb McDonald's uses the [MIT License](https://github.com/git/git-scm.com/blob/master/MIT-LICENSE.txt)
