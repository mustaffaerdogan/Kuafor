# 💇‍♂️ KuaforApp - Hair Salon Appointment Management System

A comprehensive appointment management system developed for modern hair salons. Designed to allow customers to easily book appointments, receive AI-powered hairstyle recommendations, and enable salon managers to effectively manage their businesses.

## 📸 Screenshots

*[Project main screenshot will be added here]*

## 🛠️ Technologies Used

### Backend
- **.NET 8.0** - Modern C# framework
- **ASP.NET Core MVC** - Web application architecture
- **Entity Framework Core 8.0.0** - ORM and database management
- **PostgreSQL** - Relational database
- **ASP.NET Core Identity** - Authentication and authorization

### AI Microservice
- **Python 3.x** - For AI processing
- **Flask 2.0.1** - Web framework
- **OpenCV 4.5.3.56** - Image processing
- **NumPy 1.21.2** - Mathematical operations

### Frontend
- **Razor Views** - Server-side rendering
- **Bootstrap** - Responsive UI framework
- **JavaScript/jQuery** - Client-side interaction

## ✨ Features

### 🔐 User Management
- Secure registration and login system
- Role-based authorization (User/Admin)
- Profile management

### 🏪 Salon Management
- Add, edit, and delete salons
- Working hours and days management
- Salon image upload

### 💇‍♀️ Service Management
- Service categories and pricing
- Service duration definition
- Salon-based service management

### 👥 Employee Management
- Employee profiles and working hours
- Service-employee matching
- Working days management

### 📅 Appointment System
- Online appointment booking
- Date and time selection
- Appointment status tracking (Pending/Approved/Rejected)
- Conflict checking

### 🤖 AI-Powered Recommendations
- Photo upload
- Hairstyle recommendations
- Personalized suggestions

### 📊 Admin Dashboard
- System overview
- Appointment management
- User management
- Statistics

## 🚀 Installation Instructions

### Prerequisites
- .NET 8.0 SDK
- PostgreSQL 12+
- Python 3.8+
- Git

### 1. Clone the Project
```bash
git clone https://github.com/mustaffaerdogan/KuaforApp.git
cd KuaforApp
```

### 2. Database Setup
```bash
# Create database in PostgreSQL
createdb kuaforapp

# Update connection string (appsettings.json)
"ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Database=kuaforapp;Username=your_username;Password=your_password"
}
```

### 3. Install .NET Dependencies
```bash
cd KuaforApp
dotnet restore
```

### 4. Run Database Migrations
```bash
dotnet ef database update
```

### 5. Install AI Microservice
```bash
cd PythonAI
pip install -r requirements.txt
```

## ⚡ Commands

### .NET Commands
```bash
# Build the project
dotnet build

# Run in development mode
dotnet run

# Run tests
dotnet test

# Create migration
dotnet ef migrations add [MigrationName]

# Apply migrations
dotnet ef database update
```

### Python AI Service
```bash
# Start AI service
cd PythonAI
python app.py

# Service will run at http://localhost:5000
```

### Database Commands
```bash
# Rollback migrations
dotnet ef database update [PreviousMigrationName]

# Remove migrations
dotnet ef migrations remove
```

## 🔌 API Endpoints

### Authentication
| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/Account/Login` | Login page |
| POST | `/Account/Login` | Login process |
| GET | `/Account/Register` | Registration page |
| POST | `/Account/Register` | Registration process |
| POST | `/Account/Logout` | Logout process |

### Salon Management
| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/Salons` | Salon list |
| GET | `/Salons/Create` | Salon creation form |
| POST | `/Salons/Create` | Create salon |
| GET | `/Salons/Edit/{id}` | Salon edit form |
| POST | `/Salons/Edit/{id}` | Edit salon |
| POST | `/Salons/Delete/{id}` | Delete salon |

### Appointment Management
| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/Appointments` | Appointment list |
| GET | `/Appointments/Create` | Appointment creation form |
| POST | `/Appointments/Create` | Create appointment |
| GET | `/Appointments/Details/{id}` | Appointment details |

### AI Recommendations
| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/AIRecommendation` | AI recommendation page |
| POST | `/api/AIRecommendation/recommendation` | Upload photo and get recommendations |

## 👥 User Roles and Authorization

### User (Regular User)
- ✅ View and edit profile
- ✅ View salon list
- ✅ Book and manage appointments
- ✅ Get AI recommendations
- ❌ Access to admin panel

### Admin (System Administrator)
- ✅ All User permissions
- ✅ Add, edit, delete salons
- ✅ Service management
- ✅ Employee management
- ✅ Approve/reject appointments
- ✅ System settings
- ✅ User management

### Password Requirements
- Minimum 6 characters
- At least 1 uppercase letter
- At least 1 lowercase letter
- At least 1 number
- At least 1 special character

## 🤖 AI Microservice Description

The AI microservice runs as a Flask-based Python application and provides hairstyle recommendations.

### Features
- **Image Processing**: Image analysis with OpenCV
- **Recommendation System**: Recommendations based on face shape and hair type
- **RESTful API**: JSON-based communication
- **File Management**: Secure file upload and cleanup

### Technical Details
- **Port**: 5000
- **Framework**: Flask 2.0.1
- **Image Processing**: OpenCV 4.5.3.56
- **File Formats**: PNG, JPG, JPEG

### API Usage
```bash
curl -X POST -F "photo=@image.jpg" http://localhost:5000/recommendation
```

## 🔧 Environment Variables

### appsettings.json
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Database=kuaforapp;Username=postgres;Password=your_password"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  }
}
```

### Python AI Service
```python
# In app.py
UPLOAD_FOLDER = 'uploads'
ALLOWED_EXTENSIONS = {'png', 'jpg', 'jpeg'}
```

## 📁 File/Folder Structure

```
KuaforApp/
├── Controllers/                 # MVC Controllers
│   ├── HomeController.cs       # Home page
│   ├── AccountController.cs    # Authentication
│   ├── SalonsController.cs     # Salon management
│   ├── ServicesController.cs   # Service management
│   ├── EmployeesController.cs  # Employee management
│   ├── AppointmentsController.cs # Appointment management
│   ├── AIRecommendationController.cs # AI recommendations
│   ├── AdminController.cs      # Admin dashboard
│   └── AdminAppointmentsController.cs # Admin appointment management
├── Models/                     # Entity models
│   ├── ApplicationDbContext.cs # EF DbContext
│   ├── ApplicationUser.cs      # User model
│   ├── Salon.cs               # Salon model
│   ├── Service.cs             # Service model
│   ├── Employee.cs            # Employee model
│   ├── Appointment.cs         # Appointment model
│   ├── AIRecommendation.cs    # AI recommendation model
│   └── UserRole.cs            # Role enum
├── Views/                     # Razor Views
│   ├── Home/                  # Home page views
│   ├── Account/               # Login/register views
│   ├── Salons/                # Salon views
│   ├── Services/              # Service views
│   ├── Employees/             # Employee views
│   ├── Appointments/          # Appointment views
│   ├── AIRecommendation/      # AI recommendation views
│   ├── Admin/                 # Admin views
│   └── Shared/                # Shared layouts
├── ViewModels/                # View models
│   ├── AppointmentViewModel.cs
│   ├── AdminDashboardViewModel.cs
│   ├── AccountViewModels.cs
│   └── EmployeeViewModels.cs
├── Migrations/                # EF Migrations
├── wwwroot/                   # Static files
│   ├── css/                   # Style files
│   ├── js/                    # JavaScript files
│   ├── lib/                   # Libraries (Bootstrap)
│   └── uploads/               # Uploaded files
├── PythonAI/                  # AI microservice
│   ├── app.py                 # Flask application
│   ├── requirements.txt       # Python dependencies
│   └── uploads/               # Temporary file upload
├── Program.cs                 # Application entry point
├── appsettings.json           # Configuration
└── KuaforApp.csproj           # Project file
```

## 🎯 Usage Scenarios

### Customer Scenarios

#### 1. Registration and Login
1. Click "Register" button from home page
2. Enter personal information
3. Email verification
4. Login

#### 2. Booking Appointment
1. View salon list
2. Select desired salon
3. Choose service and employee
4. Set date and time
5. Confirm appointment

#### 3. AI Recommendations
1. Go to "AI Recommendations" page
2. Upload photo
3. View recommendations
4. Save recommendations

### Admin Scenarios

#### 1. Salon Management
1. Login to admin panel
2. Go to "Salons" section
3. Add new salon
4. Edit existing salons

#### 2. Appointment Management
1. Go to "Appointments" section
2. View pending appointments
3. Approve/reject appointments
4. Review appointment details

## 🐛 Known Issues and Development Plan

### Known Issues
- [ ] AI service security vulnerabilities in production
- [ ] File upload size limitations
- [ ] CORS configuration only for localhost
- [ ] Missing email notifications

### Development Plan
- [ ] **v2.0** - Mobile application development
- [ ] **v2.1** - Payment system integration
- [ ] **v2.2** - Advanced reporting system
- [ ] **v2.3** - SMS notifications
- [ ] **v2.4** - Multi-language support
- [ ] **v2.5** - API documentation (Swagger)

### TODO List
- [ ] Add unit tests
- [ ] Write integration tests
- [ ] Performance optimization
- [ ] Security audit
- [ ] Docker containerization
- [ ] CI/CD pipeline setup

## 🤝 Contributing Guide

### Development Environment Setup
1. Fork the project
2. Create local repository
3. Create feature branch: `git checkout -b feature/amazing-feature`
4. Commit your changes: `git commit -m 'Add amazing feature'`
5. Push your branch: `git push origin feature/amazing-feature`
6. Create Pull Request

### Code Standards
- **C#**: Microsoft C# coding conventions
- **JavaScript**: ESLint rules
- **CSS**: BEM methodology
- **Git**: Conventional commits

### Writing Tests
- Use xUnit for unit tests
- Use TestServer for integration tests
- Use Selenium for UI tests

### Pull Request Process
1. Create an issue
2. Create feature branch
3. Write code and test
4. Open Pull Request
5. Wait for code review process
6. Delete branch after merge

## 👨‍💻 Developers

- **Mustafa Erdoğan** - B221210308
- **Ali Aydın** - B221210373

Sakarya University 3rd Year Web Programming semester project.

## 📞 Contact

- **Email**: [your-email@example.com]
- **GitHub**: [https://github.com/mustaffaerdogan/KuaforApp]

---

⭐ Don't forget to star this project if you liked it! 