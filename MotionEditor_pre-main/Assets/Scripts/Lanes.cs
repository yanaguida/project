using UnityEngine;
using System.Collections;

public abstract class Lanes : MonoBehaviour
{
    protected float start;
    protected float time;

    public virtual void SetLaneData(){
       
    }

    public virtual IEnumerator ExecuteLane(){
        yield break;
    }

    public virtual float GetTotalTime(){
        return 0;
    }
}