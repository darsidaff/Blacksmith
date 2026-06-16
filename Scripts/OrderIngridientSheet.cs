using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Serialization;

public class OrderIngridientSheet : MonoBehaviour
{
    [FormerlySerializedAs("_items")]
    [Header("References")]
    [SerializeField] private OrderItemData[] items;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private TMP_Text itemNameText;
    [SerializeField] private Image itemIcon;
    [SerializeField] private TMP_Text costText;
    [SerializeField] private TMP_Text quantityText;
    [SerializeField] private Button nextButton;
    [SerializeField] private Button prevButton;
    [SerializeField] private Button increaseButton;
    [SerializeField] private Button decreaseButton;
    [SerializeField] private Button confirmButton;

    private PersistentData persistentData; 
    private DataProvider dataProvider;
    
    private ICurrencyProvider currencyProvider;
    private int currentIndex;
    private int quantity = 1;
    
    public void Initialize(ICurrencyProvider currencyProvider, PersistentData persistentData,
        DataProvider dataProvider)
    {
        this.currencyProvider = currencyProvider;
        this.persistentData = persistentData;
        this.dataProvider = dataProvider;
        
        InitButtons();
        SubscribeEvents();
        UpdateUI();
        SpawnPurchasedItems();
    }

    private void SubscribeEvents()
    {
        nextButton.onClick.AddListener(NextItem);
        prevButton.onClick.AddListener(PreviousItem);
        increaseButton.onClick.AddListener(() => ChangeQuantity(1));
        decreaseButton.onClick.AddListener(() => ChangeQuantity(-1));
        confirmButton.onClick.AddListener(ProcessOrder);
    }

    private void UpdateUI()
    {
        var item = items[currentIndex];
        itemNameText.text = item.ItemName;
        itemIcon.sprite = item.Icon;
        quantityText.text = quantity.ToString();
        costText.text = $"{currencyProvider.GetBalance()} / {item.PricePerUnit * quantity}";
    }

    private void InitButtons()
    {
        nextButton.GetComponent<OrderButton>().Initialize();
        prevButton.GetComponent<OrderButton>().Initialize();
        increaseButton.GetComponent<OrderButton>().Initialize();
        decreaseButton.GetComponent<OrderButton>().Initialize();
        confirmButton.GetComponent<OrderButton>().Initialize();
    }

    private void NextItem()
    {
        currentIndex = (currentIndex + 1) % items.Length;
        UpdateUI();
    }

    private void PreviousItem()
    {
        currentIndex = (currentIndex - 1 + items.Length) % items.Length;
        UpdateUI();
    }

    private void ChangeQuantity(int delta)
    {
        quantity = Mathf.Clamp(quantity + delta, 1, 99);
        UpdateUI();
    }

    private void ProcessOrder()
    {
        var item = items[currentIndex];
        int totalCost = item.PricePerUnit * quantity;

        if (currencyProvider.CanAfford(totalCost))
        {
            currencyProvider.DeductAmount(totalCost);
            
            string itemKey = item.ItemName;
            if (!persistentData.PlayerData.PurchasedItems.ContainsKey(itemKey))
                persistentData.PlayerData.PurchasedItems[itemKey] = 0;

            persistentData.PlayerData.PurchasedItems[itemKey] += quantity;

            dataProvider.Save();
            SpawnItems(item.Prefab, quantity);
        }
        else
        {
            Debug.Log("Not enough coins!");
        }
        UpdateUI();
    }
    
    private void SpawnPurchasedItems()
    {
        foreach (var item in items)
        {
            if (persistentData.PlayerData.PurchasedItems.TryGetValue(item.ItemName, out int count))
            {
                SpawnItems(item.Prefab, count);
            }
        }
    }

    private void SpawnItems(GameObject prefab, int quantity)
    {
        for (int i = 0; i < quantity; i++)
        {
            Instantiate(prefab, spawnPoint.position, Quaternion.identity);
        }
    }

    private void OnDestroy()
    {
        nextButton.onClick.RemoveAllListeners();
        prevButton.onClick.RemoveAllListeners();
        increaseButton.onClick.RemoveAllListeners();
        decreaseButton.onClick.RemoveAllListeners();
        confirmButton.onClick.RemoveAllListeners();
    }
}
