# Banco Kata

A .NET implementation of a simple banking system, created as a coding kata to practice clean code and TDD principles.

## Features

- Account management (create, view balance)
- Deposit and withdrawal operations
- Transaction history
- Basic validation rules

## Prerequisites

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) or later

## Getting Started

1. Clone the repository:
   ```bash
   git clone https://github.com/guillercp93/KataDotnetProject.git
   cd KataDotnetProject
   ```

2. Restore dependencies:
   ```bash
   dotnet restore
   ```

3. Run the tests:
   ```bash
   dotnet test
   ```

4. Run the application:
   ```bash
   dotnet run
   ```

## Project Structure

- `BancoKata/` - Main project directory
  - `Models/` - Domain models
  - `Services/` - Business logic
  - `Interfaces/` - Service contracts
  - `Program.cs` - Application entry point
- `BancoKata.Tests/` - Unit tests
  - `UnitTest1.cs` - Example test file

## Running Tests

To run the test suite:

```bash
dotnet test
```

## Contributing

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## Acknowledgments

- Inspired by the Bank Account Kata
- Built with .NET 8.0
