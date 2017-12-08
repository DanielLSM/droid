using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Neodroid.Evaluation;
using Neodroid.Utilities;
using Neodroid.Observers;
using Neodroid.Motors;
using Neodroid.Actors;
using Neodroid.Managers;
using Neodroid.Messaging.Messages;
using Neodroid.Configurations;

namespace Neodroid.Environments
{
    public class LearningEnvironment : MonoBehaviour, HasRegister<Actor>, HasRegister<Observer>, HasRegister<ConfigurableGameObject>
    {

        #region PublicMembers

        public CoordinateSystem _coordinate_system = CoordinateSystem.GlobalCoordinates;
        public Transform _coordinate_reference_point;

        //infinite
        public ObjectiveFunction _objective_function;
        public SimulationManager _simulation_manager;
        public bool _debug = false;

        #endregion

        #region PrivateMembers

        Vector3[] _reset_positions;
        Quaternion[] _reset_rotations;
        GameObject[] _child_game_objects;
        Dictionary<string, Actor> _actors = new Dictionary<string, Actor>();
        Dictionary<string, Observer> _observers = new Dictionary<string, Observer>();
        Dictionary<string, ConfigurableGameObject> _configurables = new Dictionary<string, ConfigurableGameObject>();


        int _current_episode_frame = 0;
        float _lastest_reset_time = 0;
        float energy_spent = 0f;
        bool _was_interrupted = false;
        Reaction _lastest_reaction = null;
        bool _waiting_for_reaction = true;
        bool _has_stepped_since_reaction = true;


        #endregion

        #region UnityCallbacks

        void Start()
        {
            FindMissingMembers();
            AddToEnvironment();
            SaveInitialPoses();
        }

        void SaveInitialPoses()
        {
            var _ignored_layer = LayerMask.NameToLayer("IgnoredByNeodroid");
            _child_game_objects = NeodroidUtilities.FindAllGameObjectsExceptLayer(_ignored_layer);
            _reset_positions = new Vector3[_child_game_objects.Length];
            _reset_rotations = new Quaternion[_child_game_objects.Length];
            for (int i = 0; i < _child_game_objects.Length; i++)
            {
                _reset_positions[i] = _child_game_objects[i].transform.position;
                _reset_rotations[i] = _child_game_objects[i].transform.rotation;
            }
        }

        void Update()
        { // Update is called once per frame, updates like actor position needs to be done on the main thread

            /*if (_episode_length > 0 && _current_episode_frame > _episode_length) {
              Debug.Log ("Maximum episode length reached, resetting");
              ResetRegisteredObjects ();
              _simulation_manager.ResetEnvironment ();
              _current_episode_frame = 0;
              return;
            }*/
            if (_simulation_manager._episode_length > 0 && _current_episode_frame > _simulation_manager._episode_length)
            {
                if (_debug)
                    Debug.Log("Maximum episode length reached, resetting");
                Interrupt();
            }

            if (_lastest_reaction != null && _lastest_reaction._reset)
            {
                Interrupt();
                Configure(_lastest_reaction.Configurations);
                return;
            }

            if (_lastest_reaction != null && !_waiting_for_reaction)
            {
                //ExecuteReaction (_lastest_reaction);
            }

            if (!_simulation_manager._continue_lastest_reaction_on_disconnect)
            {
                _lastest_reaction = null;
            }
        }

        void LateUpdate()
        {
            if (!_waiting_for_reaction && !_has_stepped_since_reaction)
            {
                //UpdateObserversData ();
                _has_stepped_since_reaction = true;
            }
            if (!_waiting_for_reaction && _has_stepped_since_reaction && _simulation_manager.IsSimulationUpdated())
            {
                //_simulation_manager.SendEnvironmentState (GetCurrentState ());
                _waiting_for_reaction = true;
            }
        }

        #endregion

        #region Helpers


        void FindMissingMembers()
        {
            if (!_simulation_manager)
            {
                _simulation_manager = FindObjectOfType<SimulationManager>();
            }
            if (!_objective_function)
            {
                _objective_function = FindObjectOfType<ObjectiveFunction>();
            }
        }

        public void UpdateObserversData()
        {
            foreach (Observer obs in GetObservers().Values)
            {
                obs.GetComponent<Observer>().GetData();
            }
        }

        public EnvironmentState GetCurrentState()
        {
            foreach (Actor a in _actors.Values)
            {
                foreach (Motor m in a.GetMotors().Values)
                {
                    energy_spent += m.GetEnergySpend();
                }
            }
            var reward = 0f;
            if (_objective_function != null)
                reward = _objective_function.Evaluate();

            var interrupted_this_step = false;
            if (_was_interrupted)
            {
                interrupted_this_step = true;
                _was_interrupted = false;
            }

            return new EnvironmentState(
              GetTimeSinceReset(),
              energy_spent,
              _actors, _observers,
              GetCurrentFrameNumber(),
              reward,
              interrupted_this_step);
        }

        public int GetCurrentFrameNumber()
        {
            return _current_episode_frame;
        }

        public float GetTimeSinceReset()
        {
            return Time.time - _lastest_reset_time;//Time.realtimeSinceStartup;
        }

        public void ExecuteReaction(Reaction reaction)
        {
            _current_episode_frame++;
            var actors = GetActors();
            if (reaction != null && reaction.GetMotions().Length > 0)
                foreach (MotorMotion motion in reaction.GetMotions())
                {
                    if (_debug)
                        Debug.Log("Applying " + motion.ToString() + " To " + name + "'s actors");
                    var motion_actor_name = motion.GetActorName();
                    if (actors.ContainsKey(motion_actor_name))
                    {
                        actors[motion_actor_name].ApplyMotion(motion);
                    }
                    else
                    {
                        if (_debug)
                            Debug.Log("Could find not actor with the specified name: " + motion_actor_name);
                    }
                }
        }

        public void Configure(Configuration[] configurations)
        {
            foreach (var configuration in configurations)
            {
                if (_configurables.ContainsKey(configuration.ConfigurableName))
                {
                    _configurables[configuration.ConfigurableName].ApplyConfiguration(configuration);
                }
                else
                {
                    if (_debug)
                        Debug.Log("Could find not configurable with the specified name: " + configuration.ConfigurableName);
                }
            }
        }

        void AddToEnvironment()
        {
            _simulation_manager = NeodroidUtilities.MaybeRegisterComponent(_simulation_manager, this);
        }


        #endregion

        #region PublicMethods


        public Vector3 TransformPosition(Vector3 position)
        {
            if (_coordinate_system == CoordinateSystem.RelativeToReferencePoint)
            {
                if (_coordinate_reference_point)
                {
                    return _coordinate_reference_point.transform.InverseTransformPoint(position);
                }
                else
                {
                    return position;
                }
            }
            else
            {
                return position;
            }
        }

        public Vector3 InverseTransformPosition(Vector3 position)
        {
            if (_coordinate_system == CoordinateSystem.RelativeToReferencePoint)
            {
                if (_coordinate_reference_point)
                {
                    return _coordinate_reference_point.transform.TransformPoint(position);
                }
                else
                {
                    return position;
                }
            }
            else
            {
                return position;
            }
        }

        public Vector3 TransformDirection(Vector3 direction)
        {
            if (_coordinate_system == CoordinateSystem.RelativeToReferencePoint)
            {
                if (_coordinate_reference_point)
                {
                    return _coordinate_reference_point.transform.InverseTransformDirection(direction);
                }
                else
                {
                    return direction;
                }
            }
            else
            {
                return direction;
            }
        }

        public Vector3 InverseTransformDirection(Vector3 direction)
        {
            if (_coordinate_system == CoordinateSystem.RelativeToReferencePoint)
            {
                if (_coordinate_reference_point)
                {
                    return _coordinate_reference_point.transform.TransformDirection(direction);
                }
                else
                {
                    return direction;
                }
            }
            else
            {
                return direction;
            }
        }

        public Dictionary<string, Actor> GetActors()
        {
            return _actors;
        }

        public Dictionary<string, Observer> GetObservers()
        {
            return _observers;
        }


        public void Interrupt()
        {
            ResetRegisteredObjects();
            ResetEnvironment();
            _was_interrupted = true;
            if (_debug)
                Debug.Log("Was interrupted");
        }

        public string GetEnvironmentIdentifier()
        {
            return "LearningEnviroment" + name;
        }

        public void ResetRegisteredObjects()
        {
            if (_debug)
                Debug.Log("Resetting registed objects");
            foreach (var actor in _actors.Values)
            {
                actor.Reset();
            }
            foreach (var observer in _observers.Values)
            {
                observer.Reset();
            }
        }

        public void ResetEnvironment()
        {
            for (int resets = 0; resets < _simulation_manager._resets; resets++)
            {
                for (int i = 0; i < _child_game_objects.Length; i++)
                {
                    var rigid_body = _child_game_objects[i].GetComponent<Rigidbody>();
                    if (rigid_body)
                        rigid_body.Sleep();
                    _child_game_objects[i].transform.position = _reset_positions[i];
                    _child_game_objects[i].transform.rotation = _reset_rotations[i];
                    if (rigid_body)
                        rigid_body.WakeUp();

                    var animation = _child_game_objects[i].GetComponent<Animation>();
                    if (animation)
                        animation.Rewind();
                }
            }
            _lastest_reset_time = Time.time;
            _current_episode_frame = 0;
            //_is_environment_updated = false;
        }

        #region Registration

        public void Register(Actor actor)
        {
            if (_debug)
                Debug.Log(string.Format("Environment {0} has actor {1}", name, actor.GetActorIdentifier()));
            _actors.Add(actor.GetActorIdentifier(), actor);
        }

        public void Register(Actor actor, string identifier)
        {
            if (_debug)
                Debug.Log(string.Format("Environment {0} has actor {1}", name, identifier));
            _actors.Add(identifier, actor);
        }

        public void Register(Observer observer)
        {
            if (_debug)
                Debug.Log(string.Format("Environment {0} has observer {1}", name, observer.GetObserverIdentifier()));
            _observers.Add(observer.GetObserverIdentifier(), observer);
        }

        public void Register(Observer observer, string identifier)
        {
            if (_debug)
                Debug.Log(string.Format("Environment {0} has observer {1}", name, identifier));
            _observers.Add(identifier, observer);
        }

        public void Register(ConfigurableGameObject configurable)
        {
            if (_debug)
                Debug.Log(string.Format("Environment {0} has configurable {1}", name, configurable.GetConfigurableIdentifier()));
            _configurables.Add(configurable.GetConfigurableIdentifier(), configurable);
        }

        public void Register(ConfigurableGameObject configurable, string identifier)
        {
            if (_debug)
                Debug.Log(string.Format("Environment {0} has configurable {1}", name, identifier));
            _configurables.Add(identifier, configurable);
        }


        #endregion

        #endregion


    }
}
