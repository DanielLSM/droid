using System;
using System.Collections.Generic;
using Neodroid.Scripts.Utilities.Interfaces;
using UnityEngine;

namespace Neodroid.Evaluation {
  [Serializable]
  public abstract class ObjectiveFunction : MonoBehaviour,
                                            IHasRegister<Term> {
    public bool Debugging { get { return this._debugging; } set { this._debugging = value; } }

    public float SolvedThreshold {
      get { return this._solved_threshold; }
      set { this._solved_threshold = value; }
    }

    public virtual void Register(Term term) {
      if (this.Debugging)
        print(
              message : string.Format(
                                      format : "Term registered: {0}",
                                      arg0 : term));
      this._extra_terms_dict.Add(
                                 key : term.name,
                                 value : term);
      this._extra_term_weights.Add(
                                   key : term,
                                   value : 1);
    }

    public virtual void Register(Term term, string identifier) {
      this._extra_terms_dict.Add(
                                 key : term.name,
                                 value : term);
      this._extra_term_weights.Add(
                                   key : term,
                                   value : 1);
    }

    void Awake() {
      foreach (var go in this._extra_terms) this.Register(term : go);
    }

    public virtual float InternalEvaluate() { return 0; }

    public float Evaluate() {
      var signal = 0.0f;
      signal += this.InternalEvaluate();
      signal += this.EvaluateExtraTerms();

      if (this.Debugging) print(message : signal);
      return signal;
    }

    public void Reset() { this.InternalReset(); }

    public virtual void InternalReset() { }

    public virtual void AdjustExtraTermsWeights(Term term, float new_weight) {
      if (this._extra_term_weights.ContainsKey(key : term)) this._extra_term_weights[key : term] = new_weight;
    }

    public virtual float EvaluateExtraTerms() {
      float extra_terms_output = 0;
      foreach (var term in this._extra_terms_dict.Values) {
        if (this.Debugging)
          print(
                message : string.Format(
                                        format : "Extra term: {0}",
                                        arg0 : term));
        extra_terms_output += this._extra_term_weights[key : term] * term.Evaluate();
      }

      if (this.Debugging)
        print(
              message : string.Format(
                                      format : "Extra terms signal: {0}",
                                      arg0 : extra_terms_output));
      return extra_terms_output;
    }

    #region Fields

    [Header(
      header : "Development",
      order = 99)]
    [SerializeField]
    bool _debugging;

    [Header(
      header : "References",
      order = 100)]
    [SerializeField]
    Term[] _extra_terms;

    [SerializeField] Dictionary<string, Term> _extra_terms_dict = new Dictionary<string, Term>();

    [SerializeField] Dictionary<Term, float> _extra_term_weights = new Dictionary<Term, float>();

    [Header(
      header : "General",
      order = 101)]
    [SerializeField]
    float _solved_threshold;

    #endregion
  }
}
