using System.Collections.Generic;
using UnityEngine;

public class Bootstrap : MonoBehaviour
{
    [SerializeField] private GrindingWheel  grindingWheel;
    [SerializeField] private OrderSubmission orderSubmission;
    [SerializeField] private OrderIngridientSheet[] orderSheets;
    [SerializeField] private InstructionPage instructionPage;
    [SerializeField] private CoinsView coinsView;
    [SerializeField] private Forge forge;
    [SerializeField] private Ingot[] ingots;
    [SerializeField] private Anvil anvil;
    
    private PersistentData persistentData;
    private DataProvider dataProvider;
    private Coins coins;
    private ICurrencyProvider currencyProvider;

    private void Awake()
    {
        InitializeData();
        InitializeCurrencySystem();
        InitializeOrderSheets();
        InitializeInstructionPage();
        InitializeOrderSubmission();
        InitializeGrindingWheel();
        InitializeForge();
        InitializeIngots();
        InitializeAnvil();
    }

    private void InitializeAnvil()
    {
        if (anvil)
            anvil.Initialize();
    }
    
    private void InitializeIngots()
    {
        foreach (var ingot in ingots)
        {
            ingot.Initialize();
        }
    }

    private void InitializeGrindingWheel()
    {
        if (grindingWheel)
            grindingWheel.Initialize();
    }

    private void InitializeForge()
    {
        if (forge)
            forge.Initialize();
    }

    private void InitializeOrderSubmission()
    {
        if (orderSubmission)
            orderSubmission.Initialize(coins);
    }

    private void InitializeCurrencySystem()
    {
        coins = new Coins(persistentData, dataProvider);
        currencyProvider = new CurrencyAdapter(coins);
        if (coinsView)
            coinsView.Initialize(coins);
    }

    private void InitializeOrderSheets()
    {
        foreach (var sheet in orderSheets)
        {
            sheet.Initialize(currencyProvider, persistentData, dataProvider);
        }
    }

    private void InitializeInstructionPage()
    {
        if (instructionPage)
            instructionPage.Initialize();
    }

    private void InitializeData()
    {
        persistentData = new PersistentData();
        dataProvider = new DataProvider(persistentData);

        LoadDataOrInit();

        dataProvider.Save();
    }
    private void LoadDataOrInit()
    {
        if (dataProvider.TryLoad() == false)
        {
            Debug.Log("Init new PlayerData.");
            persistentData.PlayerData = new PlayerData();
        }
        else
        {
            Debug.Log("Data load.");
        }
    }
}
