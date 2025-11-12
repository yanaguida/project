using UnityEngine;

public class ScreenManager : MonoBehaviour
{
    public Transform miragara;

    void Update()
    {
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            miragara.transform.Rotate(0f, 0f, 10f * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            miragara.transform.Rotate(0f, 0f, -10f * Time.deltaTime);
        }
    }
}
