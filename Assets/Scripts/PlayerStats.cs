using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerStats : MonoBehaviour
{
    [SerializeField]
    private Image health_Stats, stamina_Stats;

    public float health = 100f;
    public float damage = 15f;
    // Start is called before the first frame update
    public void applyDamage()
    {
        health -= damage;
    }

    // Update is called once per frame
    void Update()
    {
        Display_HealthStats(health);
        if (Input.GetKeyDown(KeyCode.P))
        {
            applyDamage();
        }
    }



    // Start is called before the first frame update

    public void Display_HealthStats(float health)
    {
        health /= 100f;

        health_Stats.fillAmount = health;
        
    }

   /* public void Display_StaminaStats(float staminaValue)
    {
        staminaValue /= 100f;

        stamina_Stats.fillAmount = staminaValue;

    }*/

}