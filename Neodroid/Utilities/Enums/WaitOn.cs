namespace Neodroid.Scripts.Utilities.Enums {
  public enum WaitOn {
    Never,

    // Dont wait from reactions from agent
    Update,

    // Frame
    FixedUpdate
    // Note: unstable physics with the FixedUpdate setting
  }
}
