![neodroid](images/header.png)

# Droid
Droid is a unity package that enables prototyping reinforcement learning environments and communication to the [Neo](https://github.com/sintefneodroid/neo) counterpart of the [Neodroid](https://github.com/sintefneodroid) platform.

## Screenshot Of The Game View With The Droid Unity Package
![droid](images/neodroid.png)

## Setup
### !Important Notice For Windows Users!
  Run '''fix_windows_symlink.bat''' with administrative privileges, to make windows recognise the symlink. If on any other platform you should be fine.

## Features
The unity comes quite selection of scripts and tools for setting up and experimenting reinforcement learning environments.

![droid](images/package.png)(Outdated images)

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
![windows](windows_menu.png)
![windows](images/neo_sync.png)
![windows](images/neo_segment.png)
![windows](images/neo_tex.png)
![windows](images/neo_debug.png)
![windows](images/neo_env.png)

# To Do's
- [ ] Be able to reset and configure enviroments independently from each other

# Other Components Of The Neodroid Platform

- [agent](https://github.com/sintefneodroid/agent)
- [simulation](https://github.com/sintefneodroid/simulation)
- [neo](https://github.com/sintefneodroid/neo)
