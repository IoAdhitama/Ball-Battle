using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnergyBar : MonoBehaviour
{
    [SerializeField] GameManager game;

    [SerializeField] Image[] energyImageArray;

    private EnergySystem energySystem;

    private static readonly Vector4 UNFILLED_COLOR = new Vector4(0f, 0.5019f, 0.5019f, 1f); // Hex code: #008080
    private static readonly Vector4 FILLED_COLOR = new Vector4(0f, 1f, 1f, 1f); // Hex code: #00FFFF

    private void Awake()
    {
        energySystem = new EnergySystem();

        game.OnMatchStart += Game_OnMatchStart;
    }


    private void Update()
    {
        energySystem.Update();

        // Fill each segment
        float energy = energySystem.GetEnergy();
        int energySegmentCount = 6;
        for (int i = 0; i < energySegmentCount; i++)
        {
            // Find whether the energy is on a particular segment
            // That way, segment 1 will be for 0 to 1 energy, segment 2 will be for 1 to 2, and so on.
            int energySegmentMin = i * EnergySystem.ENERGY_PER_SEGMENT;
            int energySegmentMax = (i + 1) * EnergySystem.ENERGY_PER_SEGMENT;

            if (energy <= energySegmentMin)
            {
                // Energy under minimum for segment, therefore it's empty
                energyImageArray[i].fillAmount = 0f;
                energyImageArray[i].color = UNFILLED_COLOR;
            }
            else
            {
                if (energy >= energySegmentMax)
                {
                    // Energy above maximum for segment, therefore it's full
                    energyImageArray[i].fillAmount = 1f;
                    energyImageArray[i].color = FILLED_COLOR;
                }
                else
                {
                    // Energy amount somewhere in between
                    float fillAmount = (float)(energy - energySegmentMin) / EnergySystem.ENERGY_PER_SEGMENT;
                    energyImageArray[i].fillAmount = fillAmount;
                    energyImageArray[i].color = UNFILLED_COLOR;
                }
            }
        }
    }

    private void Game_OnMatchStart(object sender, System.EventArgs e)
    {
        energySystem.ResetEnergy();
    }

    private void OnDestroy()
    {
        game.OnMatchStart -= Game_OnMatchStart;
    }
}

public class EnergySystem
{
    public const int MAX_ENERGY = 6;
    public const int ENERGY_PER_SEGMENT = 1;

    private float energyRegen;
    private float currentEnergy;

    public EnergySystem()
    {
        currentEnergy = 0;
        energyRegen = 1f;
    }

    public void Update()
    {
        currentEnergy += energyRegen * Time.deltaTime;
        currentEnergy = Mathf.Clamp(currentEnergy, 0f, MAX_ENERGY);
    }

    public void SpendEnergy(int energyCost)
    {
        if (currentEnergy >= energyCost)
        { currentEnergy -= energyCost; }
    }

    public float GetEnergyNormalized()
    {
        return currentEnergy / MAX_ENERGY;
    }

    public float GetEnergy()
    {
        return currentEnergy;
    }

    public void ResetEnergy()
    {
        currentEnergy = 0;
    }
}
