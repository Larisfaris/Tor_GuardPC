using System;
using System.IO;
using System.Net;
using System.Text.Json;
using System.Windows;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace TorGuard
{
    public partial class RaspberryZugriff 
    {
        string benutzername = "Tor.Guard";
        string passwort = "Tor.Guard";

        //HC_Speicherpfade speicherpfade = new HC_Speicherpfade(
        //        "Tor.Guard@raspberrypi:/home/Tor.Guard/Documents/konfig",//speicherpfadTor,
        //        "Tor.Guard@raspberrypi:/home/Tor.Guard/Documents/tore",//speicherpfadKonfig,
        //        "Tor_config.json",//speichernameTorKonfig,
        //        "Netzwerk_config.json",//speichernameNetzwerkKonfig,
        //        "Empfängerlist_config.json",//speichernameEmailKonfig,
        //        "AufnahmeKonfig.json"//speichernameAufnahmeKonfig
        //    );
        //// Ersetzen Sie 'benutzername' und 'passwort' durch Ihre tatsächlichen Anmeldedaten

        public RaspberryZugriff()
        {        
        }

        //In der Konsole Tore Auflisten
        private void ListFilesInSmbShare()
        {
            try
            {
                HC_Speicherpfade speicherpfade = new HC_Speicherpfade();
                // Pfad zur SMB-Freigabe auf dem Raspberry Pi
                string smbPath1 = speicherpfade.Speicherpfad_tor;

                // Die NetworkCredential-Klasse wird verwendet, um die Anmeldedaten für den Zugriff auf die SMB-Freigabe anzugeben
                NetworkCredential credentials = new NetworkCredential(benutzername, passwort);

                // CredentialCache wird verwendet, um die Anmeldedaten zusammen mit dem Pfad der Freigabe zu speichern
                CredentialCache cache = new CredentialCache();
                cache.Add(new Uri(speicherpfade.Speicherpfad_tor), "Basic", credentials);

                // NetworkConnection stellt eine Verbindung zur SMB-Freigabe her, unter Verwendung der oben definierten Anmeldedaten
                using (new NetworkConnection(speicherpfade.Speicherpfad_tor, credentials))
                {
                    // Directory.GetFiles wird verwendet, um alle Dateien im angegebenen Pfad der SMB-Freigabe aufzulisten
                    foreach (var file in Directory.GetFiles(speicherpfade.Speicherpfad_tor))
                    {
                        // Hier können Sie mit den Dateien arbeiten, z.B. sie auflisten oder ihren Inhalt lesen
                        Console.WriteLine(file);
                    }
                }
                
            }
            catch (Exception ex)
            {
                // Im Falle eines Fehlers wird eine Nachrichtenbox mit der Fehlermeldung angezeigt
                MessageBox.Show($"Ein Fehler ist aufgetreten: {ex.Message}", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    
        // Generische Methode zum Laden einer Datei vom Server
        public T LadeDateiVomServer<T>(string dateiPfad, string dateiName) where T : class
        {
           
            string vollerPfad = Path.Combine(dateiPfad, dateiName);
            try
            {   //using (var netzwerkVerbindung = new NetworkConnection(dateiPfad, new NetworkCredential("benutzername", "passwort")))
                
                    using (var netzwerkVerbindung = new NetworkConnection(dateiPfad, new NetworkCredential(benutzername, passwort)))
                {
                    if (File.Exists(vollerPfad))
                    {
                        string json = File.ReadAllText(vollerPfad);
                        var configData = JsonSerializer.Deserialize<T>(json);
                        return configData;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fehler beim Laden der Datei: {ex.Message}", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            return null;
        }

        // Methode zum Speichern der Konfigurationsdatei auf dem Server
        public void SpeichereDateiAufServer(dynamic configData, string dateiPfad,string dateiName)
        {
                         
            try
            {
                using (var netzwerkVerbindung = new NetworkConnection(dateiPfad, new NetworkCredential(benutzername, passwort)))
                {
                    string json = JsonSerializer.Serialize(configData, new JsonSerializerOptions { WriteIndented = true });
                    string filePath = Path.Combine(dateiPfad, dateiName);
                    File.WriteAllText(filePath, json);
                    MessageBox.Show($"alles gut bruder ", "", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fehler beim Speichern der Datei: {ex.Message}", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}