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
    readonly bool _reset;
    readonly bool _step;
    readonly bool _terminable;
    ExecutionPhase _phase = ExecutionPhase.Middle;

    public ReactionParameters(
      bool terminable = false,
      bool step = false,
      bool reset = false,
      bool configure = false,
      bool describe = false,
      bool episode_count = true) {
      this.IsExternal = false;
      this._terminable = terminable;
      this._reset = reset;
      this._step = step;
      this._configure = configure;
      this._describe = describe;
      this._episode_count = episode_count;
    }

    public bool EpisodeCount { get { return this._episode_count; } }

    public ExecutionPhase Phase { get { return this._phase; } set { this._phase = value; } }

    public bool IsExternal { get; set; }

    public bool Terminable { get { return this._terminable; } }

    public bool Describe { get { return this._describe; } }

    public bool Reset { get { return this._reset; } }

    public bool Step { get { return this._step; } }

    public bool Configure { get { return this._configure; } }

    public override string ToString() {
      return string.Format(
                           "<ReactionParameters>\n "
                           + "Terminable:{0},\nStep:{1},\nReset:{2},\nConfigure:{3},\nDescribe:{4}\nEpisodeCount:{5}"
                           + "\n</ReactionParameters>\n",
                           this.Terminable,
                           this.Step,
                           this.Reset,
                           this.Configure,
                           this.Describe,
                           this.EpisodeCount);
    }
  }
}
