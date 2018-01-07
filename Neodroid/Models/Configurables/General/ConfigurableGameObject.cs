using Neodroid.Environments;
using Neodroid.Messaging.Messages;
using Neodroid.Utilities;
using UnityEngine;

namespace Neodroid.Configurables {
  public class ConfigurableGameObject : Configurable {
    public bool RelativeToExistingValue { get { return _relative_to_existing_value; } }

    public InputRange ValidInput { get { return _valid_input; } set { _valid_input = value; } }

    public LearningEnvironment ParentEnvironment {
      get { return _environment; }
      set { _environment = value; }
    }

    public bool Debugging { get { return _debugging; } set { _debugging = value; } }

    public override string ConfigurableIdentifier { get { return name + "Configurable"; } }

    public virtual void UpdateObservation() { }

    protected virtual void Start() { UpdateObservation(); }

    protected virtual void Awake() { AddToEnvironment(); }

    public void RefreshAwake() { Awake(); }

    public void RefreshStart() { Start(); }

    protected virtual void AddToEnvironment() {
      ParentEnvironment = NeodroidUtilities.MaybeRegisterComponent(
                                                                   ParentEnvironment,
                                                                   this);
    }

    public override void ApplyConfiguration(Configuration configuration) { }

    #region Fields

    [Header(
      "References",
      order = 99)]
    [SerializeField]
    private LearningEnvironment _environment;

    [Header(
      "Development",
      order = 100)]
    [SerializeField]
    private bool _debugging;

    [Header(
      "General",
      order = 101)]
    [SerializeField]
    private InputRange _valid_input =
      new InputRange {
                       decimal_granularity = 0,
                       min_value = 0,
                       max_value = 0
                     };

    [SerializeField]
    private bool _relative_to_existing_value;

    #endregion
  }
}
