# .NET gRPC Sample

This repository contains a simple yet comprehensive example of using **gRPC** with **.NET**. It demonstrates how to build and run a basic gRPC server and client using .NET 9 with Protobuf for defining service contracts.

## 📌 Features

- ✅ Server and client implementation in .NET
- ✅ Strongly-typed gRPC communication using Protocol Buffers
- ✅ gRPC JSON transcoding

## 🧰 Technologies Used

- .NET 9
- gRPC
- Protocol Buffers (.proto files)
- JSON transcoding

## 🏗️ Project Structure

ToDoGrpc/
├── ToDoGrpc.Server/ # gRPC service implementation
├── ToDoGrpc.Client/ # Console gRPC client to consume the service

## 🚀 Getting Started

### Clone the repository

```bash
git clone https://github.com/qandeel-codes/dotnet-grpc-sample.git
cd dotnet-grpc-sample
```

### Database setup

```bash
cd GrpcSample.Server
dotnet ef database update
```

### Run the Server

```bash
dotnet run
```

### Run the Client

In a new terminal:

```bash
cd GrpcSample.Client
dotnet run
```

You should see client-server communication in the terminal output.

## 📄 License

This project is open-source and available under the [MIT License](https://en.wikipedia.org/wiki/MIT_License).

## 🙌 Acknowledgements

- Official [gRPC for .NET documentation](https://learn.microsoft.com/en-us/aspnet/core/grpc/)
- Protobuf [Language Guide](https://protobuf.dev/programming-guides/)
- Official [gRPC JSON transcoding](https://learn.microsoft.com/en-us/aspnet/core/grpc/json-transcoding?view=aspnetcore-9.0)
