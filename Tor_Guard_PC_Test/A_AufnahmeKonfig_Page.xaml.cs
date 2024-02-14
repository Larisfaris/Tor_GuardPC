using System;
using System.IO;
using System.Runtime.ConstrainedExecution;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using System.Xml.Linq;
using TorGuard;

namespace TorGuard
{
    public partial class AufnahmeKonfig_Page : Page
    {
        
       //private AufnahmeKonfig aufnahmeKonfig;

        public AufnahmeKonfig_Page()
        {
            InitializeComponent();
            LoadConfiguration();

        }

        public void LoadConfiguration()
        {
            try
            {
                // Zugriff auf die Singleton-Instanz
                var konfig = HC_AufnahmeKonfig.Instance;

               

                // Aktualisieren der UI-Elemente mit den Werten aus der Konfiguration
                txtStundenProDatei.Text = konfig.StundenProDatei.ToString();
                txtBackupDays.Text = konfig.BackupDays.ToString();
                txtSpeicherIntervallMinute.Text = konfig.AktualisierungServerDatei.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fehler beim Laden der Konfiguration: {ex.Message}", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void SaveConfiguration_Aufnahme()
        {
            try
            {
                // Zugriff auf die Singleton-Instanz
                var konfig = HC_AufnahmeKonfig.Instance;

                // Aktualisieren der Konfigurationswerte basierend auf den UI-Elementen
                konfig.StundenProDatei = double.Parse(txtStundenProDatei.Text);
                konfig.BackupDays = double.Parse(txtBackupDays.Text);
                konfig.AktualisierungServerDatei = double.Parse(txtSpeicherIntervallMinute.Text);

                // Erstellen der JSON-Repräsentation der Konfiguration
                string json = JsonSerializer.Serialize(konfig, new JsonSerializerOptions { WriteIndented = true });

                // Erstellen einer Instanz von RaspberryZugriff
                RaspberryZugriff raspZugriff = new RaspberryZugriff();

                // Pfad und Dateiname definieren
                HC_Speicherpfade speicherpfade = new HC_Speicherpfade(); // Stellen Sie sicher, dass Speicherpfade korrekt initialisiert werden
                string dateiPfad = speicherpfade.Speicherpfad_Konfig; // Verwenden Sie den korrekten Speicherpfad
                string dateiName = "AufnahmeKonfig.json"; // Verwenden Sie den korrekten Dateinamen

                // Speichern der Konfiguration auf dem Server
                raspZugriff.SpeichereDateiAufServer(json, dateiPfad, dateiName);

                MessageBox.Show("Konfiguration gespeichert.", "Erfolg", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fehler beim Speichern der Konfiguration: {ex.Message}", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            SaveConfiguration_Aufnahme();
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            
                // Überprüft, ob es möglich ist, im Navigationsverlauf zurückzugehen
                if (this.NavigationService.CanGoBack)
                {
                    this.NavigationService.GoBack();
                }
                else
                {
                    // Wenn nicht zurückgegangen werden kann, navigieren Sie ggf. zu einer spezifischen Page
                    // Oder setzen Sie den Frame-Inhalt auf null, um zum Hauptinhalt zurückzukehren
                     //MainFrame.Content = null; // Annahme, dass 'MainFrame' der Name Ihres Hauptframes ist
                }
           
        }
    }
}
