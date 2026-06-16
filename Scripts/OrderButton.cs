using UnityEngine;
using UnityEngine.UI;

public class OrderButton : MonoBehaviour
{
    private Button button;
    
    public void Initialize()
    {
        button = GetComponent<Button>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Pen"))
        {
            button.onClick?.Invoke();
        }
    }
}
