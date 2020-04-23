# PodEscape
Endless runner of a pod trying to escape from a project being deleted.

## Obstacles groups

There are right now two groups of obstacles:

* killingObstacles: All obstacles that kill the player when colliding with them.
* dashable: All obstacles that the player can dash through them but if it is not dashing, they will kill him (e.g.fire wall)

Every new obstacle need to be added to one of those groups for collisions to work

## Level design

The purpose of this part of the document is to explain how to design levels for Pod Escape.

### Steps for creating a level

1. Create a new scene.
2. Click on the "Instance a scene as a Node" button.
3. Select `res://src/Levels/Floor.tscn` as the scene to instance.
4. Repeat step 2 for every obstacle or collectable to add to the scene (always as a child of the root node, if you want to create a new obstacle, do it in `res://src/Scenes` directory).
5. Select the difficulty of the level.
6. Save the scene with the name you want in the `res://src/Levels` directory.
7. Open `res://src/Levels/World.tscn` in the Godot editor.
8. Select `LevelController` node.
9. Add your new scene to the corresponding array in the inspector, depending on the difficulty of the level.
10. Save the World scene.
11. Test your scene and do all the modifications you want. Check next section for easy tests.
12. Commit and push your code.

### Testing your levels

For testing your levels in an easy way and not having to wait for a medium or hard level to appear, do the following:

1. In `World` scene, add your level to `Debug Levels` array.
2. Enable `Level Debug`.
3. Run the game. The levels you want to test now will appear in the first minute.
4. Remember to disable `Level Debug` before pushing and cleaning the array.
