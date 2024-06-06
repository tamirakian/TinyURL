# TinyURLApp

TinyURLApp is a URL shortening service built using .NET Core and MongoDB. It allows users to generate short URLs for long URLs and redirect users to the original target page using the short URL.

## Technologies Used

- .NET Core (ASP.NET) for server-side code
- MongoDB for database storage

## Features

- **URL Shortening**: Generate short URLs for long URLs.
- **Redirection**: Redirect users to the original target page using the short URL.
- **Consistent Short URLs**: Sending the same long URL will always result in the same short URL being returned.
- **Collision Prevention**: Prevent different long URLs from receiving the same short URL or make such collisions extremely unlikely.

## Getting Started

To get started with the project:

1. Install .NET Core SDK.
2. Install MongoDB on your machine.
3. Clone this repository.
4. Configure the MongoDB connection string in the `appsettings.json` file.
5. Build and run the project using Visual Studio or the command line.
6. Access the endpoints to generate and extract short URLs.