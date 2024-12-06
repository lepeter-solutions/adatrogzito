using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;

namespace adatrogzito
{
    public partial class NewEntryWindow : Window
    {
        private const string FilePath = "felhasznalok.json";

        public NewEntryWindow()
        {
            InitializeComponent();
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var felhasznalo = new Felhasznalo(
                    nev.Text,
                    int.TryParse(kor.Text, out int korValue) ? korValue : throw new ValidationException("Kor must be a positive integer"),
                    email.Text,
                    telefonszam.Text,
                    lakcim.Text,
                    (neme.SelectedItem as ComboBoxItem)?.Content.ToString(),
                    megjegyzes.Text
                );

                SaveFelhasznaloToFile(felhasznalo);

                MessageBox.Show("Felhasznalo created successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

                this.Close();
            }
            catch (ValidationException ex)
            {
                MessageBox.Show(ex.Message, "Validation Errors", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SaveFelhasznaloToFile(Felhasznalo felhasznalo)
        {
            List<Felhasznalo> felhasznalok = new List<Felhasznalo>();

            if (File.Exists(FilePath))
            {
                string existingData = File.ReadAllText(FilePath);
                felhasznalok = JsonSerializer.Deserialize<List<Felhasznalo>>(existingData) ?? new List<Felhasznalo>();
            }

            felhasznalok.Add(felhasznalo);

            string jsonData = JsonSerializer.Serialize(felhasznalok, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(FilePath, jsonData);
        }
    }
}