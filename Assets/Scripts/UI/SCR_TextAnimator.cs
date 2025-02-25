using System.Collections;
using TMPro;
using UnityEngine;

public class SCR_TextAnimator : MonoBehaviour
{
    // Text that will be animated
    [SerializeField] private string message;

    // Duration for the entire text animation (in seconds)
    [SerializeField] float stringAnimationDuration = 2f;

    // Reference to the TextMeshProUGUI component where text is displayed
    [SerializeField] TextMeshProUGUI animatedText;

    // Animation curve controlling the size variation of each character
    [SerializeField] private AnimationCurve sizeCurve;

    // Scale factor for the text size change
    [SerializeField] private float sizeScale = 100f;

    // Duration each character takes to complete its individual animation
    [SerializeField, Range(0.0001f, 1)] private float charAnimationDuration = 0.2f;

    // Internal timer to track animation progress
    private float timeElapsed;

    private void Start()
    {
        // Start the animation with an initial delay of 0.5 seconds
        StartCoroutine(RunAnimation(0.5f));
    }

    private IEnumerator RunAnimation(float waitForSeconds)
    {
        while (true) // Infinite loop to keep animation running
        {
            // Wait for a short delay before restarting the animation cycle
            yield return new WaitForSeconds(waitForSeconds);

            timeElapsed = 0;

            // Animate the text over the specified duration
            while (timeElapsed <= stringAnimationDuration)
            {
                float t = timeElapsed / stringAnimationDuration; // Normalize time between 0 and 1
                EvaluateRichText(t); // Update text animation frame
                timeElapsed += Time.deltaTime; // Increment timer

                yield return null; // Wait for the next frame
            }

            // Ensure the final state of the animation is displayed before restarting
            EvaluateRichText(1);

            // Wait briefly before restarting the animation loop
            yield return new WaitForSeconds(0.5f);
        }
    }

    private void EvaluateRichText(float timer)
    {
        animatedText.text = ""; // Reset the displayed text

        // Iterate through each character in the message
        for (int i = 0; i < message.Length; i++)
        {
            // Append the modified character with animated size to the final text
            animatedText.text += EvaluateCharRichText(message[i], message.Length, i, timer);
        }
    }

    private string EvaluateCharRichText(char c, int sLength, int cPosition, float timer)
    {
        // Define the time range in which this character should animate
        float startPoint = ((1 - charAnimationDuration) / (sLength - 1)) * cPosition;
        float endPoint = startPoint + charAnimationDuration;

        // Map the current animation progress into this character's individual animation time
        float subT = Mathf.Clamp(timer.Map(startPoint, endPoint, 0, 1), 0, 1);

        // Generate the rich text tag to modify character size dynamically
        string sizeStart = $"<size={(sizeCurve.Evaluate(subT) * sizeScale):F0}%>";
        string sizeEnd = "</size>";

        // Return the formatted character with rich text tags
        return sizeStart + c + sizeEnd;
    }
}

// Utility class containing helper functions
public static class Extensions
{
    // Maps a value from one range to another (e.g., from animation time to size scale)
    public static float Map(this float value, float fromLow, float fromHigh, float toLow, float toHigh)
    {
        if (fromHigh == fromLow) return toLow; // Prevent division by zero
        return (value - fromLow) * (toHigh - toLow) / (fromHigh - fromLow) + toLow;
    }
}
