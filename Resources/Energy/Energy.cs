using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Energy : MonoBehaviour
{

    [SerializeField] private float maxEnergy = 0;
    [SerializeField] private float maxStamina = 0;
    [SerializeField] private float energyRegen = 0;
    [SerializeField] private float staminaRegen = 0;

    [SerializeField] Image energySlider = null;
    [SerializeField] Image staminaSlider = null;

    private float currentEnergy;
    private float currentStamina;

    void Start()
    {
        currentEnergy = maxEnergy;
        currentStamina = maxStamina;
    }

    public void modify_maxEnergy(float value)
    {
        maxEnergy = value;
    }
    public void modify_maxStamina(float value)
    {
        maxStamina = value;
    }

    public void modify_energyRegen(float value)
    {
        energyRegen = value;
    }
    public void modify_staminaRegen(float value)
    {
        staminaRegen = value;
    }

    public bool Drain_ES(bool EnergyOnly, float amount)
    {
        if (EnergyOnly)
        {
            if (currentEnergy  >= amount)
            {
                currentEnergy -= amount;
                return true;
            }
        }
        else
        {
            if (currentStamina >= amount)
            {
                currentStamina -= amount;
                return true;
            }
            else if((currentStamina + currentEnergy) >= amount)
            {
                currentEnergy -= (amount - currentStamina);
                currentStamina = 0;
                return true;
            }
        }
        return false;
    }

    public bool Drain_ES_Greater(bool EnergyOnly, float amount, bool NewCommand, float new_required_amount) //Used for some time-based drains. A new command requires the required_amount
    {
        if(NewCommand && (new_required_amount > (currentStamina + currentEnergy)))
        {
            return false;
        }
        return Drain_ES(EnergyOnly, amount);
    }

    void FixedUpdate()
    {
        if(currentEnergy >= maxEnergy)
        {
            currentEnergy = maxEnergy;
        }
        else
        {
            currentEnergy += energyRegen * Time.fixedDeltaTime;
        }

        if(currentStamina >= maxStamina)
        {
            currentStamina = maxStamina;
        }
        else
        {
            currentStamina += staminaRegen * Time.fixedDeltaTime;
        }

        if(energySlider)
        {
            UpdateEnergyBar();
        }
    }

    void UpdateEnergyBar()
    {
        energySlider.fillAmount = currentEnergy / maxEnergy;
        staminaSlider.fillAmount = currentStamina / maxStamina;
    }
}
