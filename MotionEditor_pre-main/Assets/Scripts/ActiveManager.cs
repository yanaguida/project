using UnityEngine;

public class ActiveManager : MonoBehaviour
{
    public GameObject tekitou;

    public void OnClickActivate()
    {
        this.tekitou.SetActive(true);
    }

    public void OnClickDeactivate()
    {
        this.tekitou.SetActive(false);
    }
}
