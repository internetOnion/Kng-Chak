# Kng Chak

Kng Chak is a 2D mobile arcade game. A space fire brigade defends its home planet from falling rocks: fly the rocket, push rocks with its flame, break them before they hit, and chain combos. One run is an "emergency call" of 12 waves that ends with the Inferno boss.

- **Platform:** phones in landscape (main target), PC for testing
- **Engine:** Unity 6, Universal 2D (URP)
- **Team:** 5 people (see [CONTRIBUTING](CONTRIBUTING.md#areas-and-owners))

## Requirements

| Tool        | Version / notes                                                                        |
| ----------- | -------------------------------------------------------------------------------------- |
| Unity       | **6000.3.8f1** exactly (install it from Unity Hub, with Android and iOS Build Support) |
| Git         | any recent version                                                                     |
| Git LFS     | needed: all images and sounds are stored with LFS                                      |
| Code editor | VS Code with the **Unity** extension (or Rider)                                        |
| .NET SDK    | needed by VS Code for code hints                                                       |

Using a different Unity version will upgrade the project files and break them for everyone else. If Hub asks to change the version, say no.

## Open the project

1. Do the **first-day setup** in [CONTRIBUTING.md](CONTRIBUTING.md#first-day-setup-every-computer) first (Git LFS, Smart Merge).
2. Clone the repo:
   ```bash
   git clone https://github.com/internetOnion/Kng-Chak.git
   ```
3. In Unity Hub: **Add → Add project from disk** and pick the `Kng-Chak` folder. Open it with **6000.3.8f1**.
4. The first open takes a few minutes while Unity builds its `Library` cache.
5. Open the scene `Assets/_Game/_Scenes/Arena` and press Play.

## Folder map

Everything we make lives in `Assets/_Game/`. Don't put files directly in `Assets/`.

| Folder            | What goes in it                                                             |
| ----------------- | --------------------------------------------------------------------------- |
| `Art/`            | Sprites, split into `Backgrounds`, `Planets`, `Rocks`, `Ships`, `UI`, `VFX` |
| `Data/`           | Data assets (ScriptableObjects): `ArenaConfig`, `Rocks/`, `Districts/`      |
| `Fonts/`          | Font files and TextMeshPro font assets                                      |
| `Prefabs/`        | Every object in the game is a prefab (Planet, Ship, rocks, …)               |
| `_Scenes/`        | `Arena` (the shared game scene) and `Sandbox/` (one test scene per person)  |
| `Scripts/`        | All C# code (`KngChak.Runtime` assembly), split by area: `Arena`, `Data`, … |
| `Scripts/Editor/` | Editor-only tools (`KngChak.Editor` assembly, never in the phone build)     |
| `Shaders/`        | Shader Graphs and materials for effects (flame, rings, glow)                |

Outside `_Game`: `Assets/Settings/` holds the URP and Input System settings (don't move it), and `Docs/` at the repo root holds the team docs.

## Docs and links

- [CONTRIBUTING.md](CONTRIBUTING.md): setup, workflow and team rules. **Read it before your first commit.**
- [Art guide](Docs/kng-chak-art-guide.md): style, palette, sizes, file names, sorting layers
- [Design doc (Google Doc)](https://docs.google.com/document/d/1-vzbE6LcHQEDjmcx1szRkzakfpakmHI4E8A8iBscnC0/edit): the full game design
- [Web prototype](https://claude.ai/artifact/Lw1S4YxvqTmsVDobWLrnEz): play the design before we build it in Unity
