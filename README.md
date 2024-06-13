# Include Team

## Team Members

- Enrico Tondato
- Pedro Ribeiro
- Lucas Ribeiro
- Otavio Souza
- Matheus Jardim
- Wellington Rodrigues

## Include App

This project was developed to fulfill the task of creating an application that does not use a database or any other type of data persistence. All application execution is done in the main memory, including session data. When the execution stops, this data is lost.

This proposal was made by Prof. Eduardo Enari, who teaches Data Structures, a course present in the 3rd period/semester at Fatec Prof. Waldomiro May, in Cruzeiro, São Paulo, Brazil.

The API developed was built using the ASP.NET framework tools, specifically in the ASP.NET Web API model. Libraries such as Newtonsoft.Json were used for implementation.

The system is divided into five main folders essential for its operation:

### Controller
Responsible for the execution and manipulation of general information.

### LSharp
The core of the API, where provided routes are decoded, request types are identified, parameters sent are analyzed, and JSON format body keys and values sent or received by the API are read and interpreted.

### Model
Represents the system's creation files, which are:

- User
- Post
- Comment
- BodyContent
- BodyCommentContent
- Friendship
- Message
- BodyMessage
- Notification

### Route
Contains route files with their respective controllers for each route file.

For example, if we have `ChatRoutes`, we will have a `ChatController`. The reverse is also!

### Utils
Auxiliary tools to support validation, such as regex.

---

### FrontEnd

The FrontEnd used with this API in Main Memory was developed by Lucas Ribeiro. You can find it at this link:
[FrontEnd Include App](https://github.com/Lucas-RCS/include-frontend)

---

### Commands

#### Cloning the Repository

To clone the repository to your local machine, use the following command in your terminal:

```bash
git clone https://github.com/Dablio-0/API_CSharp_Include.git
```

#### Building the Application
Navigate to the project directory and use the following command to clear the cache if you have ever run before:
```bash
dotnet clean
```

Build the project with this following command:
```bash
dotnet build
```

And now, run it:
```bash 
dotnet run
```
