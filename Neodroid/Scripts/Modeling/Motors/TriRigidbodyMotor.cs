using System;
using Neodroid.Utilities;
using Neodroid.Messaging.Messages;
using UnityEngine;

namespace Neodroid.Motors
{    [RequireComponent(typeof(Rigidbody))]
	public class TriRigidbodyMotor : RigidbodyMotor
    {

        string _X;
        string _Y;
        string _Z;

        public override void RegisterComponent()
        {
            _X = GetMotorIdentifier() + "X";
            _Y = GetMotorIdentifier() + "Y";
            _Z = GetMotorIdentifier() + "Z";
            _actor_game_object = NeodroidUtilities.MaybeRegisterNamedComponent(_actor_game_object, (Motor)this, _X);
            _actor_game_object = NeodroidUtilities.MaybeRegisterNamedComponent(_actor_game_object, (Motor)this, _Y);
            _actor_game_object = NeodroidUtilities.MaybeRegisterNamedComponent(_actor_game_object, (Motor)this, _Z);
        }

        public override string GetMotorIdentifier()
        {
            return name + "Rigidbody";
        }

        public override void ApplyMotion(MotorMotion motion)
        {
            if (!_bidirectional && motion.Strength < 0)
            {
                Debug.Log("Motor is not bi-directional. It does not accept negative input.");
                return; // Do nothing
            }
            if (_debug)
                Debug.Log("Applying " + motion.ToString() + " To " + name);
            if (motion.GetMotorName() == _X)
            {
                _rigidbody.AddForce(Vector3.left * motion.Strength);
            }
            else if (motion.GetMotorName() == _Y)
            {
                _rigidbody.AddForce(Vector3.up * motion.Strength);
            }
            else if (motion.GetMotorName() == _Z)
            {
                _rigidbody.AddForce(Vector3.forward * motion.Strength);
            }

            _energy_spend_since_reset += _energy_cost * motion.Strength;
        }
    }
}
