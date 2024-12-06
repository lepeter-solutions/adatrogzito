using Microsoft.Win32;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Windows;

namespace adatrogzito
{
    public partial class MainWindow : Window
    {
        private const string FilePath = "felhasznalok.json";

        public MainWindow()
        {
            InitializeComponent();
            LoadFelhasznalokToListBox();
        }

        private void uj_Click(object sender, RoutedEventArgs e)
        {
            NewEntryWindow newEntryWindow = new NewEntryWindow();
            newEntryWindow.ShowDialog();
            LoadFelhasznalokToListBox();
        }

        private void torles_Click(object sender, RoutedEventArgs e)
        {
            if (output.SelectedItem != null)
            {
                string selectedItem = output.SelectedItem.ToString();
                string[] selectedItemParts = selectedItem.Split(", ");
                string selectedEmail = selectedItemParts[2];
                string selectedName = selectedItemParts[0];

                MessageBoxResult result = MessageBox.Show($"Are you sure you want to delete {selectedName}?", "Confirm Deletion", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    if (File.Exists(FilePath))
                    {
                        string jsonData = File.ReadAllText(FilePath);
                        var felhasznalok = JsonSerializer.Deserialize<List<Felhasznalo>>(jsonData) ?? new List<Felhasznalo>();

                        var felhasznaloToRemove = felhasznalok.Find(f => f.Email == selectedEmail);
                        if (felhasznaloToRemove != null)
                        {
                            felhasznalok.Remove(felhasznaloToRemove);

                            string updatedJsonData = JsonSerializer.Serialize(felhasznalok, new JsonSerializerOptions { WriteIndented = true });
                            File.WriteAllText(FilePath, updatedJsonData);

                            LoadFelhasznalokToListBox();
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select a user to delete.", "No Selection", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void import_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "JSON files (*.json)|*.json|All files (*.*)|*.*"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                string selectedFilePath = openFileDialog.FileName;
                if (File.Exists(selectedFilePath))
                {
                    string jsonData = File.ReadAllText(selectedFilePath);
                    var felhasznalok = JsonSerializer.Deserialize<List<Felhasznalo>>(jsonData) ?? new List<Felhasznalo>();

                    string updatedJsonData = JsonSerializer.Serialize(felhasznalok, new JsonSerializerOptions { WriteIndented = true });
                    File.WriteAllText(FilePath, updatedJsonData);

                    LoadFelhasznalokToListBox();
                }
            }
        }

        private void export_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "JSON files (*.json)|*.json|All files (*.*)|*.*"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                string selectedFilePath = saveFileDialog.FileName;
                if (File.Exists(FilePath))
                {
                    File.Copy(FilePath, selectedFilePath, true);
                }
            }
        }

        private void LoadFelhasznalokToListBox()
        {
            if (File.Exists(FilePath))
            {
                string jsonData = File.ReadAllText(FilePath);
                var felhasznalok = JsonSerializer.Deserialize<List<Felhasznalo>>(jsonData) ?? new List<Felhasznalo>();

                output.Items.Clear();
                foreach (var felhasznalo in felhasznalok)
                {
                    output.Items.Add($"{felhasznalo.Nev}, {felhasznalo.Kor}, {felhasznalo.Email}, {felhasznalo.Telefonszam}, {felhasznalo.Lakcim}, {felhasznalo.Neme}, {felhasznalo.Megjegyzes}");
                }
            }
        }
    }
}