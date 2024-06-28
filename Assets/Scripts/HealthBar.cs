using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Image healthBarSprite;
    public Transform cam;

    void Update()
    {
        transform.forward = cam.transform.forward;
    }
    public void updateHealthBar(float currentHP, float maxHP){
        healthBarSprite.fillAmount = currentHP / maxHP;
    }
}
