using UnityEngine;
using System.Collections.Generic;
using Neodroid.Utilities;
using System;
using System.Runtime.Serialization;
using System.Runtime.InteropServices;

namespace Neodroid.Evaluation {
  [Serializable]
  public abstract class ObjectiveFunction : MonoBehaviour, HasRegister<Term> {

    public bool _debug = false;

    public float _solved_threshold = 0;

    public Term[] _extra_term_go;

    public Dictionary<string, Term> _extra_terms = new Dictionary<string, Term> ();
    public Dictionary<Term, float> _extra_term_weights = new Dictionary<Term, float> ();

    /*[SerializeField]
        public StringGameObjectDictionary _extra_terms_serial = StringGameObjectDictionary.New<StringGameObjectDictionary>();
        public Dictionary<string, GameObject> _extra_terms {
          get { return _extra_terms_serial.dictionary; }
        }*/

    void Awake () {
      foreach (var go in _extra_term_go) {
        _extra_terms.Add (go.name, go);
        _extra_term_weights.Add (go, 1);
      }
    }

    public virtual float InternalEvaluate () {
      return 0;
    }


    public float Evaluate () {
      var signal = 0.0f;
      signal += InternalEvaluate ();
      signal += EvaluateExtraTerms ();

      if (_debug) {
        print (signal);
      }
      return signal;
    }

    public void Reset () {
      InternalReset ();
    }

    public virtual void InternalReset () {
    }


    public virtual void AdjustExtraTermsWeights (Term term, float new_weight) {
      if (_extra_term_weights.ContainsKey (term))
        _extra_term_weights [term] = new_weight;
    }

    public virtual float EvaluateExtraTerms () {
      float extra_terms_output = 0;
      foreach (var term in _extra_terms.Values) {
        extra_terms_output += _extra_term_weights [term] * term.evaluate ();
      }
      return extra_terms_output;
    }

    public virtual void Register (Term term) {
      _extra_terms.Add (term.name, term);
      _extra_term_weights.Add (term, 1);
    }

    public virtual void Register (Term term, string identifier) {
      _extra_terms.Add (term.name, term);
      _extra_term_weights.Add (term, 1);
    }


  }
}
