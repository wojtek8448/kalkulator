using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NumberButton : MonoBehaviour
{
    public int number;                // cyfra dla przycisku
    public string operation;          // operator: "+", "-", "*", "/"
    public bool isEqualButton;        // zaznaczone dla "="
    public TMP_InputField resultField; // przeciągnij tutaj TMP_InputField

    private Button button;
    private static string currentInput = "";      // wpisywana liczba
    private static float previousValue = 0;       // pierwsza liczba
    private static string currentOperator = "";   // zapisany operator

    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnClick);
    }

    void OnClick()
    {
        if (isEqualButton)
        {
            // "=" - wykonaj działanie
            float currentValue;
            if (float.TryParse(currentInput, out currentValue))
            {
                float result = Calculate(previousValue, currentValue, currentOperator);
                if (resultField != null)
                {
                    resultField.text = result.ToString();
                }
            }

            // Reset po "="
            currentInput = "";
            currentOperator = "";
            previousValue = 0;
        }
        else if (!string.IsNullOrEmpty(operation))
        {
            // operator (+, -, *, /)
            if (float.TryParse(currentInput, out previousValue))
            {
                currentOperator = operation;
                currentInput = ""; // teraz wpisujemy drugą liczbę
                if (resultField != null)
                {
                    resultField.text = ""; // czyścimy InputField
                }
            }
        }
        else
        {
            // cyfra
            currentInput += number.ToString();
            if (resultField != null)
            {
                resultField.text = currentInput;
            }
        }
    }

    float Calculate(float a, float b, string op)
    {
        switch (op)
        {
            case "+": return a + b;
            case "-": return a - b;
            case "*": return a * b;
            case "/": return b != 0 ? a / b : 0; // ochrona przed dzieleniem przez 0
            default: return b;
        }
    }
}
