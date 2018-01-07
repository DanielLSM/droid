#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Neodroid.Windows {
  #if UNITY_EDITOR
  public class WindowManager : EditorWindow {
    private static readonly System.Type[] _desired_dock_next_toos = {
                                                               typeof(RenderTextureConfiguratorWindow),
                                                               typeof(CameraSynchronisationWindow),
                                                               typeof(DebugWindow),
                                                               typeof(SegmentationWindow),
                                                               typeof(SimulationWindow),
                                                               typeof(TaskWindow),
                                                               typeof(DemonstrationWindow)
                                                             };

    [MenuItem("Neodroid/ShowAll")]
    public static void ShowWindow() {
      GetWindow<RenderTextureConfiguratorWindow>(
                                                 _desired_dock_next_toos); //Show existing window instance. If one doesn't exist, make one.
      GetWindow<CameraSynchronisationWindow>(_desired_dock_next_toos);
      GetWindow<DebugWindow>(_desired_dock_next_toos);
      GetWindow<SegmentationWindow>(_desired_dock_next_toos);
      GetWindow<SimulationWindow>(_desired_dock_next_toos);
      GetWindow<TaskWindow>(_desired_dock_next_toos);
      GetWindow<DemonstrationWindow>(_desired_dock_next_toos);
    }
  }

  #endif
}
