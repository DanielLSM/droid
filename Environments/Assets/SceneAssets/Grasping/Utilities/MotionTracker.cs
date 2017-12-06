using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SceneSpecificAssets.Grasping.Utilities {
  public interface MotionTracker {
    bool IsInMotion ();

    bool IsInMotion (float sensitivity);
  }
}
