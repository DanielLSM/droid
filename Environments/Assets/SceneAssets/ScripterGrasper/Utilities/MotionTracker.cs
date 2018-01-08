namespace SceneAssets.ScripterGrasper.Utilities {
  public interface IMotionTracker {
    bool IsInMotion();

    bool IsInMotion(float sensitivity);
  }
}
