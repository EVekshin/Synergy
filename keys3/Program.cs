using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

public class Calculator : Form
{
    private TextBox display;
    private double result = 0;
    private string operation = "";
    private bool isNewNumber = true;
    private string savedResult = "";

    public Calculator()
    {
        this.Text = "Калькулятор";
        this.Width = 300;
        this.Height = 400;

        // Создание дисплея
        display = new TextBox
        {
            Width = 260,
            Height = 30,
            Location = new Point(10, 10),
            ReadOnly = true,
            Text = "0",
            TextAlign = HorizontalAlignment.Right
        };

        // Массивы для создания кнопок
        string[] buttonTexts = { "7", "8", "9", "/", "4", "5", "6", "*", "1", "2", "3", "-", "0", "C", "=", "+" };
        
        // Создание кнопок
        for (int i = 0; i < 16; i++)
        {
            Button button = new Button
            {
                Text = buttonTexts[i],
                Width = 60,
                Height = 60,
                Location = new Point(10 + (i % 4) * 65, 50 + (i / 4) * 65)
            };
            button.Click += Button_Click;
            this.Controls.Add(button);
        }

        // Кнопки для сохранения/загрузки результата
        Button saveButton = new Button
        {
            Text = "Save",
            Width = 80,
            Height = 30,
            Location = new Point(10, 310)
        };
        saveButton.Click += SaveButton_Click;

        Button loadButton = new Button
        {
            Text = "Load",
            Width = 80,
            Height = 30,
            Location = new Point(100, 310)
        };
        loadButton.Click += LoadButton_Click;

        // Комбобокс для выбора размера шрифта
        ComboBox fontSizeCombo = new ComboBox
        {
            Location = new Point(190, 310),
            Width = 80
        };
        fontSizeCombo.Items.AddRange(new object[] { "8", "10", "12", "14", "16" });
        fontSizeCombo.SelectedIndex = 2;
        fontSizeCombo.SelectedIndexChanged += FontSizeCombo_SelectedIndexChanged;

        // Добавление элементов управления на форму
        this.Controls.Add(display);
        this.Controls.Add(saveButton);
        this.Controls.Add(loadButton);
        this.Controls.Add(fontSizeCombo);
    }

    private void Button_Click(object sender, EventArgs e)
    {
        Button button = (Button)sender;
        string buttonText = button.Text;

        if (char.IsDigit(buttonText[0]))
        {
            if (isNewNumber)
            {
                display.Text = buttonText;
                isNewNumber = false;
            }
            else
            {
                display.Text += buttonText;
            }
        }
        else switch (buttonText)
        {
            case "C":
                display.Text = "0";
                result = 0;
                operation = "";
                isNewNumber = true;
                break;

            case "=":
                CalculateResult();
                isNewNumber = true;
                break;

            case "+":
            case "-":
            case "*":
            case "/":
                if (!isNewNumber)
                {
                    CalculateResult();
                    operation = buttonText;
                    result = double.Parse(display.Text);
                    isNewNumber = true;
                }
                break;
        }
    }

    private void CalculateResult()
    {
        double currentNumber = double.Parse(display.Text);
        switch (operation)
        {
            case "+":
                result += currentNumber;
                break;
            case "-":
                result -= currentNumber;
                break;
            case "*":
                result *= currentNumber;
                break;
            case "/":
                if (currentNumber != 0)
                    result /= currentNumber;
                else
                {
                    MessageBox.Show("Деление на ноль невозможно!");
                    display.Text = "0";
                    result = 0;
                    operation = "";
                    isNewNumber = true;
                    return;
                }
                break;
            default:
                result = currentNumber;
                break;
        }
        display.Text = result.ToString();
    }

    private void SaveButton_Click(object sender, EventArgs e)
    {
        savedResult = display.Text;
        File.WriteAllText("calc_result.txt", savedResult);
        MessageBox.Show("Результат сохранен!");
    }

    private void LoadButton_Click(object sender, EventArgs e)
    {
        if (File.Exists("calc_result.txt"))
        {
            display.Text = File.ReadAllText("calc_result.txt");
            isNewNumber = true;
        }
    }

    private void FontSizeCombo_SelectedIndexChanged(object sender, EventArgs e)
    {
        ComboBox combo = (ComboBox)sender;
        float fontSize = float.Parse(combo.SelectedItem.ToString());
        display.Font = new Font(display.Font.FontFamily, fontSize);
    }

    [STAThread]
    static void Main()
    {
        Application.EnableVisualStyles();
        Application.Run(new Calculator());
    }
}