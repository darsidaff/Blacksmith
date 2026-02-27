
using UnityEngine;
using UnityEngine.UI;

public class BookButton : MonoBehaviour
{
    [SerializeField] private int cooldownToClick;
    private Button button;
    private bool clickable = true;

    public void Initialize()
    {
        button = GetComponent<Button>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (clickable)
        {
            button.onClick?.Invoke();
            clickable = false;
            Invoke("MakeClickable", cooldownToClick);
        }
    }

    private void MakeClickable()
    {
        clickable = true;
    }
}
