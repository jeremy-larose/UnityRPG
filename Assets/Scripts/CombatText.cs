using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CombatText : MonoBehaviour
{
    private const float DISAPPEAR_TIMER_MAX = 1f;
    private TextMeshPro textMesh;
    private float disappearTimer;
    private Vector3 moveVector;
    private Color textColor;
    private static int sortingOrder;

    public static CombatText Create(Vector3 position, float damageAmount, bool isCriticalHit, Color color)
    {
        Transform damagePopupTransform =
            Instantiate(GameAssets.i.pfCombatText, position, Quaternion.identity);
        CombatText damagePopup = damagePopupTransform.GetComponent<CombatText>();
        damagePopup.Setup(damageAmount, isCriticalHit, color );

        return damagePopup;
    }

    private void Awake()
    {
        textMesh = transform.GetComponent<TextMeshPro>();
    }

    public void Setup(float damageAmount, bool isCriticalHit, Color color)
    {
        textMesh.SetText(damageAmount.ToString());
        textMesh.color = Color.cyan;

        if (!isCriticalHit)
        {
            textMesh.fontSize = 3f;
            textMesh.color = color;
        }
        else
        {
            textMesh.fontSize = 3f;
            textMesh.color = Color.red;
        }

        disappearTimer = DISAPPEAR_TIMER_MAX;

        sortingOrder++;
        textMesh.sortingOrder = sortingOrder;
        moveVector = new Vector3(1f, 2f, 2f);
    }

    private void Update()
    {
        transform.position += moveVector * Time.deltaTime;
        moveVector -= moveVector * (2f * Time.deltaTime);

        if (disappearTimer > DISAPPEAR_TIMER_MAX * .5f)
        {
            // first half of the popup
            float increaseScaleAmount = 1f;
            transform.localScale += Vector3.one * (increaseScaleAmount * Time.deltaTime);
        }
        else
        {
            // first half of the popup
            float decreaseScaleAmount = 1f;
            transform.localScale -= Vector3.one * (decreaseScaleAmount * Time.deltaTime);
            // second half of the popup
        }

        disappearTimer -= Time.deltaTime;
        if (disappearTimer < 0)
        {
            // Start disappearing
            float disappearSpeed = 3f;
            textColor.a -= disappearSpeed * Time.deltaTime;
            textMesh.color = textColor;
            if (textColor.a < 0)
            {
                Destroy(gameObject);
            }
        }
    }
}
