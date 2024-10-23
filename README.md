# honaAPI

The purpose of this project is to create a common API for reuse across different projects.

## **Overview**
This API is built using the **.NET 8.0** framework and serves as a tool to support information management of various data.

## **Features**
- **Create:** Allows users to add new data to the database.
- **Update:** Enables users to modify existing information, such as names, price,..
- **Delete:** Provides functionality to remove data from the database.
- **Search:** Users can search the database for specific entries based on various criteria. This feature allows for advanced filtering to easily find the needed information.
- **Sort:** This feature provides users with the ability to sort data in the database based on specified fields, such as sorting by price, name, or date in ascending or descending order.

## **Technology Stack** ##
- **Backend Framework/Lib:** C# / .NET 8.0 / ASP.NET Web API / JWT
- **Database:** MS SQL

## **Installation**
To run the API locally, ensure you have the following prerequisites installed:

- **.NET 8.0 SDK:** [Download](https://dotnet.microsoft.com/download)
- Your preferred IDE or code editor (e.g., Visual Studio, Visual Studio Code)

Clone this repository to your local machine:

```bash
git clone https://github.com/vuduchoainam/honaapi.git
```
Navigate to the project directory and restore dependencies:
```bash
cd honaapi
dotnet restore
```
Run the application:
```bash
dotnet run
```

The API will be accessible at https://localhost:7177 by default.

## **API Documentation**

Swagger/OpenAPI documentation is available to explore and test the API endpoints. Once the application is running locally, visit the following URL in your web browser:
```bash
https://localhost:7177/swagger/index.html
```

## **Contributing**
Contributions are welcome! If you'd like to contribute to this project, please follow these steps:

- Fork the repository
- Create your feature branch (git checkout -b feature/YourFeature)
- Commit your changes (git commit -am 'Add some feature')
- Push to the branch (git push origin feature/YourFeature)
- Open a pull request
- Please ensure your pull request adheres to the repository's code style and includes relevant tests if applicable.

## **References** ##
- Implementing JWT Authentication in .NET Core: *https://positiwise.com/blog/jwt-authentication-in-net-core*












