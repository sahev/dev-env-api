# dev-env-api

Backend API for provisioning and managing development environments with services like **RabbitMQ**, **PostgreSQL**, **Kafka**, and **Redis**.

> ⚠️ **This project is a required dependency for [sahev/dev-env-web](https://github.com/sahev/dev-env-web)**  
> Ensure this API is running for the frontend interface to function correctly.

---

## 📋 Description

This project provides a RESTful API that facilitates the creation, management, and teardown of isolated development environments. It automates the provisioning of essential services using **Docker** and **Kubernetes**, enabling developers to:

- 🐘 **PostgreSQL**
- 🐇 **RabbitMQ**
- 🔄 **Apache Kafka**
- 🧠 **Redis**

The API is designed to work seamlessly with the `dev-env-web` frontend, offering a complete solution for managing development environments.

---

## 🚀 Technologies Used

- [.NET Core](https://dotnet.microsoft.com/)
- [Docker](https://www.docker.com/)
- [Kubernetes](https://kubernetes.io/)
- [Entity Framework Core](https://docs.microsoft.com/en-us/ef/core/)

---

## ⚙️ Prerequisites

Before running the API, ensure you have the following installed:

- [.NET 6 SDK](https://dotnet.microsoft.com/download/dotnet/6.0)
- [Docker](https://www.docker.com/) installed and running
- [Kubernetes](https://kubernetes.io/) configured (Docker Desktop with Kubernetes enabled)
- [PostgreSQL](https://www.postgresql.org/) configured

---

## 🧪 Running the Project

### 1. Clone this repository

```bash
git clone https://github.com/sahev/dev-env-api.git
cd dev-env-api
```

### 2. Configure PostgreSQL Connection String

In order to connect to PostgreSQL, you need to specify the connection string in the `appsettings.development.json` file. Here's how to configure it:

- Open `appsettings.development.json`
- Under the `"AppSettings.ConnectionStrings"` section, add your PostgreSQL connection string. It should look something like this:

```json
{
  "AppSettings": {
    "ConnectionStrings": {
      "DefaultConnection": "Host=myhost;Port=myport;Database=mydb;Username=myuser;Password=mypassword"
    },
    "UseInMemoryDatabase": false
  }
}
```
