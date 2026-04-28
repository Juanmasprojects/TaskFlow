# TaskFlow

TaskFlow is a console-based task management application built with C# and .NET.

It was designed to practice backend fundamentals, clean architecture and building a structured, user-friendly CLI application.

---

## 🎯 Project Goal

The main goal of this project was to move beyond simple scripts and build a more professional application with:

- Clear separation of concerns
- Scalable structure
- Real-world development practices
- Usable and intuitive command-line experience

This project represents a step towards writing production-ready backend code.

---

🎥 Demo

[![Watch the video](https://img.youtube.com/vi/7lKqEMZOfiY/0.jpg)](https://www.youtube.com/watch?v=7lKqEMZOfiY)

---

## 🚀 Features

- Create, update and delete tasks
- Filter tasks by status (ToDo, InProgress, Done)
- Interactive task list with inline detail view
- JSON persistence using repository pattern
- Color-coded task status for better readability
- Clean navigation and user-friendly CLI flow

---

## 🧱 Architecture

The project follows a layered architecture:

- **Core** → Domain models and enums  
- **Application** → Business logic and services  
- **Infrastructure** → Data persistence (JSON repository)  
- **Console** → User interface (CLI)

This structure allows the business logic to remain independent from the UI and persistence layers.

---

## 🛠️ Technologies

- C#
- .NET
- System.Text.Json
- Git & GitHub

---

## ▶️ How to Run

bash
git clone https://github.com/Juanmasprojects/TaskFlow.git
cd TaskFlow
dotnet run --project TaskFlow.Console

---

🎮 Usage
Create tasks with title and description
View tasks in a compact list
Select a task using its ID to see detailed information
Update task status
Delete tasks with confirmation
Data is automatically saved and loaded from a JSON file

---

📚 What I Learned

This project helped me develop and reinforce key backend skills:

Designing a layered architecture in .NET
Applying the repository pattern for data persistence
Separating business logic from presentation
Structuring a scalable and maintainable project
Improving user experience in a CLI environment
Handling real-world concerns like data persistence and validation
Using Git and GitHub in a structured and incremental workflow

---

## Author

Juan Manuel Blanca