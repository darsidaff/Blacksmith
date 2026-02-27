
using UnityEngine;
using UnityEngine.UI;

public class InstructionPage : MonoBehaviour
{
    [SerializeField] private int pagesNumber;
    [SerializeField] private GameObject pages;
    [SerializeField] private Button nextButton;
    [SerializeField] private Button prevButton;
    private int currentIndex = 0;
    private MeshRenderer meshRenderer;

    public void Initialize()
    {
        meshRenderer = pages.GetComponent<MeshRenderer>();
        InitButtons();
        TurnPageEvents();
        UpdatePageUI();
    }
    private void UpdatePageUI()
    {
        switch(currentIndex)
        {
            case 0:
                meshRenderer.material.mainTexture = Resources.Load<Texture2D>("Page_1");
                break;
            case 1:
                meshRenderer.material.mainTexture = Resources.Load<Texture2D>("Page_2");
                break;
            case 2:
                meshRenderer.material.mainTexture = Resources.Load<Texture2D>("Page_3");
                break;
            case 3:
                meshRenderer.material.mainTexture = Resources.Load<Texture2D>("Page_4");
                break;
        }
    }
    private void TurnPageEvents()
    {
        nextButton.onClick.AddListener(NextPage);
        prevButton.onClick.AddListener(PreviousPage);
    }

    private void NextPage()
    {
        currentIndex++;
        UpdatePageUI();
        if (currentIndex == pagesNumber - 1) nextButton.gameObject.SetActive(false);
        prevButton.gameObject.SetActive(true);
    }
    private void PreviousPage()
    {
        currentIndex--;
        UpdatePageUI();
        if (currentIndex == 0) prevButton.gameObject.SetActive(false);
        nextButton.gameObject.SetActive(true);
    }
    private void InitButtons()
    {
        nextButton.GetComponent<BookButton>().Initialize();
        prevButton.GetComponent<BookButton>().Initialize();
    }
}
