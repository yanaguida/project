using UnityEngine;

public class ActiveManager : MonoBehaviour
{
    public GameObject gameObject;

    public void OnClickActivate()
    {
        this.gameObject.SetActive(true);
    }

    public void OnClickDeactivate()
    {
        this.gameObject.SetActive(false);
    }
}
