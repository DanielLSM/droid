using UnityEngine;
using System.Collections.Generic;
using Neodroid.Utilities;
using System;
using System.Runtime.Serialization;

namespace Neodroid.Evaluation {
  [Serializable]
  public abstract class ObjectiveFunction : MonoBehaviour, HasRegister<Term> {

    public bool _debug = false;

    public Dictionary<string, Term> _extra_terms = new Dictionary<string, Term> ();
    public Dictionary<Term, float> _extra_term_weights = new Dictionary<Term, float> ();

    /*[SerializeField]
        public StringGameObjectDictionary _extra_terms_serial = StringGameObjectDictionary.New<StringGameObjectDictionary>();
        public Dictionary<string, GameObject> _extra_terms {
          get { return _extra_terms_serial.dictionary; }
        }*/

    public abstract float Evaluate ();


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
      _extra_terms.Add (term.name (), term);
      _extra_term_weights.Add (term, 1);
    }

    public virtual void Register (Term term, string identifier) {
      _extra_terms.Add (term.name (), term);
      _extra_term_weights.Add (term, 1);
    }


  }
}
