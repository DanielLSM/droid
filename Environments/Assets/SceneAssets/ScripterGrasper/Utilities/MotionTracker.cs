namespace SceneSpecificAssets.Grasping.Utilities {
  public interface MotionTracker {
    bool IsInMotion();

    bool IsInMotion(float sensitivity);
  }
}
