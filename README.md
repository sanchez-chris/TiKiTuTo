# TiKiTuTo - Table Soccer Tournament Tool

TiKiTuTo is a modern tournament management software designed to streamline the organization of table soccer tournaments. It replaces the chaos and inefficiency of traditional methods like Excel and paper with a professional, automated solution.

## Overview

This project is a .NET 8 console application designed to manage a table soccer tournament. The application is split into two separate projects: one for the program logic and another for the console application interface. This separation aims for loose coupling, allowing for easy replacement of the presentation layer in the future.

## Technical Requirements

- **Project Structure**: 
  - Implemented the program logic and console application in two separate projects.
  - Use project references to provide functionality to the console application.
  
- **Input Requirements**:
  - Read the number of teams (must be an even number).
  - Gather team information:
    - Team names (default to "Team 1", "Team 2", etc. if left blank).
    - Player names (default to "Player 1", "Player 2", etc. if left blank).
  - Specify the number of preliminary round games per team (must be divisible by the number of teams).
  - Determine the number of teams for the next round (1st KO round) (must be a power of 2).

- **Tournament Plan**:
  - Create a tournament schedule based on the input information.
  - Generate a match schedule before each round, assigning teams to matches randomly.
  - Ensure each team plays the same number of games per round and does not play against the same team more than once in a round.

- **Round Details**:
  - **Preliminary Round**: 
    - Once the desired number of games per team is reached, select teams for the next round based on games won. In case of a tie, the team with the better goal difference advances.
  - **KO Rounds**:
    - Form groups of 2 teams, with 1 game per group. The winner advances to the next round. The two losers in the semifinals play for 3rd place.
  - **Final**: 
    - Two teams compete in the final; the winner wins the tournament.

- **Persistence**:
  - Automatically save the current game state to allow resuming after a restart.
  - Enable automatic reading of user inputs required at startup, allowing pre-entry of teams and players before program execution.
  - Use established practices/formats (e.g., JSON) for data persistence.

## Bonus Tasks

1. **Configurable Timer per Game**: 
   - Set a maximum duration for a game. After the timer expires, if the score is tied, the team that scores next wins.
   - Implemented an acoustic signal to end the timer.

## Getting Started

1. **Prerequisites**:
   - .NET 8 SDK installed on your machine.

2. **Project Setup**:
   - Clone the repository.
   - Navigate to the project directory.
   - Build the solution using the .NET CLI or Visual Studio.

3. **Running the Application**:
   - Execute the console application project.
   - Follow the prompts to input tournament details and start managing your table soccer tournament.

## Future Enhancements

- Implement a graphical user interface (GUI) to replace the console application for improved user experience.
- Add more configuration options for tournament rules and settings.

## License

This project is licensed under the MIT License.






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

   git clone https://github.com/sanchez-chris/TiKiTuTo.git
   cd TiKiTuTo
              
2. Build the Project:

   Build the application using the following command:
   dotnet build

3. Run the Application:

   Start the console application by running:
   dotnet run --project TiKiTuTo.ConsoleApp

4. Follow the Prompts:

   The application will guide you through the setup process for the tournament.