using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


//TODO Rename to HealthSystem and import code from other project
public class HealthBar : MonoBehaviour 
{
    public Image imageHealthBar;
    public Text healthPercentageText;
    public int minHealth = 0;
    public int maxHealth;
    private int mCurrentValue;
    private float mCurrentPercentage;


    public void SetHealth(int health)
    {
        //don't update if already at full health...
        if(health != mCurrentValue)
        {
            //don't divide by 0 and explode the computer
            if(maxHealth - minHealth == 0)
            {
                mCurrentValue = 0;
                mCurrentPercentage = 0;
            }
            //attempt division to change value into a percentage for the fill amount slider
            else
            {
                mCurrentValue = health;
                mCurrentPercentage = (float)mCurrentValue / (float)(maxHealth - minHealth);
            }
            //update the percentage text
            healthPercentageText.text = string.Format("{0} %", Mathf.RoundToInt(mCurrentPercentage * 100));
            //update the health bar image
            imageHealthBar.fillAmount = mCurrentPercentage;
        }
    }

    public float CurrentHealthPercentage
    {
        get{
            return mCurrentPercentage;
        }
    }
    public int CurrentHealthValue
    {
        get{
            return CurrentHealthValue;
        }
    }

	void Start () 
    {

        SetHealth(43);
	}

}
