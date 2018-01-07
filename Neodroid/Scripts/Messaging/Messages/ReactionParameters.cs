namespace Neodroid.Messaging.Messages {
  public enum ExecutionPhase {
    Before,
    Middle,
    After
  }

  public class ReactionParameters {
    readonly bool _configure;
    readonly bool _describe;
    readonly bool _episode_count;
    ExecutionPhase _phase = ExecutionPhase.Middle;
    readonly bool _reset;
    readonly bool _step;
    readonly bool _terminable;

    public ReactionParameters(
      bool terminable = false,
      bool step = false,
      bool reset = false,
      bool configure = false,
      bool describe = false,
      bool episode_count = true) {
      IsExternal = false;
      _terminable = terminable;
      _reset = reset;
      _step = step;
      _configure = configure;
      _describe = describe;
      _episode_count = episode_count;
    }

    public bool EpisodeCount { get { return _episode_count; } }

    public ExecutionPhase Phase { get { return _phase; } set { _phase = value; } }

    public bool IsExternal { get; set; }

    public bool Terminable { get { return _terminable; } }

    public bool Describe { get { return _describe; } }

    public bool Reset { get { return _reset; } }

    public bool Step { get { return _step; } }

    public bool Configure { get { return _configure; } }

    public override string ToString() {
      return string.Format(
                           "<ReactionParameters>\n "
                           + "Terminable:{0},\nStep:{1},\nReset:{2},\nConfigure:{3},\nDescribe:{4}\nEpisodeCount:{5}"
                           + "\n</ReactionParameters>\n",
                           Terminable,
                           Step,
                           Reset,
                           Configure,
                           Describe,
                           EpisodeCount);
    }
  }
}
