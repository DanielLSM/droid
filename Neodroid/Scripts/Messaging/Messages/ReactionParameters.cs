using System;

namespace Neodroid.Messaging.Messages {
  public class ReactionParameters {

    bool _reset = false;
    bool _step = false;
    bool _configure = false;
    bool _describe = false;
    bool _interruptible = false;

    public ReactionParameters (bool interruptible = false, bool step = false, bool reset = false, bool configure = false, bool describe = false) {
      _interruptible = interruptible;
      _reset = reset;
      _step = step;
      _configure = configure;
      _describe = describe;
    }

    public bool Interruptible {
      get { return _interruptible; }
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
        "Interruptible:{0},\nStep:{1},\nReset:{2},\nConfigure:{3},\nDescribe:{4}" +
        "\n</ReactionParameters>\n", 
        Interruptible.ToString (),
        Step.ToString (),
        Reset.ToString (),
        Configure.ToString (),
        Describe.ToString ());
    }
  }

}
