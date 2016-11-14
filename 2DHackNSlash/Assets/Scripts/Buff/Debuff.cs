using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public abstract class Debuff : MonoBehaviour {
    [HideInInspector]
    public ModData MD;
    [HideInInspector]
    public float CD = 0;

    protected ObjectController target;

    abstract public void ApplyDebuff(ModData MD,ObjectController target);
    abstract protected void RemoveDebuff();
}
