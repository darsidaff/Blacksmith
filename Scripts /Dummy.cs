
using TMPro;
using UnityEngine;

public class Dummy : MonoBehaviour
{
    [SerializeField] private int cooldownToHide;

    [SerializeField] private GameObject dummyCanvas;
    [SerializeField] private TextMeshProUGUI strengthText;
    [SerializeField] private TextMeshProUGUI sharpnessText;
    [SerializeField] private AudioSource hitSound;
    [SerializeField] private ParticleSystem embersParticle;
    [SerializeField] private ParticleSystem sandParticle;
    [SerializeField] private ParticleSystem shockwaveParticle;
    private int currentStrength;
    private int currentSharpness;

    private void OnCollisionEnter(Collision collision)
    {
        GameObject sword = collision.gameObject;
        if (sword.tag == "Blade")
        {
            SwordCharacteristics currentSwordCharacteristics = sword.GetComponent<SwordCharacteristics>();
            if (currentSwordCharacteristics.GetAnvilComplete())
            {
                hitSound.Play();
                EmitParticles();
                currentStrength = currentSwordCharacteristics.GetStrength();
                currentSharpness = currentSwordCharacteristics.GetSharpness();
                CharacteristicsOutput();
            }
        }
    }

    private void CharacteristicsOutput()
    {
        strengthText.text = currentStrength.ToString();
        sharpnessText.text = currentSharpness.ToString();
        dummyCanvas.SetActive(true);
        Invoke("CharacteristicsHide", cooldownToHide);
    }

    private void CharacteristicsHide()
    {
        dummyCanvas.SetActive(false);
    }

    private void EmitParticles()
    {
        embersParticle.Emit(30);
        sandParticle.Emit(500);
        shockwaveParticle.Emit(1);
    }
}

