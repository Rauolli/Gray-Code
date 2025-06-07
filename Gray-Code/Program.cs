using System;

public static class CodeConverter
{
    // --- Umwandlungen zwischen Binär-String und Dezimal ---

    public static int BinaryStringToDecimal(string binaryString)
    {
        if (string.IsNullOrEmpty(binaryString))
        {
            throw new ArgumentException("Binary string cannot be null or empty.");
        }
        return Convert.ToInt32(binaryString, 2);
    }

    public static string DecimalToBinaryString(int decimalValue)
    {
        if (decimalValue < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(decimalValue), "Negative decimal values are not supported for simple binary string conversion.");
        }
        if (decimalValue == 0)
        {
            return "0";
        }
        return Convert.ToString(decimalValue, 2);
    }

    // --- Umwandlungen zwischen Gray-Code-String und Dezimal ---

    public static int GrayStringToDecimal(string grayString)
    {
        string binaryString = GrayToBinaryString(grayString);
        return BinaryStringToDecimal(binaryString);
    }

    public static string DecimalToGrayString(int decimalValue)
    {
        string binaryString = DecimalToBinaryString(decimalValue);
        return BinaryToGrayString(binaryString);
    }

    // --- Ursprüngliche Umwandlungen zwischen Binär-Code-String und Gray-Code-String ---

    public static string GrayToBinaryString(string grayCode)
    {
        if (string.IsNullOrEmpty(grayCode))
        {
            return string.Empty;
        }

        char[] binaryChars = new char[grayCode.Length];
        binaryChars[0] = grayCode[0];

        for (int i = 1; i < grayCode.Length; i++)
        {
            binaryChars[i] = (char)(((grayCode[i] - '0') ^ (binaryChars[i - 1] - '0')) + '0');
        }

        return new string(binaryChars);
    }

    public static string BinaryToGrayString(string binaryCode)
    {
        if (string.IsNullOrEmpty(binaryCode))
        {
            return string.Empty;
        }

        char[] grayChars = new char[binaryCode.Length];
        grayChars[0] = binaryCode[0];

        for (int i = 1; i < binaryCode.Length; i++)
        {
            grayChars[i] = (char)(((binaryCode[i] - '0') ^ (binaryCode[i - 1] - '0')) + '0');
        }

        return new string(grayChars);
    }

    // --- Main-Methode für Benutzereingaben ---
    public static void Main(string[] args)
    {
        bool running = true;
        while (running)
        {
            Console.WriteLine("\n--- Code-Konverter Menü ---");
            Console.WriteLine("1. Wert als Dezimalzahl eingeben");
            Console.WriteLine("2. Wert als Binär-Code-String eingeben");
            Console.WriteLine("3. Wert als Gray-Code-String eingeben");
            Console.WriteLine("0. Beenden");
            Console.Write("Bitte wählen Sie eine Option: ");

            string choice = Console.ReadLine();
            Console.WriteLine(); // Leerzeile für bessere Lesbarkeit

            string inputString = "";
            int decimalInput = 0;
            string binaryString = "";
            string grayString = "";
            int grayAsBinaryDecimal = 0; // Neu: Für den Fall, dass Gray-Code als Binär interpretiert wird

            try
            {
                switch (choice)
                {
                    case "1":
                        Console.Write("Geben Sie eine Dezimalzahl ein: ");
                        inputString = Console.ReadLine();
                        decimalInput = int.Parse(inputString);

                        binaryString = DecimalToBinaryString(decimalInput);
                        grayString = DecimalToGrayString(decimalInput);
                        // Gray als Binär interpretieren (redundant hier, da decimalInput schon der binäre Wert ist)
                        grayAsBinaryDecimal = BinaryStringToDecimal(grayString);
                        break;

                    case "2":
                        Console.Write("Geben Sie einen Binär-Code-String ein: ");
                        inputString = Console.ReadLine();
                        binaryString = inputString;

                        decimalInput = BinaryStringToDecimal(binaryString);
                        grayString = BinaryToGrayString(binaryString);
                        // Gray als Binär interpretieren (wieder der Wert der grayString, als ob er binär wäre)
                        grayAsBinaryDecimal = BinaryStringToDecimal(grayString);
                        break;

                    case "3":
                        Console.Write("Geben Sie einen Gray-Code-String ein: ");
                        inputString = Console.ReadLine();
                        grayString = inputString;

                        // Hier ist der entscheidende Punkt:
                        // Gray in Binär umwandeln, um den ECHTEN Dezimalwert zu erhalten
                        binaryString = GrayToBinaryString(grayString);
                        decimalInput = BinaryStringToDecimal(binaryString);

                        // Den Gray-String als BINÄR interpretieren
                        grayAsBinaryDecimal = BinaryStringToDecimal(grayString);
                        break;

                    case "0":
                        running = false;
                        Console.WriteLine("Programm wird beendet. Auf Wiedersehen!");
                        break;

                    default:
                        Console.WriteLine("Ungültige Auswahl. Bitte versuchen Sie es erneut.");
                        break;
                }

                if (running && !string.IsNullOrEmpty(inputString))
                {
                    Console.WriteLine("\n--- Umwandlungsergebnisse ---");
                    Console.WriteLine($"Eingabe ({GetInputType(choice)}): {inputString}");
                    Console.WriteLine($"Binär: {binaryString}");
                    Console.WriteLine($"Dezimal (vom Binär-Code): {decimalInput}"); // Echter Dezimalwert
                    Console.WriteLine($"Gray-Code: {grayString}");

                    // NEUE AUSGABE:
                    // Dieser Wert zeigt, was der Gray-Code wäre, wenn er "fälschlicherweise" als Binär interpretiert wird.
                    Console.WriteLine($"Dezimal (Gray-Code als Binär interpretiert): {grayAsBinaryDecimal}");
                }
            }
            catch (FormatException)
            {
                Console.WriteLine("Fehler: Ungültige Eingabe. Bitte geben Sie eine gültige Zahl oder einen gültigen Binär/Gray-String ein.");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Fehler: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ein unerwarteter Fehler ist aufgetreten: {ex.Message}");
            }

            if (running)
            {
                Console.WriteLine("\nDrücken Sie eine beliebige Taste, um fortzufahren...");
                Console.ReadKey();
            }
        }
    }

    private static string GetInputType(string choice)
    {
        switch (choice)
        {
            case "1": return "Dezimal";
            case "2": return "Binär";
            case "3": return "Gray-Code";
            default: return "Unbekannt";
        }
    }
}