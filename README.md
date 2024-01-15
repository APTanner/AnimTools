# AnimTools
![3dGrid](https://github.com/APTanner/AnimTools/assets/53225660/4fffa8d3-4f91-40ae-a3fb-3a3cdc583f92)

Simple C# Unity Animation Library

*Built on Unity 2022.3.6f1*

**Note: This library is not complete. The intent is to add functionality as needed in the future.**

## Includes
AnimTools requires the [Shapes](https://acegikmo.com/shapes/) library for rendering. Please note that this library is a commercial product and is not included in the source code.

## Setup
To set up AnimTools:
1. Create a class that inherits from the `Animate` class.
2. Attach this class, along with a `VisualizationDrawer`, to GameObjects in the scene for correct rendering.

## Usage
- Implement animation code within the `AnimationUpdate` virtual function.
- Get a reference to the visualizer (shape drawing) using `Visualizer.instance`.
- Utilize the `Visualizer` along with time management methods from the `Animate` class to create animations or visualizations.
- Use the `waitForInput` method to pause the animation until the **SPACE** key is pressed.

### Animation Scrubbing and Playback Speed
- In the Unity inspector, you can scrub through the animation or change the playback speed after its initial run.
- Note: Scrubbing will respect the time taken waiting for user input.

### Drawing Functions and Ease Delegates
- Certain `Visualizer` drawing functions allow for ease delegate functions to modify the timing of their drawing.
- You can create a custom bezier curve for an ease function using the `BezierPoints` struct, or utilize functions from the static `EaseFunctions` class.
