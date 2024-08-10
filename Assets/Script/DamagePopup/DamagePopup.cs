using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamagePopup : MonoBehaviour
{
   
    
    public static DamagePopup Create(Vector3 pos, float takenDmg, bool isCritHit)
    {
        Transform damagePopupTransform = Instantiate(GameAssets.Instance.pfDamagePopup, pos, Quaternion.identity);
        DamagePopup dmgPopup = damagePopupTransform.GetComponent<DamagePopup>();
        dmgPopup.SetUp(takenDmg, isCritHit);
        return dmgPopup;
    }

    private TextMeshPro _textMeshPro;
    private float timeDestroy;
    private Color textColor;
    private void Awake()
    {
        timeDestroy = 0.0f;
        _textMeshPro = GetComponent<TextMeshPro>();
    }
    private void Update()
    {
        float moveSpeed = 5f;
        transform.position += new Vector3(0, moveSpeed) * Time.deltaTime;
        timeDestroy += Time.deltaTime;

        if(timeDestroy >= 1f)
        {
            float dissappearSpeed = 3f;
            textColor.a -= dissappearSpeed * Time.deltaTime;
            _textMeshPro.color = textColor;

            if(textColor.a < 0f )
            {
                Destroy(gameObject);
            }
        }
    }
    public void SetUp(float damageAmount, bool isCritHit)
    {
        if (isCritHit) 
        {
            _textMeshPro.color = Color.red;
            _textMeshPro.fontSize += 1;
            
        }
        if (damageAmount >= 0) 
        {
            _textMeshPro.color = Color.green;
            _textMeshPro.SetText("+" + damageAmount.ToString());
        } else
        {
            _textMeshPro.SetText(damageAmount.ToString());
        }
        
    }

}
