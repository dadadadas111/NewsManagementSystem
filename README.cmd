# FUNewsManagementSystem - News Management System

## 1. Introduction
A News Management System (NMS) is a software application that helps universities and educational institutions efficiently manage, organize, and publish news and content to their website and other channels.

The NMS typically includes features such as:
- Content creation
- Approval workflow
- Scheduling
- Publishing
- Analytics

This helps universities streamline news operations, improve communication with students and the wider community, and better engage with their target audience.

As a developer of the News Management System named **FUNewsManagementSystem**, your tasks include:
- Managing account information
- Managing news articles

**Default Admin Account:**
- Email: `admin@FUNewsManagementSystem.org`
- Password: `@@abc123@@`
- These credentials are stored in `appsettings.json`.

The application must support the following actions (CRUD + Search):
- **Create**: Add new information
- **Read**: View information
- **Update**: Modify information
- **Delete**: Remove information
- **Search**: Find information

This assignment explores creating an **ASP.NET Core Web API OData** with C# and Entity Framework Core. The client application can be a Web Application (ASP.NET Core Web MVC or Razor Pages). An MS SQL Server database will be created to persist the data and will be used for reading and managing data.

## 2. Assignment Objectives
In this assignment, you will:
- Use Visual Studio.NET to create a Web application and ASP.NET Core Web API project (with OData support)
- Perform CRUD actions using Entity Framework Core
- Apply 3-layer architecture to develop the application
- Apply Repository pattern and Singleton pattern in the project
- Add CRUD and searching actions to the Client application with ASP.NET Core Web API
- Apply data type validation for all fields
- Run the project and test the actions of the Client Web app and ASP.NET Core Web API