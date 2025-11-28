using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;
using System.Linq;

public class NumberButton : MonoBehaviour
{
    public int number;
    public string operation;
    public bool isEqualButton;
    public bool isSaveButton;
    public bool isMaxButton;
    public bool isClearButton;
    public TMP_InputField resultField;

    private Button button;
    private static string currentInput = "";
    private static float previousValue = 0;
    private static string currentOperator = "";

    private string savePath;

    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnClick);

        // Pełna ścieżka do pliku
        savePath = Path.Combine(Application.persistentDataPath, "wyniki.txt");

        // Utwórz katalog, jeśli nie istnieje
        string directory = Path.GetDirectoryName(savePath);
        if (!Directory.Exists(directory))
            Directory.CreateDirectory(directory);

        // Utwórz plik, jeśli nie istnieje
        if (!File.Exists(savePath))
            File.WriteAllText(savePath, "");

      
    }

    void OnClick()
    {
        // 1. CLEAR
        if (isClearButton)
        {
            File.WriteAllText(savePath, "");
            if (resultField != null)
                resultField.text = "Wyniki usunięte";
            return;
        }

        // 2. SAVE
        if (isSaveButton)
        {
            if (resultField != null && !string.IsNullOrEmpty(resultField.text))
                File.AppendAllText(savePath, resultField.text + "\n");
            return;
        }

        // 3. MAX
        if (isMaxButton)
        {
            ShowMaxResult();
            return;
        }

        // 4. "="
        if (isEqualButton)
        {
            float currentValue;
            if (float.TryParse(currentInput, out currentValue))
            {
                float result = Calculate(previousValue, currentValue, currentOperator);

                if (resultField != null)
                    resultField.text = result.ToString();

                // automatyczny zapis wyniku
                File.AppendAllText(savePath, result.ToString() + "\n");
            }

            currentInput = "";
            currentOperator = "";
            previousValue = 0;
            return;
        }

        // 5. Operator
        if (!string.IsNullOrEmpty(operation))
        {
            if (float.TryParse(currentInput, out previousValue))
            {
                currentOperator = operation;
                currentInput = "";
                if (resultField != null)
                    resultField.text = "";
            }
            return;
        }

        // 6. Cyfra
        currentInput += number.ToString();
        if (resultField != null)
            resultField.text = currentInput;
    }

    void ShowMaxResult()
    {
        if (!File.Exists(savePath))
        {
            if (resultField != null)
                resultField.text = "Brak zapisów";
            return;
        }

        var numbers = File.ReadAllLines(savePath)
            .Select(l => {
                float v;
                return float.TryParse(l, out v) ? v : float.NaN;
            })
            .Where(v => !float.IsNaN(v))
            .ToList();

        if (numbers.Count == 0)
        {
            if (resultField != null)
                resultField.text = "Brak danych";
            return;
        }

        float max = numbers.Max();
        if (resultField != null)
            resultField.text = max.ToString();
    }

    float Calculate(float a, float b, string op)
    {
        switch (op)
        {
            case "+": return a + b;
            case "-": return a - b;
            case "*": return a * b;
            case "/": return b != 0 ? a / b : 0;
            case "^": return Mathf.Pow(a, b);
            default: return b;
        }
    }
}
