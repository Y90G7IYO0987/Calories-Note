# Calories Note 🍏

**Calories Note** - This is a Telegram bot for convenient calorie counting, creating a food diary and tracking statistics. The project is written in C# using the principles of a clean architecture.

## 🚀 Main features

*   **Product Search:** Quick product search from the database.
*   **Food Diary:** Adding meals per day.
*   **Statistics:** View calories, protein, fat, and carbohydrates consumed.
*   **Standards:** Calculation of individual daily calorie intake.
*   **User-friendly interface:** Intuitive communication via Telegram commands.

## 🛠️ Technology stack

*   **Language:** C# (.NET 8)
*   **Data Base:** PostgreSQL
*   **ORM:** Entity Framework Core
*   **The Bot Library:** Telegram.Bot
*   **Architecture:** Clean Architecture (Core / Infrastructure / Presentation)
*   **Infrastructure:** Docker, Docker Compose

## 🏗️ Project Architecture

The project is divided into layers to ensure the independence of business logic from external frameworks.:

*   `Calories_Note` (Presentation): Entry point, processing of Telegram commands.
*   `Calories_Note_Core` (Domain): Pure business logic, interfaces, models.
*   `Calories_Note_Infrastructure` (Data): Working with PostgreSQL, EF Core, and implementing repositories.

## ⚙️ How to run locally

### Requirements
*   Installed [.NET 8 SDK](https://dotnet.microsoft.com/download)
*   Installed [Docker](https://www.docker.com/)
*   A registered Telegram bot (token from [@BotFather](https://t.me/BotFather))

### Installation
1. Clone the repository:
   ```bash
   git clone https://github.com/Y90G7IYO0987/Calories_Note.git
   cd Calories_Note
   ```
2. Create a file .env in the root folder or configure docker-compose.yml. Add your token and the database connection string there.:
   ```bash
   BOT_TOKEN=ваш_токен_здесь
   DB_CONNECTION=Host=localhost;Database=calories;Username=postgres;Password=yourpassword
   ```
3. Run the project via Docker Compose:
   ```bash
   docker-compose up -d --build
   ```
4. (Alternatively, you can run without Docker through Visual Studio by configuring appsettings.json in the Presentation layer)

🤝 Contacts
Author: ACommunity (AtsProfi)

GitHub: @Y90G7IYO0987
