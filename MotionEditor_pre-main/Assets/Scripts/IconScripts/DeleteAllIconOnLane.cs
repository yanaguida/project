using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using TMPro;

public class DeleteAllIconOnLane : MonoBehaviour
{
    private List<IIcon> IconList = new List<IIcon>();
    public GameObject confirmDialog;
    public TextMeshProUGUI message;
    [SerializeField] private int i = 0;

    public void OnMouseDown()
    {
        IconList = FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None).OfType<IIcon>().ToList();
        IconList.RemoveAll(icon => icon.GetIconState() != IconState.OnLane);
        i = 0;
        foreach (var icon in IconList)
        {
            if (icon.GetIssaved() == false)
            {
                Debug.Log("Unsaved Icon is: " + icon);
                i++;
            }
        }
        if (i > 0)
        {
            DisplayUnSavedNum();
            confirmDialog.SetActive(true);
        }
        else DeleteAll();
    }

    public void OnConfirmDeleteAll(bool i)
    {
        confirmDialog.SetActive(false);

        if (i)
            DeleteAll();
        else
            Debug.Log("消去をキャンセルしました。");
    }

    private void DeleteAll()
    {
        foreach (var icon in IconList)
            icon.Delete();
    }

    private void DisplayUnSavedNum()
    {
        string original = "There are xx unsaved icon exist.\nAre you sure to delete all?";
        string index = i.ToString();
        string result = original.Replace("xx", index);

        message.text = result;
    }
}
