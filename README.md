# PodEscape
Endless runner of a pod trying to escape from a project being deleted.

## Obstacles groups

There are right now two groups of obstacles:

* killingObstacles: All obstacles that kill the player when colliding with them.
* dashable: All obstacles that the player can dash through them but if it is not dashing, they will kill him (e.g.fire wall)

Every new obstacle need to be added to one of those groups for collisions to work
