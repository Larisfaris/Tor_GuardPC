using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;






namespace TorGuard
{
    public partial class Netzwerkkonfig_Page : Page
    {

        private void Netzwerkkonfig()
        {
            InitializeComponent();
            LoadConfigurationFromFile_Netzwerk(); // Beim Initialisieren Konfiguration laden
        }

            private void btnSpeichern_Click(object sender, RoutedEventArgs e)
            {
                try
                {
                    SaveConfigurationToFile_Netzwerk();

                    MessageBox.Show("Tor Konfiguration gespeichert.", "Erfolg", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Fehler beim Speichern der Konfiguration: {ex.Message}", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

        private void SaveConfigurationToFile_Netzwerk()
        {
            try
            {
                var configData_Netzwerk = new
                {
                    WLAN_SSID = txtWlan.Text,
                    WLAN_Password = txtWLANpassword.Password
                };

                // Speichern der Konfiguration
                string json = JsonSerializer.Serialize(configData_Netzwerk, new JsonSerializerOptions { WriteIndented = true });

                // Erstellen einer Instanz von RaspberryZugriff
                var raspZugriff = new RaspberryZugriff();

                // Zugriff auf Speicherpfade
                var speicherPfade = new HC_Speicherpfade(); // Passen Sie die Parameter entsprechend an

                // Pfad und Dateiname für die zu speichernde Konfigurationsdatei
                string dateiPfad = speicherPfade.Speicherpfad_Konfig;
                string dateiName = speicherPfade.Speichername_Aufnahemekonfig;

                // Aufrufen der Methode zum Speichern der Datei auf dem Raspberry
                raspZugriff.SpeichereDateiAufServer(configData_Netzwerk, dateiPfad, dateiName);

                MessageBox.Show("Die Eintragungen der Anwendung wurden gespeichert.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fehler beim Speichern der Konfiguration: {ex.Message}", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LoadConfigurationFromFile_Netzwerk()
        {
            try
            {
                HC_Speicherpfade speicherpfad = new HC_Speicherpfade();

                var raspZugriff = new RaspberryZugriff(); // Stellen Sie sicher, dass dies korrekt instanziiert wird.
                var netzwerkKonfig = raspZugriff.LadeDateiVomServer<dynamic>(speicherpfad.Speicherpfad_Konfig, speicherpfad.Speichername_Netzwerkkonfig);

                if (netzwerkKonfig != null)
                {
                    // Nehmen Sie an, dass Sie Textfelder im UI für SSID und Passwort haben
                    txtWlan.Text = netzwerkKonfig.WLAN_SSID;
                    txtWLANpassword.Password = netzwerkKonfig.WLAN_Password;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fehler beim Laden der Konfiguration: {ex.Message}", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }




        // Event Handler für den "WLAN Verbinden"-Button
        //private void btnWLAN_Click(object sender, RoutedEventArgs e)
        //{
        //    // Überprüfen, ob WLAN-SSID und Passwort nicht leer sind
        //    if (!string.IsNullOrWhiteSpace(txtWlan.Text) && !string.IsNullOrWhiteSpace(txtWLANpassword.Password))
        //    {
        //        // WLAN-Verbindung herstellen
        //        ConnectToWifi(txtWlan.Text, txtWLANpassword.Password);
        //    }
        //    else
        //    {
        //        MessageBox.Show("Bitte geben Sie WLAN-SSID und Passwort ein.", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
        //    }
        //}



        // Überschreiben der OnLoaded-Methode, um die Konfiguration beim Laden der Anwendung zu laden
        private void Netzwerkkonfig_Page_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                LoadConfigurationFromFile_Netzwerk();

                MessageBox.Show("Tor Konfiguration gespeichert.", "Erfolg", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fehler beim Speichern der Konfiguration: {ex.Message}", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            try
            { // Überprüft, ob es möglich ist, im Navigationsverlauf zurückzugehen
                if (this.NavigationService.CanGoBack)
                {
                    this.NavigationService.GoBack();
                }
                else
                {
                    // Wenn nicht zurückgegangen werden kann, navigieren Sie ggf. zu einer spezifischen Page
                    // Oder setzen Sie den Frame-Inhalt auf null, um zum Hauptinhalt zurückzukehren
                    // Beispiel: MainFrame.Content = null; // Annahme, dass 'MainFrame' der Name Ihres Hauptframes ist
                }
                SaveConfigurationToFile_Netzwerk();
               
                MessageBox.Show("Tor Konfiguration gespeichert.", "Erfolg", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fehler beim Speichern der Konfiguration: {ex.Message}", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            SaveConfigurationToFile_Netzwerk();
        }
    }
    
        

}
    



