using System.Collections;
using TMPro;
using UnityEngine;

public class SCR_TextAnimator : MonoBehaviour
{
    [SerializeField] private string message;
    [SerializeField] float stringAnimationDuration = 2f;

    [SerializeField] TextMeshProUGUI animatedText;

    [SerializeField] private AnimationCurve sizeCurve;
    [SerializeField] private float sizeScale = 100f;
    [SerializeField, Range(0.0001f, 1)] private float charAnimationDuration = 0.2f;

    private float timeElapsed;

    private void Start()
    {
        StartCoroutine(RunAnimation(0.5f));
    }

    private IEnumerator RunAnimation(float waitForSeconds)
    {
        while (true) // Infinite loop to keep animation running
        {
            yield return new WaitForSeconds(waitForSeconds);

            timeElapsed = 0;
            while (timeElapsed <= stringAnimationDuration)
            {
                float t = timeElapsed / stringAnimationDuration;
                EvaluateRichText(t);
                timeElapsed += Time.deltaTime;

                yield return null;
            }

            // Ensure the final state is displayed before restarting
            EvaluateRichText(1);

            // Add a delay before restarting the animation
            //yield return new WaitForSeconds(0.5f);
        }
    }

    private void EvaluateRichText(float timer)
    {
        animatedText.text = "";

        for (int i = 0; i < message.Length; i++)
        {
            animatedText.text += EvaluateCharRichText(message[i], message.Length, i, timer);
        }
    }

    private string EvaluateCharRichText(char c, int sLength, int cPosition, float timer)
    {
        float startPoint = ((1 - charAnimationDuration) / (sLength - 1)) * cPosition;
        float endPoint = startPoint + charAnimationDuration;

        // Clamp subT to avoid abrupt resets or unintended behavior
        float subT = Mathf.Clamp(timer.Map(startPoint, endPoint, 0, 1), 0, 1);

        // Ensure size formatting is correct
        string sizeStart = $"<size={(sizeCurve.Evaluate(subT) * sizeScale):F0}%>";
        string sizeEnd = "</size>";

        return sizeStart + c + sizeEnd;
    }
}

public static class Extensions
{
    public static float Map(this float value, float fromLow, float fromHigh, float toLow, float toHigh)
    {
        if (fromHigh == fromLow) return toLow; // Prevent division by zero
        return (value - fromLow) * (toHigh - toLow) / (fromHigh - fromLow) + toLow;
    }
}
