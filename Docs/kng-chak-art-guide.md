# Kng Chak art guide (Unity build)

Agreed 2026-10-24. Reference for drawing assets and building the arena in Unity. Numbers are starting values; change this file when they change.

## Style: flat vector + glow
- Flat shapes with 2–3 tones of cel shading: base, shadow and an optional highlight. No outlines and no gradients on solid objects.
- Light comes from the **top left** on every object, so shadows sit bottom right.
- **Glow means "this matters".** Only energy glows: the flame, heat stripes, primed rocks, the orbit band, PERFECT and combo callouts. Rocks, the hull and the planet never glow on their own.
- Readability beats detail. Every rock type must be recognisable by **shape alone** (colour-blind safe) at phone size, about 15–40 px on screen.
- Fonts: Atomic Age for display and Jost for UI and numbers (see the brand fonts decision).

## Who makes what
| Source | Assets |
|---|---|
| You (vector tool: Figma, Affinity or Inkscape) | Rocket parts, rocks, planet surfaces, upgrade icons, logo |
| Packs (CC0 or bought, e.g. Kenney) | Particle textures, SFX, star and nebula backdrops if they fit the style |
| Shaders and particles in Unity | Flame, orbit band, edge ripple, atmosphere rim, sparks, debris, impact rings |
| Code | Fuel arc, threat ring, chevron, radar |
| AI | Moodboards and concept references only, never shipped art |

## Palette (from the prototype)
| Use | Colour |
|---|---|
| Space background | `#141a33` (deep), `#0e0f22` (darker) |
| Ember planet ocean, deep ocean, land, ice | `#2f8fa8`, `#1b4672`, `#3f7d4a`, `#dfeef5` |
| Atmosphere / orbit band | `#4fd8e8` |
| Rock | `#b08a68` |
| Shard | `#c49a74` |
| Comet | `#bff6ff` (glow `#2a8fa0`) |
| Iron | `#5d6b7d` |
| Cluster | `#8a5a7a` |
| Heat: soft (red-orange) | core `#ffb347`, mid `#ff5a1f`, edge `#c0260f` |
| Heat: boost (white-gold) | core `#ffffff`, mid `#ffe08a`, edge `#ffa030` |
| Heat: slingshot (blue) | core `#f2ffff`, mid `#7fe6ff`, edge `#2f7dff` |
| Heat: stunned (grey) | core `#9aa0aa`, mid `#6a707c`, edge `#40444d` |
| Primed rock | gold shifting to magenta |

## Tint, don't redraw
Anything whose colour changes in play is drawn **white or greyscale** and tinted in Unity: heat stripes, the prime overlay on rocks, planet land and ocean per district, the shield ring. One sprite then covers every state and district.

## Scale and export
- 1 Unity unit = 1 prototype unit. Planet radius 10, orbit band up to 4.5 above the surface (radius 14.5), arena edge at radius 66.
- Camera: `camView` 50 means the short side of the screen shows 100 units. On a 1080 px phone that's about 11 px per unit, so the ship (~3 units) is only ~32 px. **Decide the zoom on a phone (50 vs ~35) before drawing final art.**
- Export at **32 pixels per unit** (planets at 64). For example a rock of size 1.4 is about 45 px across in the PNG, and the planet is 1280 px.
- Keep vector source files; export PNG with transparent edges and 2 px of padding.
- Names: `kc_<group>_<name>_<variant>.png`, e.g. `kc_rock_iron_a.png`, `kc_ship_nose_hard.png`, `kc_planet_ember_surface.png`.
- In Unity: one Sprite Atlas per group (Gameplay, VFX, UI) and ASTC compression for mobile.

## Map: code plus data
- One `Arena` scene. No Tilemap and no scene per planet.
- Each planet is a `DistrictDefinition` asset: name, surface sprite, palette, nebula tint, spawn weights, drag. Ember (start, standard), Glacier (comets, low drag), Foundry (iron, 4-shard clusters).
- Rings (orbit band, edge) are drawn from code using `ArenaConfig` values, so tuning a number moves the art.

## Sorting layers (back to front)
| Layer | Contents |
|---|---|
| Backdrop | Solid colour and district nebula, follows the camera |
| Stars | 2 parallax star layers (tiling texture, scrolled by camera position) |
| ArenaFX | Orbit band, edge arc |
| Planet | Spinning surface in a circle mask, shadow overlay, atmosphere rim |
| Rocks | Rocks, debris |
| Ship | Rocket parts |
| VFX | Flame, sparks, impacts, shield |
| WorldUI | Fuel arc, threat ring, chevron |
| (UI canvas) | HUD, radar, cards, menus |

## Asset checklist in production order
**Phase 0: greybox.** Placeholder circle, triangle and ring sprites. Arena scene, data assets, phone zoom check.

**Phase 1: style tile.** One mock at real phone size: the Ember planet, the rocket in its soft and hard states, all 5 rock types and the flame. Check it on a phone before making the rest.

**Phase 2: Ember gameplay**
- [ ] Rocket: helmet nose (soft), halberd nose (hard), body, engine grille, fins, stripe mask (greyscale)
- [ ] Rocks: shard, rock, comet (+ tail), iron, cluster, 2–3 shape variants each, a prime mask, 3–4 debris chunks
- [ ] Ember planet: surface texture, shadow overlay
- [ ] Backdrop: Ember nebula, 2 star layers
- [ ] VFX: flame shader, sparks, impact burst, ignition ring, shield ring, PERFECT flash
- [ ] World UI: fuel arc, threat ring, chevron, edge ripple (code)

**Phase 3: UI.** TMP font assets (Atomic Age, Jost), HUD (HP %, combo, score), radar, upgrade card frame and Flame/Spear/Orbit icons, pause and results screens.

**Phase 4: later.** Inferno boss (plates, core, burn VFX), Glacier and Foundry (a surface texture and nebula each), cosmetic part skins.
