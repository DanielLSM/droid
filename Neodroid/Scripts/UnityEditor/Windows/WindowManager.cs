#if UNITY_EDITOR
using System;
using UnityEditor;

#endif

namespace Neodroid.Scripts.UnityEditor.Windows {
  #if UNITY_EDITOR
  public class WindowManager : EditorWindow {
    static readonly Type[] _desired_dock_next_toos = {
                                                       typeof(RenderTextureConfiguratorWindow),
                                                       typeof(CameraSynchronisationWindow),
                                                       typeof(DebugWindow),
                                                       typeof(SegmentationWindow),
                                                       typeof(SimulationWindow),
                                                       typeof(TaskWindow),
                                                       typeof(DemonstrationWindow)
                                                     };

    [MenuItem(itemName : "Neodroid/ShowAll")]
    public static void ShowWindow() {
      GetWindow<RenderTextureConfiguratorWindow>(
                                                 desiredDockNextTo :
                                                 _desired_dock_next_toos); //Show existing window instance. If one doesn't exist, make one.
      GetWindow<CameraSynchronisationWindow>(desiredDockNextTo : _desired_dock_next_toos);
      GetWindow<DebugWindow>(desiredDockNextTo : _desired_dock_next_toos);
      GetWindow<SegmentationWindow>(desiredDockNextTo : _desired_dock_next_toos);
      GetWindow<SimulationWindow>(desiredDockNextTo : _desired_dock_next_toos);
      GetWindow<TaskWindow>(desiredDockNextTo : _desired_dock_next_toos);
      GetWindow<DemonstrationWindow>(desiredDockNextTo : _desired_dock_next_toos);
    }
  }

  #endif
}
