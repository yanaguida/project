using UnityEngine;

public class Lanereader : MonoBehaviour
{
    public int num;
    public RectTransform droppedIconRect;
    public float occupiedlen;

    public void ReadLean(){
        FillVoidWithWait();
    }

    private void FillVoidWithWait(){
       // if(CheckVoid=true){
           // Controld.instance.lane1Dict();
        //}
    }

    private bool CheckVoid(){
        if(droppedIconRect.rect.width>occupiedlen){
            return true;
        }
        else
        return false;
    }
}
