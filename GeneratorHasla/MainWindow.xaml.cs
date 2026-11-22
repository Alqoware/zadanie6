using System.Security.Cryptography;
using System.Windows;

namespace GeneratorHasla;

public partial class MainWindow : Window
{
    private const string Letters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
    private const string Digits = "0123456789";
    private const string SpecialCharacters = "!@#$%^&*()_+-=";
    private string password = string.Empty;

    public MainWindow()
    {
        InitializeComponent();
    }

    private void GenerateButton_Click(object sender, RoutedEventArgs e)
    {
        if (!int.TryParse(LengthTextBox.Text, out int length))
        {
            ShowError("Wpisz poprawną liczbę znaków.");
            return;
        }

        if (length <= 0)
        {
            ShowError("Liczba znaków musi być większa od zera.");
            return;
        }

        if (length > 1000)
        {
            ShowError("Maksymalna obsługiwana długość hasła to 1000 znaków.");
            return;
        }

        bool useLetters = LettersCheckBox.IsChecked == true;
        bool useDigits = DigitsCheckBox.IsChecked == true;
        bool useSpecial = SpecialCheckBox.IsChecked == true;
        int requiredTypes = (useLetters ? 1 : 0) + (useDigits ? 1 : 0) + (useSpecial ? 1 : 0);

        if (requiredTypes == 0)
        {
            ShowError("Zaznacz co najmniej jeden zestaw znaków.");
            return;
        }

        if (length < requiredTypes)
        {
            ShowError("Liczba znaków jest za mała, aby uwzględnić każdy zaznaczony zestaw.");
            return;
        }

        string selectedLetters = useLetters ? Letters : string.Empty;
        string selectedDigits = useDigits ? Digits : string.Empty;
        string selectedSpecial = useSpecial ? SpecialCharacters : string.Empty;
        string allSelected = selectedLetters + selectedDigits + selectedSpecial;
        var generated = new char[length];

        // Cyfra zajmuje pierwszą pozycję, a znak specjalny drugą, jeśli są zaznaczone.
        int position = 0;
        if (useDigits)
            generated[position++] = PickRandom(selectedDigits);
        if (useSpecial)
            generated[position++] = PickRandom(selectedSpecial);
        if (useLetters)
            generated[position++] = PickRandom(selectedLetters);

        while (position < generated.Length)
            generated[position++] = PickRandom(allSelected);

        password = new string(generated);
        MessageBox.Show(this, password, "Wygenerowane hasło", MessageBoxButton.OK,
            MessageBoxImage.Information);
    }

    private static char PickRandom(string characterSet) =>
        characterSet[RandomNumberGenerator.GetInt32(characterSet.Length)];

    private void ShowError(string message) =>
        MessageBox.Show(this, message, "Błąd", MessageBoxButton.OK, MessageBoxImage.Warning);
}
