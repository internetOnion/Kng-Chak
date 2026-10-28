# Contributing to Kng Chak

How 5 people work on one Unity project without breaking each other's work. Read it once fully, then use it as a checklist.

## First-day setup (every computer)

Do this once on each computer you work on. These settings live on your machine, not in the repo, so nobody can do them for you.

1. **Install Unity 6000.3.8f1** from Unity Hub, with **Android Build Support** and **iOS Build Support** (iOS only works on a Mac).
2. **Turn on Git LFS.** Without it, images and sounds download as tiny text files and show up as missing in Unity.
   ```bash
   git lfs install
   ```
3. **Clone the repo** (after step 2):
   ```bash
   git clone https://github.com/internetOnion/Kng-Chak.git
   ```
4. **Connect Unity Smart Merge.** It lets Git merge scenes and prefabs without breaking them. Run these inside the `Kng-Chak` folder.

   macOS:

   ```bash
   git config merge.unityyamlmerge.name "Unity SmartMerge"
   git config merge.unityyamlmerge.driver '"/Applications/Unity/Hub/Editor/6000.3.8f1/Unity.app/Contents/Helpers/UnityYAMLMerge" merge -p %O %B %A %A'
   ```

   Windows (Git Bash):

   ```bash
   git config merge.unityyamlmerge.name "Unity SmartMerge"
   git config merge.unityyamlmerge.driver '"C:/Program Files/Unity/Hub/Editor/6000.3.8f1/Editor/Data/Tools/UnityYAMLMerge.exe" merge -p %O %B %A %A'
   ```

   If Unity is installed somewhere else, change the path to match.

5. **Set up the code editor.**
   - Install the [.NET SDK](https://dotnet.microsoft.com/download) and **VS Code** with the **Unity** extension (VS Code suggests it when you open the project).
   - In Unity: **Edit (Unity on Mac) → Preferences → External Tools → External Script Editor → Visual Studio Code**.
   - If VS Code says it can't start `dotnet`: open **Preferences: Open User Settings (JSON)** and add `"dotnet.dotnetPath": "/usr/local/share/dotnet"` (macOS). Then quit VS Code fully and reopen it from Unity.
6. **Make your sandbox scene.** Open `Assets/_Game/_Scenes/Arena`, then **File → Save As** into `_Scenes/Sandbox/` as `Sandbox_<YourName>`. Open `Arena` again afterwards.

## Daily workflow

`main` is protected: nobody pushes to it directly. Every change goes through a branch and a pull request (PR) with 1 approval.

1. **Start from an up-to-date `main`:**
   ```bash
   git switch main
   git pull
   ```
2. **Make a branch** named `type/issue-short-description`:
   - types: `feature`, `fix`, `art`, `docs`, `setup`
   - examples: `feature/12-touch-stick`, `fix/18-rock-falls-through-planet`, `art/7-iron-rock-sprites`
   ```bash
   git switch -c feature/12-touch-stick
   ```
3. **Work in small steps.** Test in your sandbox scene. In Unity, **File → Save Project** before you commit.
4. **Check what changed** before every commit. If something you changed isn't listed, it isn't saved yet.
   ```bash
   git status
   ```
5. **Commit and push:**
   ```bash
   git add -A
   git commit -m "Touch stick moves the ship"
   git push -u origin feature/12-touch-stick
   ```
6. **Open a PR:** `gh pr create --fill`, or use the GitHub website.
   - Write what changed and why in the description.
   - Add `Closes #12` so the issue closes when it merges.
   - Ask a reviewer, ideally from **another area**, so knowledge spreads.
7. **After approval:** press **Merge** on GitHub, then clean up:
   ```bash
   git switch main
   git pull
   git branch -d feature/12-touch-stick
   ```
8. **More changes on an open PR?** Just commit and push to the same branch. The PR updates by itself.

Keep PRs small, about one task each. A small PR gets reviewed today; a huge one sits for a week.

## Unity rules

1. **Move, rename and delete files only inside Unity**, never in Finder or Explorer. Unity moves the `.meta` file with it, which keeps the asset's ID and every reference to it.
2. **Always commit the `.meta` files** together with their assets. A missing `.meta` breaks references for everyone else.
3. **Save Project before committing.** Data asset and setting changes stay in memory until you save.
4. **`Arena.unity` belongs to Area 1 (Ship, orbit & camera).** Everyone else tests in their own sandbox and adds their finished prefab to `Arena` in a small PR. Never edit someone else's sandbox.
5. **Prefab-first.** Build things as prefabs and edit the prefab, not the copy in the scene. Two people can then work on different prefabs without touching the same file.
6. **Same prefab, same day?** Agree in chat who goes first. Smart Merge helps, but it can't fix two people changing the same object.
7. **Never commit** `Library/`, `Temp/`, `Logs/`, `UserSettings/` or `.csproj` files. `.gitignore` already blocks them; don't force-add them.
8. **Empty folders need a `.gitkeep`** (Git doesn't store empty folders). Delete it once the folder has real files.

## Code rules

- **Namespace = `KngChak.` + the folder inside `Scripts`.** A script in `Scripts/Data` uses `namespace KngChak.Data`; one in `Scripts/Ship` uses `KngChak.Ship`.
- **One class per file**, and the file name matches the class name. Unity needs this for MonoBehaviours and ScriptableObjects.
- **Assemblies:** all game code is in `KngChak.Runtime` (everything under `Scripts/`). Editor tools go **only** in `Scripts/Editor/` (`KngChak.Editor`, namespace `KngChak.EditorTools`), because `UnityEditor` doesn't exist in phone builds.
- **Using a package in code** (for example the Input System)? Add it to the **Assembly Definition References** of `KngChak.Runtime`, or the code won't compile.
- **No magic numbers.** Read sizes and tuning from the data assets (`ArenaConfig`, `RockType_*`), never type `66` into a script.
- **Data assets are read-only for code.** Write their values as `[field: SerializeField] public float Size { get; private set; }`. Game code must never change a ScriptableObject at runtime, because in the editor that change is saved into the asset.
- **Inspector fields on MonoBehaviours:** `[SerializeField] private`, not `public`.
- **Style:** `.editorconfig` sets the formatting (4 spaces, `{` on its own line). Let your editor format on save.
- **The Console must be clean** (no red errors) before you open a PR.

## Art rules

The full rules are in the [art guide](Docs/kng-chak-art-guide.md). The short version:

- **Style:** flat vector + glow. Only energy glows (flame, heat stripes, primed rocks).
- **File names:** `kc_<group>_<name>_<variant>.png`, e.g. `kc_rock_iron_a.png`, `kc_ship_nose_hard.png`.
- **Import:** Sprite Mode **Single**, **32 pixels per unit** (planets 64).
- **Colours that change in play** (heat stripes, primed rocks, planet palettes) are drawn in greyscale and tinted in code.
- **Sorting layers**, back to front: `Backdrop`, `Stars`, `ArenaFX`, `Planet`, `Rocks`, `Ship`, `VFX`, `WorldUI`. Use a layer for categories and **Order in Layer** for parts of one object.

## Areas and owners

Each area owns its folders, namespace and prefabs. Ask the owner before changing their files.

| Area                       | Owner | Folders / namespace                                               | Owns                                                                        |
| -------------------------- | ----- | ----------------------------------------------------------------- | --------------------------------------------------------------------------- |
| 1. Ship, orbit & camera    | _TBD_ | `Scripts/Ship`, `Scripts/Arena` · `KngChak.Ship`, `KngChak.Arena` | Flying, touch controls, orbit, slingshot, camera, arena edge, `Arena.unity` |
| 2. Rocks & waves           | _TBD_ | `Scripts/Rocks` · `KngChak.Rocks`                                 | Spawning, falling, wave director, districts                                 |
| 3. Combat & combos         | _TBD_ | `Scripts/Combat` · `KngChak.Combat`                               | Flame push, prime and break, planet HP, combos, heals                       |
| 4. UI & game flow          | _TBD_ | `Scripts/UI`, `Scripts/Flow` · `KngChak.UI`, `KngChak.Flow`       | HUD, radar, menus, the 12-wave run, upgrades                                |
| 5. Art, VFX & audio + lead | _TBD_ | `Art/`, `Shaders/`, `Data/`                                       | Sprites, effects, sound, tuning data, reviews                               |

## Getting help

Stuck? Post in the team chat with what you tried and the **first red line** from the Unity Console. Never "fix" a problem by deleting `.meta` files or force-pushing.
