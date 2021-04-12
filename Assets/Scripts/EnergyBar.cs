using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnergyBar : MonoBehaviour
{
    [SerializeField] Image energy0Image;
    [SerializeField] Image energy1Image;
    [SerializeField] Image energy2Image;
    [SerializeField] Image energy3Image;
    [SerializeField] Image energy4Image;
    [SerializeField] Image energy5Image;

    private EnergySystem energySystem;

    private void Awake()
    {
        // Testing fill
        energy5Image.fillAmount = 0f;

        energySystem = new EnergySystem();
    }

    private void Update()
    {
        energySystem.Update();

        // Testing fill overtime
        energy5Image.fillAmount = energySystem.GetEnergyNormalized();
    }
}

public class EnergySystem
{
    public const int MAX_ENERGY = 6;

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
    }

    public void SpendEnergy(int energyCost)
    {
        // Insufficient energy
        if (currentEnergy < energyCost) return;

        // Energy is sufficient
        else
        {
            currentEnergy -= energyCost;
        }
    }

    public float GetEnergyNormalized()
    {
        return currentEnergy / MAX_ENERGY;
    }
}
