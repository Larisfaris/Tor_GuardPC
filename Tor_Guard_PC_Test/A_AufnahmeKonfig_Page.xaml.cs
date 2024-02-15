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
                RaspberryZugriff raspZugriff = new RaspberryZugriff();
                string dateiPfad = HC_Speicherpfade.Instance.Speicherpfad_Konfig;
                string dateiName = HC_Speicherpfade.Instance.Speichername_Aufnahemekonfig;

                // Laden der Konfigurationsdatei als JsonDocument
                var configContent = raspZugriff.LadeDateiVomServer<string>(dateiPfad, dateiName);
                if (configContent == null)
                {
                    MessageBox.Show("Konfigurationsdatei konnte nicht geladen werden.", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                JsonDocument doc;
                try
                {
                    doc = JsonDocument.Parse(configContent);
                }
                catch (JsonException je)
                {
                    MessageBox.Show($"Fehler beim Parsen der Konfigurationsdatei: {je.Message}", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                JsonElement root = doc.RootElement;

                if (root.TryGetProperty("StundenProDatei", out JsonElement stundenProDateiElement))
                {
                    txtStundenProDatei.Text = stundenProDateiElement.ToString();
                }

                if (root.TryGetProperty("BackupDays", out JsonElement backupDaysElement))
                {
                    txtBackupDays.Text = backupDaysElement.ToString();
                }

                if (root.TryGetProperty("AktualisierungServerDatei", out JsonElement aktualisierungServerDateiElement))
                {
                    txtSpeicherIntervallMinute.Text = aktualisierungServerDateiElement.ToString();
                }
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
                //// Zugriff auf die Singleton-Instanz
                //var konfig = HC_AufnahmeKonfig.Instance;
                //// Speichere auf dem Server
                //RaspberryZugriff raspZugriff = new RaspberryZugriff();
                //raspZugriff.SpeichereDateiAufServer(configData_Email, dateiPfad, dateiName);
                //// Aktualisieren der Konfigurationswerte basierend auf den UI-Elementen

                var configData_Aufnahme = new
                {
                    StundenProDatei = double.Parse(txtStundenProDatei.Text),
                        BackupDays = double.Parse(txtBackupDays.Text),
                        AktualisierungServerDatei = double.Parse(txtSpeicherIntervallMinute.Text)

                };

                //// Erstellen der JSON-Repräsentation der Konfiguration
                //string json = JsonSerializer.Serialize(konfig, new JsonSerializerOptions { WriteIndented = true });

                // Erstellen einer Instanz von RaspberryZugriff
                RaspberryZugriff raspZugriff = new RaspberryZugriff();

                //// Pfad und Dateiname definieren

                string dateiPfad = HC_Speicherpfade.Instance.Speicherpfad_Konfig; // Verwenden Sie den korrekten Speicherpfad
                string dateiName = HC_Speicherpfade.Instance.Speichername_Aufnahemekonfig; // Verwenden Sie den korrekten Dateinamen

                // Speichern der Konfiguration auf dem Server
                raspZugriff.SpeichereDateiAufServer(configData_Aufnahme, dateiPfad, dateiName);

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


            //this.NavigationService.GoBack();

            //Überprüft, ob es möglich ist, im Navigationsverlauf zurückzugehen
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
