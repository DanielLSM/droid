using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Neodroid.Utilities.NeodroidCamera
{
    [ExecuteInEditMode]
    public class IgnoreLightSource : MonoBehaviour
    {

        public Light[] _lights_to_ignore;
        public bool _ignore_infrared_if_empty = true;

        // Use this for initialization
        void Start()
        {
            if (_lights_to_ignore == null || _lights_to_ignore.Length == 0 && _ignore_infrared_if_empty)
            {
                var infrared_light_sources = FindObjectsOfType<InfraredLightSource>();
                List<Light> lights = new List<Light>();
                foreach (var ils in infrared_light_sources)
                {
                    lights.Add(ils.GetComponent<Light>());
                }
                _lights_to_ignore = lights.ToArray();
            }
        }

        // Update is called once per frame
        void Update()
        {

        }

        void OnPreCull()
        {
            if (_lights_to_ignore != null)
            {
                foreach (var light in _lights_to_ignore)
                {
                    if (light)
                        light.enabled = false;
                }
            }
        }

        void OnPreRender()
        {
            if (_lights_to_ignore != null)
            {
                foreach (var light in _lights_to_ignore)
                {
                    if (light)
                        light.enabled = false;
                }
            }
        }

        void OnPostRender()
        {
            if (_lights_to_ignore != null)
            {
                foreach (var light in _lights_to_ignore)
                {
                    if (light)
                        light.enabled = true;
                }
            }
        }
    }
}