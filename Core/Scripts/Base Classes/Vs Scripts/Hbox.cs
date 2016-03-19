using UnityEngine;
using System.Collections;

public class Hbox : ScriptableObject {

    [SerializeField] private string _animationName;
    [SerializeField] private HitboxData _hitboxData;
    
    public string animationName { get { return _animationName; } }
    public HitboxData hitboxData { get { return _hitboxData.Clone(); } }
}