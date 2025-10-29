using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Rechord_Gear : MonoBehaviour
{
    private GameObject rechord_gearGO;
    private RectTransform rechord_gear;
    private float speed;

    void Start()
    {
        rechord_gearGO = this.gameObject;
        rechord_gear = rechord_gearGO.GetComponent<RectTransform>();
    }

    public IEnumerator RotateGear(){
        speed = 10f;
        while(true){
            rechord_gear.Rotate(Vector3.forward*speed*Time.deltaTime);
            if(speed<=50f) speed += 10f;
            yield return null;
        }
    }
}
