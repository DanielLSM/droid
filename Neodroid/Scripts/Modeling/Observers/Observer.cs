using UnityEngine;
using Neodroid.Utilities;
using Neodroid.Environments;
using Neodroid.Managers;

namespace Neodroid.Observers
{
    [System.Serializable]
    public class Observer : MonoBehaviour
    {

        public LearningEnvironment _environment;

        public Vector3 _position;
        public Vector3 _rotation;
        public Vector3 _direction;

        //public Quaternion _rotation;
        //public Quaternion _direction;

        public bool _debug = false;
        public string _observer_identifier = "";
        public byte[] _data;

        protected virtual void Start()
        {
            AddToEnvironment();
            UpdatePosRotDir();
        }


        protected void AddToEnvironment()
        {
            _environment = NeodroidUtilities.MaybeRegisterComponent(_environment, this);
        }

        public virtual byte[] GetData()
        {
            if (_data != null)
                return _data;
            else
                return new byte[] { };
        }



        public virtual string GetObserverIdentifier()
        {
            return name + "Observer";
        }


        void UpdatePosRotDir()
        {
            if (_environment)
            {
                _position = _environment.TransformPosition(this.transform.position);
                _direction = _environment.TransformDirection(this.transform.forward);
                _rotation = _environment.TransformDirection(this.transform.up);
            }
            else
            {
                _position = this.transform.position;
                _direction = this.transform.forward;
                _rotation = this.transform.up;
            }
        }

        private void Update()
        {
            UpdatePosRotDir();
        }

        public virtual void Reset()
        {

        }
    }
}
