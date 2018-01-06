namespace Neodroid.Messaging.Messages {
  public class ReactionParameters {
    bool _terminable = false;
    bool _step = false;
    bool _reset = false;
    bool _configure = false;
    bool _describe = false;
    bool _episode_count = true;
    bool _before_observation = false;


    public ReactionParameters (bool terminable = false, bool step = false, bool reset = false, bool configure = false, bool describe = false, bool episode_count = true) {
      _terminable = terminable;
      _reset = reset;
      _step = step;
      _configure = configure;
      _describe = describe;
      _episode_count = episode_count;
    }

    public bool EpisodeCount {
      get {
        return _episode_count;
      }
    }

    public bool BeforeObservation {
      get {
        return _before_observation;
      }
      set {
        _before_observation = value;
      }
    }

    public bool Terminable {
      get { return _terminable; }
    }

    public bool Describe {
      get { return _describe; }
    }

    public bool Reset {
      get { return _reset; }
    }

    public bool Step {
      get { return _step; }
    }

    public bool Configure {
      get { return _configure; }
    }

    public override string ToString () {
      return System.String.Format (
        "<ReactionParameters>\n " +
        "Terminable:{0},\nStep:{1},\nReset:{2},\nConfigure:{3},\nDescribe:{4}\nEpisodeCount:{5}" +
        "\n</ReactionParameters>\n", 
        Terminable.ToString (),
        Step.ToString (),
        Reset.ToString (),
        Configure.ToString (),
        Describe.ToString (),
        EpisodeCount.ToString ());
    }
  }

}
