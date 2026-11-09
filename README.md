# 📋 Task Tracker CLI

A lightweight, colorful command-line interface (CLI) application built in C# (.NET) to easily manage your daily tasks directly from your terminal. Tasks are persistently stored locally using JSON.

---

## ✨ Features

- **Interactive REPL Loop:** Runs continuously until you gracefully exit with `stop`.
- **Color-Coded Interface:** Styled output with distinct colors for prompts, headers, success messages, and system info.
- **Full CRUD Support:** Add, update, delete, and view your tasks seamlessly.
- **Status Tracking:** Keep track of what's on your plate with `todo`, `in-progress`, and `done` statuses.
- **Persistent Storage:** All data is safely saved locally so you never lose your progress.

---

## 🚀 Getting Started

### Prerequisites

- [.NET SDK](https://dotnet.microsoft.com/) installed on your machine.

### Installation & Running

1. Clone the repository or open the project folder in your terminal:

```bash
cd TaskTrackerCli

```

2. Build and run the project:

```bash
dotnet run

```

---

## 📖 Commands Reference

Type `help` anytime while the application is running to view the menu:

| Command               | Description                                | Example                               |
| --------------------- | ------------------------------------------ | ------------------------------------- |
| `help`                | Displays the full interactive command menu | `help`                                |
| `add "task"`          | Adds a new task to your list               | `add "Buy groceries"`                 |
| `update ID "desc"`    | Updates an existing task description       | `update 1 "Buy groceries and snacks"` |
| `delete ID`           | Deletes a task by its ID                   | `delete 1`                            |
| `mark-in-progress ID` | Marks a specific task as in-progress       | `mark-in-progress 1`                  |
| `mark-done ID`        | Marks a specific task as completed         | `mark-done 1`                         |
| `list`                | Lists all existing tasks                   | `list`                                |
| `list todo`           | Filters and lists only pending tasks       | `list todo`                           |
| `list in-progress`    | Filters and lists only tasks in-progress   | `list in-progress`                    |
| `list done`           | Filters and lists only completed tasks     | `list done`                           |
| `stop`                | Exits the application                      | `stop`                                |

---

## 💻 Example Usage

```text
Welcome to task cli, to get type help to get commands, type stop to quit
task-cli help
############### task cli menu #############
add "some task"
update id "some task"
delete id
mark-in-progress id
mark-done id
list
list todo
list in-progress
list done
stop
###########################################
task-cli add "Build a CLI app"
Task Added Successfully!
task-cli list

--- Task List (All) ---
ID: 1 | [Todo] Build a CLI app

task-cli stop
Stopping application

```

---

## 🛠️ Built With

- **C# (.NET)**
- **System.Text.Json** for serialization/persistence.
