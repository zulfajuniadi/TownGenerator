## Medieval Town Generator ##

Developed unity 2018.1. Ported the code from [runegri/Town](https://github.com/runegri/Town) to generate meshes instead of SVG.

Generated Town:

![Generated](https://i.imgur.com/o3iCyqt.png "Generated")

Generator Options:

![Generator Options](https://i.imgur.com/9Pz3AUi.png "Generator Options")


Town Options:

- Overlay: Generates ground meshes
- Walls: Does the generated town have walls
- Water: Is the town situated next to a body of water
- Patches: How large of a map do you want to generate
- Seed: The random seed of the generator

Renderer Options:

- Root: The parent of the generated town.
- Materials: Various materials for different parts of the map

Usage:

View the `TownBuilder.cs` file to see how the generation is initiated.

Todos:

1. Migrate the scripts to use UnityEngine.Vector2.
2. Reduce GC allocation for generation (Now its around 100MB)
3. Make the generation multithreaded via jobs

### License ###

Copyright 2018 Zulfa Juniadi

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.