# GlitchGame

GlitchGame is a .NET 10 Blazor app built around a small multiplayer event game prototype.

The current implementation focuses on a shared event experience where visitors join a district, send chat messages, and contribute points to their district. A live dashboard visualizes district growth based on accumulated points.

## Project context

This project is inspired by **Glitched**, which iss a Swedish LAN and gaming festival. The site highlights LAN, gaming, esports, cosplay, a card festival, community activities, and an event presence at **Elmia in Jönköping**.

This repository is not the Glitched website itself. It is a separate app prototype themed around district-based participation and live event interaction.

Source for event context: https://www.glitched.se

## What the project does

The solution currently contains:

- A **Blazor Web app** for the game UI
- A **shared project** for common types
- A **visitor flow** where a user picks a nickname and district
- A **shared chat feed** for incoming player messages
- A **district score system** that rewards player actions
- A **map visualization** that resizes districts based on score totals
- Simple **JSON file persistence** for messages and points

## Current gameplay flow

### Visitor page
The `/visitor` page is the player-facing entry point.

A visitor can:
- enter a nickname
- choose one of the available districts
- start chatting
- send messages into the shared chat stream

Sending a message currently awards points to the selected district and the sending user.

### Dashboard page
The `/dashboard` page is the spectator-style view.

It shows:
- a live district map
- the shared chat panel
- district growth driven by point totals

The map uses the current district totals to calculate relative sizes for:
- Forge
- Mirage
- Neon
- Pink
- Tokyo
- Tilted

## Solution structure

### `GlitchGame.Web`
The main Blazor application.

Important pieces:
- `Pages/Visitor.razor` - onboarding and message sending
- `Pages/Dashboard.razor` - combined map and chat view
- `Components/Dashboard/Chat.razor` - live message table
- `Components/Dashboard/Map.razor` - district visualization
- `Services/ChatService.cs` - message distribution and persistence
- `Services/PointsTracker.cs` - user and district scoring
- `Services/MapService.cs` - map update notifications
- `Repositories/MessageRepository.cs` - JSON-backed message storage
- `Repositories/PointsRepository.cs` - JSON-backed point storage

### `GlitchGame.Shared`
Shared types used by the app.

Includes:
- `Districts` enum
- `MapUpdate` DTO

## Data storage

The web project persists runtime data to JSON files in the project directory:

- `messages.json` - stored chat messages
- `user_points.json` - per-user score totals
- `district_points.json` - per-district score totals

This keeps the current prototype simple to run locally without a database.

## Scoring rules

`PointsTracker` currently uses these values:

- `UserJoined` = 5
- `GoodMessage` = 10
- `BadMessage` = -15
- `VeryBadMessage` = -25
- `HadMessageRedacted` = -50
- `RightAnswer` = 20
- `WrongAnswer` = 5

At the moment, sending a normal chat message is treated as a `GoodMessage`.

## AI-related code

The solution contains `MessageAnalysisService`, which appears to be an early integration point for AI-based message analysis using OpenAI libraries.

This service is not currently wired into the main chat flow and does not include configured credentials out of the box.

## Getting started

### Prerequisites
- .NET 10 SDK
- Visual Studio 2026 or a compatible `dotnet` CLI environment

### Run the app
The simplest way to explore the current prototype is to run `GlitchGame.Web` as the startup project.

Useful routes:
- `/` - placeholder home page
- `/visitor` - player page
- `/dashboard` - live dashboard

## Current state

This repository is best described as a working prototype.

A few parts are still in progress:
- the home page is still the default template content
- message redaction exists in the service layer but is not exposed as a full moderation workflow
- AI message analysis is present but not integrated

## Tech summary

- .NET 10
- Blazor interactive server components
- JSON file persistence

## Repository goal

GlitchGame is set up to evolve into a live event, party, or LAN-style district competition app where player interaction changes a shared visual game state in real time.

That makes the current codebase a good foundation for adding:
- moderation rules
- real-time multiplayer improvements
- question/answer mechanics
- district leaderboards
- persistent storage
- admin controls
- AI-assisted message classification
