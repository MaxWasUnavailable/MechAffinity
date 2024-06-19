# Mech Affinity

## Description

Mech Affinity is a mod that gives pilots an affinity towards mechs they consistently use.

The more missions they complete in a mech, the higher their affinity grows, and the more bonuses they receive.

## Features

- Affinity grows by 1 per mission completed in a mech
- Affinity is reduced by 1 per mission completed in a different mech, for all mechs the pilot has affinity with
- Depending on affinity, bonuses are applied:
    - 5 - 20: Damage reduction

## Config

> The config can be found in the MechAffinity's mod directory, under `Config/settings.yaml`

- `UninstallMode` (default: `false`)
    - If set to `true`, the mod will remove all affinity data on save load, allowing for a clean uninstallation of the
      mod by saving.