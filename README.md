#  3D-Projection in Godot
This little Project was created by me to understad how 3-D Graphics work.  
While it is written with Godot the Code in the scripts Folder can be used elswhere with light revision.  
There are two functions i have written to draw triangles to the screen, fillPolygon and fillRasterizedPolygon.  
The first function is fast but doesnt implement a Z-Buffer. The second function implements a Z-Buffer but is very slow because it calls setPixel for every pixel on screen.
## Example
![Cube](https://github.com/LucaBeati1/Photos/blob/main/Cube.PNG)
