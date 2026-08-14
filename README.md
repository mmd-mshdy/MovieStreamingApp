# 🎬 MovieStreamingApp

A full-stack movie streaming platform built with **ASP.NET Core**, **React**, and **Python**, featuring a personalized movie recommendation system.

The project explores how modern backend architecture, frontend development, and recommendation systems can be combined into a complete application.

---

## ✨ Overview

MovieStreamingApp provides a streaming-platform-style experience where users can browse movies, search the catalog, view movie details, rate and review movies, track their watch progress, and receive personalized movie recommendations.

The project consists of three main components:

* **ASP.NET Core Backend** — business logic, authentication, movie management, reviews, watch history, and API endpoints.
* **React + Vite Frontend** — responsive streaming-style user interface.
* **Python Recommendation Service** — generates personalized movie recommendations based on user preferences and movie information.

The backend was designed using **Clean Architecture, Domain-Driven Design (DDD), CQRS, and the Repository Pattern** to keep the application maintainable and separate business rules from infrastructure concerns.

---

# 🏗️ Architecture

The application follows a layered architecture with responsibilities separated across the system.

```text
┌──────────────────────────────────────────────┐
│                React + Vite                  │
│                  Frontend                    │
└──────────────────────┬───────────────────────┘
                       │ HTTP / REST
                       ▼
┌──────────────────────────────────────────────┐
│               ASP.NET Core API               │
│                                              │
│   Controllers / Presentation Layer           │
│                    │                         │
│                    ▼                         │
│            Application Layer                 │
│           Commands / Queries                 │
│                MediatR                       │
│                    │                         │
│                    ▼                         │
│               Domain Layer                   │
│        Entities / Aggregates / Rules         │
│                    │                         │
│                    ▼                         │
│            Infrastructure Layer              │
│        EF Core / Repositories / Auth         │
└───────────────┬──────────────────┬───────────┘
                │                  │
                ▼                  ▼
         ┌────────────┐    ┌──────────────────┐
         │  Database  │    │ Python           │
         │            │    │ Recommendation   │
         │            │    │ Service          │
         └────────────┘    └──────────────────┘
```

---

# 🧩 Backend Architecture

The backend is built with **ASP.NET Core** and applies several architectural and design patterns.

## Clean Architecture

Responsibilities are separated into layers so that the core business logic does not depend directly on external infrastructure.

```text
MovieStreaming/
│
├── Domain/
│   ├── Aggregates
│   ├── Entities
│   ├── Value Objects
│   └── Domain Logic
│
├── Application/
│   ├── Commands
│   ├── Queries
│   ├── DTOs
│   └── Interfaces
│
├── Infrastructure/
│   ├── Persistence
│   ├── Repositories
│   └── External Services
│
└── API / Presentation
    └── Controllers
```

### Domain-Driven Design

The domain layer represents the core concepts and business rules of the movie streaming platform.

DDD helps keep domain behavior separate from infrastructure and presentation concerns.

### CQRS

The application uses **Command Query Responsibility Segregation (CQRS)** to separate operations that modify state from operations that retrieve data.

```text
Request
   │
   ├── Command ──► Command Handler ──► Domain ──► Repository
   │
   └── Query ────► Query Handler ────► Data Source
```

Commands handle operations such as creating or modifying resources, while queries are responsible for retrieving information.

**MediatR** is used to dispatch commands and queries to their corresponding handlers.

### Repository Pattern

Persistence logic is abstracted through repositories.

This keeps database operations outside the core domain logic and reduces coupling between the application and its persistence implementation.

---

# 🤖 Recommendation System

One of the main features of the project is its **Python-based recommendation system**.

Instead of embedding recommendation logic directly into the ASP.NET Core application, the recommendation engine is implemented as a separate Python service.

```text
User Activity
      │
      ▼
ASP.NET Core Backend
      │
      │ User / Movie Information
      ▼
Python Recommendation Service
      │
      │ Recommended Movies
      ▼
ASP.NET Core API
      │
      ▼
React Frontend
```

This separation allows the recommendation component to evolve independently from the primary backend.

It also demonstrates how a machine-learning/data-driven component can be integrated into a larger software architecture rather than remaining an isolated model or notebook.

---

# 🚀 Features

### 🔐 Authentication

* User registration and login
* JWT-based authentication
* Protected API endpoints
* User-specific functionality

### 🎥 Movie Discovery

* Browse movie catalog
* View movie information
* Search for movies
* Browse movies by genre
* Movie poster support

### ⭐ Ratings & Reviews

Users can interact with movies by:

* submitting ratings
* writing reviews
* viewing existing reviews

These interactions can also provide useful preference information for personalized features.

### ▶️ Continue Watching

The application stores user watch history and playback progress.

Users can return to partially watched movies and see their progress through the **Continue Watching** section.

### 🧠 Personalized Recommendations

The Python recommendation service uses movie and user information to provide personalized movie suggestions.

This keeps recommendation logic independent from the core ASP.NET application and makes it easier to experiment with different recommendation approaches.

---

# 💻 Frontend

The user interface is built with:

* **React**
* **Vite**
* **TypeScript/JavaScript**
* REST API integration

The design takes inspiration from modern movie streaming platforms while remaining an independently implemented UI.

The frontend communicates with the ASP.NET Core API for authentication, movie information, reviews, watch history, and recommendations.

---

# 🛠️ Technology Stack

| Area                    | Technologies               |
| ----------------------- | -------------------------- |
| Backend                 | ASP.NET Core / C#          |
| Architecture            | Clean Architecture         |
| Domain Design           | Domain-Driven Design (DDD) |
| Application Pattern     | CQRS                       |
| Messaging               | MediatR                    |
| Persistence             | Entity Framework Core      |
| Data Access             | Repository Pattern         |
| Authentication          | JWT                        |
| Frontend                | React + Vite               |
| Recommendation System   | Python                     |
| API Communication       | REST                       |
| Documentation / Testing | Swagger / OpenAPI          |

---

# 📂 Repository Structure

```text
MovieStreamingApp/
│
├── MovieStreaming/
│   └── ASP.NET Core backend
│
├── moviestreaming-ui/
│   └── React + Vite frontend
│
├── recommendation-service/
│   └── Python recommendation service
│
├── MovieStreaming.slnx
│
└── README.md
```

Each component has a distinct responsibility, keeping the frontend, core backend, and recommendation functionality separated.

---

# ⚙️ Running the Project

## Prerequisites

Make sure the following are installed:

* .NET SDK
* Node.js
* npm
* Python 3
* Required database server
* Git

Clone the repository:

```bash
git clone https://github.com/mmd-mshdy/MovieStreamingApp.git
cd MovieStreamingApp
```

---

## 1. Start the ASP.NET Core Backend

Navigate to the backend:

```bash
cd MovieStreaming
```

Restore dependencies:

```bash
dotnet restore
```

Run the application:

```bash
dotnet run
```

Once running, the API can be explored through **Swagger/OpenAPI** in development mode.

---

## 2. Start the React Frontend

Open another terminal:

```bash
cd moviestreaming-ui
```

Install dependencies:

```bash
npm install
```

Start the Vite development server:

```bash
npm run dev
```

Vite will display the local frontend URL in the terminal.

---

## 3. Start the Recommendation Service

Open another terminal and navigate to:

```bash
cd recommendation-service
```

Create a virtual environment if desired:

```bash
python -m venv .venv
```

Activate it and install the Python dependencies required by the recommendation service.

Then start the recommendation API using the service's configured Python/FastAPI entry point.

> Exact commands may vary depending on your local Python environment and recommendation-service configuration.

---

# 🔄 Application Flow

A typical authenticated request follows this flow:

```text
React UI
   │
   ▼
ASP.NET Core Controller
   │
   ▼
MediatR
   │
   ▼
Command / Query Handler
   │
   ▼
Domain Logic
   │
   ▼
Repository
   │
   ▼
Database
```

Recommendation requests additionally communicate with the Python recommendation service.

---

# 🎯 Project Goals

The main goal of this project was not simply to reproduce the appearance of a streaming website.

It was built as an opportunity to explore:

* designing a maintainable ASP.NET Core application
* applying Clean Architecture in a real project
* modeling business logic with DDD
* implementing CQRS with MediatR
* separating persistence using repositories
* building a modern React frontend
* implementing user-specific functionality
* developing a recommendation system in Python
* integrating an intelligent service into a full-stack application
* designing communication between different application components

---

# 📚 What I Learned

Building MovieStreamingApp provided practical experience with the interaction between **software architecture and intelligent systems**.

In particular, the project helped develop a better understanding of how architectural patterns such as **DDD, CQRS, Clean Architecture, and Repository Pattern** affect the maintainability of a growing backend.

It also provided practical experience integrating a **Python recommendation component with an ASP.NET Core application and React frontend**, demonstrating how different technologies can work together as parts of one system.

---

# 🔮 Possible Future Improvements

Potential extensions include:

* more advanced collaborative filtering
* hybrid recommendation approaches
* recommendation model evaluation
* improved recommendation explanations
* caching frequently requested recommendations
* richer user profiles and preference modeling
* improved search and filtering
* automated integration tests
* Docker containerization
* CI/CD pipeline
* deployment to a cloud environment

---

# 👨‍💻 Author

**Mohammadreza Mashhadi**

Developed as a full-stack software engineering and recommendation-system project.

---

## ⭐ About the Project

If you are interested in **ASP.NET Core, software architecture, recommendation systems, or full-stack development**, feel free to explore the repository and its implementation.
