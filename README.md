# Bank Application

A .NET 8.0 based banking application that provides comprehensive banking operations through secure HTTP endpoints with JWT authentication.

## 🚀 Getting Started

### Prerequisites

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) or later
- Your preferred IDE (Visual Studio, VS Code, or JetBrains Rider)
- [Git](https://git-scm.com/)

### Installation

1. Clone the repository:
   ```bash
   git clone <repository-url>
   cd DotnetPractice
   ```

2. Restore dependencies:
   ```bash
   dotnet restore
   ```

3. Configure application settings:
   - Copy `appsettings.Development.json.template` to `appsettings.Development.json`
   - Update the configuration values as needed

4. Build the solution:
   ```bash
   dotnet build Bank
   ```

5. Run the application:
   ```bash
   dotnet run --launch-profile https --project Bank
   ```

## 🏗️ Project Structure

```
Bank/
├── Controllers/        # API controllers
├── DTOs/              # Data Transfer Objects
├── Models/            # Domain models
├── Services/          # Business logic services
├── Properties/        # Project properties
├── appsettings.json   # Configuration
└── Program.cs         # Application entry point
```

## 🔧 Configuration

Update `appsettings.Development.json` with your configuration:

```json
{
  "TokenServiceSettings": {
    "Url": "https://your-auth-service.com/token",
    "User": "your-username",
    "Password": "your-password"
  },
  "ServiceSettings": {
    "AccountServiceUrl": "https://your-account-service.com/api",
    "HistoryServiceUrl": "https://your-credit-history-service.com/api",
    "DataCreditServiceUrl": "https://your-data-credit-service.com/api"
  }
}
```

## 🧪 Testing

Use the included `Bank.http` file with REST Client extension to test the API endpoints.

## 📄 API Endpoints

Refer to the `Bank.http` file for available API endpoints and example requests.

## Testing

Run the test suite with:
```bash
dotnet test
```

## Contributing

1. Fork the repository
2. Create your feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add some amazing feature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.
