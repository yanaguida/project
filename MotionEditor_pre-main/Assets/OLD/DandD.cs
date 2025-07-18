using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pygmy.UI;
using UnityEngine.EventSystems;


public class DandD : MonoBehaviour
{

    float num;

    public void Setup()
    {
        var dragAndDrop = gameObject.AddComponent<DragAndDrop>();
        dragAndDrop.Setup(true, true);

        dragAndDrop.onBeginDrag.AddListener(() =>
        {
            Debug.Log("onBeginDrag");
        });

        dragAndDrop.onDrag.AddListener(() =>
        {
            Debug.Log("onDrag");
        });

        dragAndDrop.onEndDrag.AddListener(() =>
        {
            Debug.Log("onEndDrag");
        });
    }

    private void Update()
    {
    }

    private void CheckWhere(Moving moving)
    {
        num = moving.WhereScroll;
        Debug.Log(num);
    }


}
