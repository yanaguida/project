using UnityEngine;

public class ScreenManager : MonoBehaviour
{
    public Transform miragara;
    private float time;
    [SerializeField] private float speed = 0.2f;

    void Update()
    {
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            time += Time.deltaTime;
            miragara.transform.Rotate(0f, 0f, speed * time);
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            time += Time.deltaTime;
            miragara.transform.Rotate(0f, 0f, -speed * time);
        }
        else if (Input.GetKey(KeyCode.UpArrow))
        {
            time += Time.deltaTime;
            miragara.transform.Rotate(speed * -time, 0f, 0f);
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            time += Time.deltaTime;
            miragara.transform.Rotate(speed * time, 0f, 0f);
        }
        else time = 0;
    }
}
