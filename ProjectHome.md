### Name: Squamster ###
### License: GPLv3 ###


### The Name ###
I was talking to a friend of mine and I asked about a name idea for this program. She mentioned that about 1/2 of the viewers she's seen (although she hasn't seen that many) have animal names. In college some friends and I created a 'legend' about a hamster (that one of the freshman girls snuck into the dorm) escaping and mating with one of the numerous squirrels on campus and forming a mutant "Squamster". This program is a combination of things you normally don't see together (Mesh viewer, Mesh painter, Open Source) and we decided the program was also a mutant.


### Features ###
  * Load any Ogre.mesh using its own material and shaders
  * View the mesh from any angle
  * Play and pause any of the meshes animations
  * A lot faster than the other mesh viewers I've seen
  * View all the textures attached to the mesh
  * Paint directly onto any texture attached to the mesh in real-time
  * Customizable brushes with opacity and scale
  * Image Filters  - Brightness, Contrast, Scale, Rotate
  * Blur, Burn, and Dodge Tools
  * Shape tools to draw a square, rectangle, circle, or ellipse.
  * Customizable Brushes with variable opacity and size.


### Upcoming planned features ###
  * Empty the "Known Bugs" list
  * Smudge tools
  * User settings - control undo history, add additional resource locations from settings.
  * Image Layers


## Prerequisites ##
  * Visual Studio 2008 redist
  * DirectX redist


# The Download #
  * http://code.google.com/p/squamster/downloads/list

# The Code #
  * http://code.google.com/p/squamster/source/checkout


### Known Bugs ###
  * If a resource location is added outside the viewer's project, "Load all" won't load from its old locations any more. (bug persists from previous release)


### Known Quirks ###
  * If the mesh you load doesn't have the materials/textures in the same folder or a subfolder, the materials won't be viewed.