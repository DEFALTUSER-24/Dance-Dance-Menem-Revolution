using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonHover : MonoBehaviour
{
    public void Hover()
    {
        EventSystem.current.SetSelectedGameObject(this.gameObject);
    }
}