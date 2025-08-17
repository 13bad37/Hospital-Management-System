# Hospital Management System

A simple, interactive console application for managing hospital operations, built in C#. This system allows patients, floor managers, and surgeons to register, log in, and perform their respective tasks within a hospital environment. The app is designed for educational purposes and small-scale hospital management scenarios.

## Features

- **Patient Management**: Register, check in/out, view assigned room, surgeon, and surgery schedule.
- **Staff Management**: Floor managers can assign rooms and schedule surgeries; surgeons can view their schedule and perform surgeries.
- **User Authentication**: Secure login and password management for all users.
- **Input Validation**: Ensures all user data (name, age, email, password, etc.) is valid and unique.
- **Console-Based UI**: Simple menu-driven interface for easy navigation.

## Technologies Used

- C# (.NET Core)
- Console Application
- Object-Oriented Programming

## Getting Started

### Prerequisites
- [.NET SDK](https://dotnet.microsoft.com/download) (Core or 6+)
- A terminal (Linux, macOS, or Windows)

### Cloning the Repository

```bash
git clone https://github.com/yourusername/hospital-management-system.git
cd hospital-management-system
```

### Running the Application

1. If you don't see a `.csproj` file, create one:
   ```bash
   dotnet new console
   ```
2. Build and run the app:
   ```bash
   dotnet run
   ```

### Notes
- All data is stored in memory; restarting the app will reset all users and records.
- The app is designed for learning and demonstration purposes, not for production use.

## Contributing
Pull requests are welcome! For major changes, please open an issue first to discuss what you would like to change.

## License
This project is open source and available under the MIT License.

---

**Project Description:**
A console-based hospital management system for patients and staff, featuring registration, authentication, and role-based actions. Built with C# for educational and demonstration use.

**How to Clone and Run:**
1. Clone the repo:
   ```bash
   git clone https://github.com/yourusername/hospital-management-system.git
   cd hospital-management-system
   ```
2. Make sure you have the .NET SDK installed.
3. Run:
   ```bash
   dotnet run
   ```

Enjoy managing your hospital from the terminal!
