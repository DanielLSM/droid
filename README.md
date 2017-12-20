![neodroid](images/header.png)

# Droid
Droid is a unity package that enables prototyping reinforcement learning environments and communication to the [Neo](https://github.com/sintefneodroid/neo) counterpart of the [Neodroid](https://github.com/sintefneodroid) platform.

## Screenshot Of The Game View With The Droid Unity Package
![droid](images/neodroid.png)

## Setup
### !Important notice for Windows users!
  Run '''fix_windows_symlink.bat''' with administrative privileges, to make windows recognise the symlink. If on any other platform you should be fine.

## Environments
- LunarLander
- 3DGridWorld
- Walker
- And many more..

![droid](images/3Dgridworld.png)
![droid](images/lunarlander.png)
![droid](images/walker.png)

## Features
This unity packages is a selection of scripts and tools for easily setting up (Rapid prototyping) and experimenting reinforcement learning environments.

### Objective Functions (Out Of The Box, Easily Extendable)
- euclidean distance between objects

### Types Of Observations (Out Of The Box, Easily Extendable)
- positions
- rotations
- bounding boxes of objects
- instance segmentation images
- segmentation images
- depth images
- infrared shadow images
- rgb images

### Types Of Motors (Out Of The Box, Easily Extendable)

- rigidbody motors
- single Axis Transform motors
- wheel motors for vehicles

### Many Custom Windows For Quick Prototyping
![windows](images/neo_sync.png)
![windows](images/neo_segment.png)
![windows](images/neo_tex.png)
![windows](images/neo_debug.png)
![windows](images/neo_env.png)

# To Do's
- [ ] Server side memory hash table for large environment state spaces (raw poses and bodies are never send to the learning agent but instead a hash value is send, for dictionary look ups on the server).

# Other Components Of The Neodroid Platform

- [agent](https://github.com/sintefneodroid/agent)
- [simulation](https://github.com/sintefneodroid/simulation)
- [neo](https://github.com/sintefneodroid/neo)
