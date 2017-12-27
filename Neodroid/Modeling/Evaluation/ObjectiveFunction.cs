using UnityEngine;
using System.Collections.Generic;
using Neodroid.Utilities;
using System;
using System.Runtime.Serialization;
using System.Runtime.InteropServices;

namespace Neodroid.Evaluation {
  [System.Serializable]
  public abstract class ObjectiveFunction : MonoBehaviour, HasRegister<Term> {
    #region Fields

    [Header ("Development", order = 99)]
    [SerializeField]
    bool _debugging = false;

    [Header ("References", order = 100)]

    [Header ("General", order = 101)]
    [SerializeField]
    float _solved_threshold = 0;
    [SerializeField]
    Term[] _extra_terms;
    [SerializeField]
    Dictionary<string, Term> _extra_terms_dict = new Dictionary<string, Term> ();
    [SerializeField]
    Dictionary<Term, float> _extra_term_weights = new Dictionary<Term, float> ();


    #endregion

    void Awake () {
      foreach (var go in _extra_terms) {
        _extra_terms_dict.Add (go.name, go);
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

      if (Debugging) {
        print (signal);
      }
      return signal;
    }

    public void Reset () {
      InternalReset ();
    }

    public virtual void InternalReset () {
    }

    public bool Debugging {
      get {
        return _debugging;
      }
      set { 
        _debugging = value;
      }
    }

    public float SolvedThreshold {
      get {
        return _solved_threshold;
      }
      set {
        _solved_threshold = value;
      }
    }

    public virtual void AdjustExtraTermsWeights (Term term, float new_weight) {
      if (_extra_term_weights.ContainsKey (term))
        _extra_term_weights [term] = new_weight;
    }

    public virtual float EvaluateExtraTerms () {
      float extra_terms_output = 0;
      foreach (var term in _extra_terms_dict.Values) {
        extra_terms_output += _extra_term_weights [term] * term.evaluate ();
      }
      return extra_terms_output;
    }

    public virtual void Register (Term term) {
      _extra_terms_dict.Add (term.name, term);
      _extra_term_weights.Add (term, 1);
    }

    public virtual void Register (Term term, string identifier) {
      _extra_terms_dict.Add (term.name, term);
      _extra_term_weights.Add (term, 1);
    }


  }
}
