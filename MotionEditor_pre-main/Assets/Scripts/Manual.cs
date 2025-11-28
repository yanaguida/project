using UnityEngine;

public class Manual : MonoBehaviour
{
    [SerializeField] private GameObject screen;

    void Start()
    {
        //screen.SetActive(true);
    }

    public void OnMouseDown()
    {
        screen.SetActive(true);
    }

    public void CloseWindow()
    {
        screen.SetActive(false);
    }
}
