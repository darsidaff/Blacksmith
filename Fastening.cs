
using HurricaneVR.Framework.Core;
using UnityEngine;

public class Fastening : MonoBehaviour
{
    [SerializeField] private GameObject workBench;
    [SerializeField] private int stagesCount = 3;
    [SerializeField] private AudioSource hitSound;
    private SwordAssembly swordAssembly;
    private int fasteningStage = 0;
    private Rigidbody currentPartRb;
    public void Initialize()
    {
        swordAssembly = workBench.GetComponent<SwordAssembly>();
    }
    private void Start()
    {
        Initialize();
    }
    private void OnCollisionEnter(Collision collision)
    {
        int stage = swordAssembly.GetAssemblyStage();
        GameObject part = collision.gameObject;
        switch (part.tag)
        {
            case "Guard":
                if (stage == 2)
                {
                    ShiftPart(part, stage);
                }
                break;
            case "Handle":
                if (stage == 4)
                {
                    ShiftPart(part, stage);
                }
                break;
            case "Pommel":
                if (stage == 6)
                {
                    ShiftPart(part, stage);
                }
                break;
        }
    }
    private void ShiftPart(GameObject part, int stage)
    {
        hitSound.Play();
        part.transform.position += new Vector3(0.015f, 0, 0);
        if (fasteningStage < stagesCount - 1)
        {
            fasteningStage += 1;
        }
        else
        {
            currentPartRb = part.GetComponent<Rigidbody>();
            currentPartRb.isKinematic = true;
            swordAssembly.SetAssemblyStage(stage + 1);
            fasteningStage = 0;
            part.GetComponent<Collider>().enabled = false;
        }
    }
}
