using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json;


namespace TorGuard
{
    public class Tor_Verwaltungsstelle
    {
        Dictionary<string, HC_Tore> tore = new Dictionary<string, HC_Tore>();

        // Methode zum Laden aller Tore aus den JSON-Dateien
        public void LoadAllTore()
        {
            HC_Speicherpfade speicherpfade = new HC_Speicherpfade();
            RaspberryZugriff raspZugriff = new RaspberryZugriff();

            string[] torFiles = Directory.GetFiles(speicherpfade.Speicherpfad_tor, "*.json");
            foreach (string filePfad in torFiles)
            {
                string dateiName = Path.GetFileName(filePfad); // Holt den Dateinamen aus dem Pfad
                try
                {
                    // Korrekter Aufruf unter Verwendung von Pfad und Dateinamen
                    HC_Tore tor = raspZugriff.LadeDateiVomServer<HC_Tore>(speicherpfade.Speicherpfad_tor, dateiName);
                    if (tor != null && !string.IsNullOrEmpty(tor.TorName))
                    {
                        // Verarbeiten Sie das geladene HC_Tore-Objekt weiter
                    }
                }
                catch (Exception ex)
                {
                    // Fehlerbehandlung, z.B. Logging
                    Console.WriteLine($"Fehler beim Laden der Torkonfiguration: {ex.Message}");
                }
            }
        }


        // Methode zum Hinzufügen oder Aktualisieren eines Tors
        public void AddOrUpdateTor(HC_Tore tor)
        {
            if (tor != null && !string.IsNullOrEmpty(tor.TorName))
            {
                tore[tor.TorName] = tor;
                SaveTor(tor); // Speichere das Tor sofort
            }
        }

        // Methode zum Speichern eines spezifischen Tors
        private void SaveTor(HC_Tore tor)
        {
            HC_Speicherpfade speicherpfad = new HC_Speicherpfade();
            string filePath = Path.Combine(speicherpfad.Speicherpfad_tor, $"{tor.TorName}.json");
            string json = JsonSerializer.Serialize(tor, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(filePath, json);
        }

        // Methode, um ein Tor anhand des Namens zu bekommen
        public HC_Tore GetTorByName(string name)
        {
            tore.TryGetValue(name, out HC_Tore tor);
            return tor;
        }


        public List<string> GetAllToreNames()
        {
            // Annahme, dass eine interne Collection von Toren existiert
            return tore.Values.Select(tor => tor.TorName).ToList();
        }

        public List<HC_Tore> GetAllTore()
        {
            // Angenommen, es gibt eine interne Sammlung, die alle Tore speichert.
            // Diese Sammlung könnte ein Dictionary, eine List oder eine andere Collection sein.
            return tore.Values.ToList();
        }

        public HC_Tore FindTorByPin(int pinNummer, bool isIndoor)
        {
            // Iteriere durch alle Tore, um das entsprechende Tor zu finden.
            foreach (var tor in tore.Values)
            {
                if (isIndoor && tor.GpioPinIndoor == pinNummer)
                {
                    return tor; // Wenn es ein Indoor-Pin ist und die Pinnummern übereinstimmen.
                }
                else if (!isIndoor && tor.GpioPinOutdoor == pinNummer)
                {
                    return tor; // Wenn es ein Outdoor-Pin ist und die Pinnummern übereinstimmen.
                }
            }

            return null; // Wenn kein passendes Tor gefunden wurde.
        }


        // Methode, um den Crash-Speicherort eines Tors zu erhalten
        public string GetCrashSpeicherortByName(string torName)
        {
            // Verwende GetTorByName, um das Tor-Objekt zu erhalten
            var tor = GetTorByName(torName);

            // Überprüfe, ob das Tor gefunden wurde
            if (tor != null)
            {
                // Gibt den Speicherort des Crashes zurück, wenn das Tor existiert
                return tor.SpeicherortCrash;
            }
            else
            {
                // Gibt null oder eine geeignete Nachricht zurück, falls das Tor nicht gefunden wurde
                return null; // oder "Tor nicht gefunden."
            }
        }
    }
}
