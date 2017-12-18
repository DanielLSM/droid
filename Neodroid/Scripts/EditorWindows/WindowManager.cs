﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Neodroid.Configurations;

namespace Neodroid.Windows {
  #if UNITY_EDITOR
  using UnityEditor;

  public class WindowManager : EditorWindow {

    static Type[] _desired_dock_next_toos = new Type[] {
      typeof(RenderTextureConfiguratorWindow),
      typeof(CameraSynchronisationWindow),
      typeof(DebugWindow),
      typeof(SegmentationWindow),
      typeof(SimulationWindow),
      typeof(TaskWindow),
      typeof(DemonstrationWindow)
    };

    [MenuItem ("Neodroid/ShowAll")]
    public static void ShowWindow () {
      EditorWindow.GetWindow<RenderTextureConfiguratorWindow> (_desired_dock_next_toos);      //Show existing window instance. If one doesn't exist, make one.
      EditorWindow.GetWindow<CameraSynchronisationWindow> (_desired_dock_next_toos); 
      EditorWindow.GetWindow<DebugWindow> (_desired_dock_next_toos); 
      EditorWindow.GetWindow<SegmentationWindow> (_desired_dock_next_toos); 
      EditorWindow.GetWindow<SimulationWindow> (_desired_dock_next_toos);
      EditorWindow.GetWindow<TaskWindow> (_desired_dock_next_toos); 
      EditorWindow.GetWindow<DemonstrationWindow> (_desired_dock_next_toos); 
    }

  }

  #endif
}