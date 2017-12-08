using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Neodroid.Utilities.NeodroidCamera
{
    [ExecuteInEditMode]
    public class ReplacementShaderEffect : MonoBehaviour
    {
        public Shader _replacement_shader;
        public Color _color;
        public string _replace_rendertype = "";

        void OnValidate()
        {
            Shader.SetGlobalColor("_OverDrawColor", _color);
            Shader.SetGlobalColor("_SegmentationColor", _color);
            //Shader.SetGlobalColor ("_Color", _color);
        }

        void OnEnable()
        {
            if (_replacement_shader != null)
                GetComponent<Camera>().SetReplacementShader(_replacement_shader, _replace_rendertype);
        }

        void OnDisable()
        {
            GetComponent<Camera>().ResetReplacementShader();
        }

        void OnPreRender()
        {

        }
    }
}