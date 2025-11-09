using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject Prefab;
    public GameObject Parent;

    void OnMouseDown()
    {
        GameObject clone = Instantiate(Prefab, Parent.transform);
        clone.SetActive(true);
        RectTransform rt = clone.GetComponent<RectTransform>();
        if (rt != null)
            rt.anchoredPosition = Vector2.zero;  // 親の中で (0,0) に配置
    }
}
