# TiKiTuTo - Table Soccer Tournament Tool

TiKiTuTo is a modern tournament management software designed to streamline the organization of table soccer tournaments. It replaces the chaos and inefficiency of traditional methods like Excel and paper with a professional, automated solution.

---

## Project Overview


## Features

### Core Functionality
1. **Team and Player Management**
   - Input the number of teams (must be an even number).
   - Input team names (defaults to "Team 1", "Team 2", etc.).
   - Input player names (defaults to "Player 1", "Player 2", etc.).

2. **Tournament Configuration**
   - Input the desired number of preliminary matches per team (must be divisible by the number of teams).
   - Input the number of teams for the next round (must be a power of 2, e.g., 2, 4, 8, etc.).

3. **Tournament Plan**
   - Automatically generate a tournament plan based on the input data.
   - Randomly assign teams to matches in each round.
   - Ensure each team plays the same number of matches in a round and does not face the same opponent multiple times in one round.

4. **Game Rules**
   - **Preliminary Round**: Teams advance based on the number of wins and, in case of a tie, goal difference.
   - **Knockout Rounds**: Teams compete in 2-player groups, winners advance to the next round. Losers in the semifinals play a match for third place.
   - **Final**: Two teams compete, and the winner is crowned champion.

5. **Save and Resume**
   - Automatically save the current tournament progress, allowing users to resume the tournament from where they left off.
   - Support for preloading all teams and players before starting the program, enabling instant tournament plan generation.

---

## Technical Details

### Development Requirements
- **Framework**: .NET 8
- **Architecture**: 
  - The project is split into two separate modules:
    - **Program Logic**: Handles the core functionality of the tournament.
    - **Console Application**: Provides the user interface and interacts with the program logic.
  - The two modules are loosely coupled via project references, allowing for easy replacement of the user interface in the future.

### Data Persistence
- All data is saved in a structured format (e.g., JSON) to ensure easy storage and retrieval.
- The application is designed to automatically load saved data on startup.

---

## Installation and Usage

### Prerequisites
- Install [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0).
- Ensure your development environment supports .NET projects (e.g., Visual Studio, Visual Studio Code).

### Installation
1. Clone the repository:

   git clone https://github.com/YourUsername/TiKiTuTo.git
   cd TiKiTuTo              
2. Build the Project

   Build the application using the following command:
   dotnet build

3. Run the Application

   Start the console application by running:
   dotnet run --project TiKiTuTo.ConsoleApp

4. Follow the Prompts

   The application will guide you through the setup process for the tournament.