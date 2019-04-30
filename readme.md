# Lego Mindstorms (.NET Standard)

> This library is heavily based on @BrianPeek's excellent work <https://github.com/BrianPeek/legoev3>.

Rewrite of the Lego.Ev3 library to support .NET Core

## Project Structure

Source code is in the **/Lego.Mindstorms** directory. A sample console project is in the **/Lego.Mindstorms.Client** directory.

### Prerequisites

- .NET Core 2.2 or later [dot.net](https://dotnet.microsoft.com/)

### Quick start

1. Clone this repository in an empty directory:

    ```
    git clone https://github.com/seesharprun/Lego-Mindstorms .
    ```

1. Connect your brick to your computer using a USB cable. For the test client, connect a large or small motor to **Port A**.

1. Use the .NET CLI to execute the test project:

    ```
    dotnet run --project Lego.Mindstorms.Client/Lego.Mindstorms.Client.csproj
    ```

## Short-term goals (v1.0)

- Update project to .NET Standard
    - Goal to have near universal compatibility
- Change namespace from ``Lego.Ev3.Desktop`` to ``Lego.Mindstorms``
    - Much of the code can work with previous generations of Mindstorm bricks
- Publish project to NuGet
- Stabilize Version 1

## Long-term goals (v2.0 forward)

- Update library to be easier to use for new developers
    - Implement IDisposable for Brick library
    - Use generics
- Modernize documentation using GitHub Pages
- Investigate brick support on linux/osx/etc.
- Take feature requests for Version 2