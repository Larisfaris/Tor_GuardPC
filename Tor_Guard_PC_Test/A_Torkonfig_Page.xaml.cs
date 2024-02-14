using System.Text.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Runtime.ConstrainedExecution;


//Mögliche Fehler und Verbesserungsvorschläge:
//Parameterloser Konstruktor: Der parameterlose Konstruktor lädt auch die Konfigurationsdatei, aber ohne eine torVerwaltung-Instanz zu initialisieren oder zu übergeben. Dies könnte zu einer NullReferenceException führen, wenn torVerwaltung verwendet wird. Stellen Sie sicher, dass torVerwaltung in jedem Fall korrekt initialisiert wird.
//Doppelte Initialisierung in Konstruktoren: Die Logik zum Laden der Konfigurationsdatei ist in beiden Konstruktoren dupliziert. Es wäre besser, diese Logik in eine separate Methode auszulagern und von beiden Konstruktoren aus aufzurufen.
//Speichern-Logik: In der btnSpeichern_Click-Methode wird ein neues Tor-Objekt erstellt, auch wenn ein existierendes Tor ausgewählt und bearbeitet wird. Dies könnte zu unbeabsichtigten Ergebnissen führen, besonders wenn Sie versuchen, ein existierendes Tor zu aktualisieren. Stattdessen sollten Sie nur ein neues Tor-Objekt erstellen, wenn wirklich ein neues Tor hinzugefügt wird.
//Fehlerbehandlung: Es ist gut, dass Sie Fehlermeldungen anzeigen, wenn beim Laden oder Speichern ein Fehler auftritt. Es könnte jedoch hilfreich sein, zusätzliche Validierungen für Benutzereingaben hinzuzufügen, um zu verhindern, dass ungültige Daten gespeichert werden.
//UI-Feedback: Nachdem ein neues Tor hinzugefügt oder Änderungen gespeichert wurden, könnte es nützlich sein, dem Benutzer ein visuelles Feedback zu geben, z.B. durch das Leeren der Textfelder oder das Aktualisieren der Auswahl in der ListBox, um die Änderungen zu reflektieren.
//Verbesserung der Benutzerfreundlichkeit: Für eine bessere Benutzererfahrung könnten Sie Labels neben den Textfeldern hinzufügen, um klarzustellen, was in jedes Feld eingegeben werden soll.


//Speicherorthinzufügen

namespace TorGuard
{
    public partial class Torkonfig_Page : Page 
    {
        
        private Tor_Verwaltungsstelle torVerwaltung;

        private static void InitializeTore()
        {
            //tor.Add(new Tor(
            //1
            //, 11
            //, 12
            //, "rstp://192.168.1.100"
            //, "rstp://192.168.1.100"
            //, "/home/Tor.Guard/Videos/Tor.Guard/Tor_1/Cam_indoor"
            //, "/home/Tor.Guard/Videos/Tor.Guard/Tor_1/Cam_Outdoor"
            //, "/home/Tor.Guard/Videos/Tor.Guard/Crash"));

            //tore.Add(new Tor(2, 12, "camera_url_2.1", "camera_url_2.2", "/home/Tor.Guard/Videos/Tor.Guard/Tor_2/Cam_indoor", "/home/Tor.Guard/Videos/Tor.Guard/Tor_2/Cam_Outdor",false, DateTime.MinValue));
        }

        public Torkonfig_Page() //TorVerwaltung torVerwaltung
        {
            InitializeComponent();
            var torVerwaltung = new Tor_Verwaltungsstelle();
            torVerwaltung.LoadAllTore();
            LoadConfigurationFromFile_Tor(); // Beim Initialisieren Konfiguration laden
        }

        private void btnSpeichern_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Speichere die Konfiguration
                SaveConfigurationToFile_Tor();
                RefreshTorList();

                MessageBox.Show("Tor Konfiguration gespeichert.", "Erfolg", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fehler beim Speichern der Konfiguration: {ex.Message}", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void btnAddTor_Click(object sender, RoutedEventArgs e)
        {
            AddNewTor();
        }



        // Beim Hinzufügen oder Speichern eines Tors
        private void AddNewTor()
        {
            var neuesTor = new HC_Tore(
                txtTorName.Text, // Name des Tors
                Convert.ToInt32(txtGpioPinIndoor.Text), // GPIO Pin Indoor
                Convert.ToInt32(txtGpioPinOutdoor.Text), // GPIO Pin Outdoor
                txtKameraIndoorUrl.Text, // Kamera Indoor URL
                txtKameraOutdoorUrl.Text, // Kamera Outdoor URL
                txtSpeicherortKameraIndoor.Text, // Speicherort Kamera Indoor
                txtSpeicherortKameraOutdoor.Text, // Speicherort Kamera Outdoor
                txtSpeicherortCrash.Text); // Speicherort für Crash-Informationen

            // Hinzufügen oder Aktualisieren des Tors in der Verwaltung
            torVerwaltung.AddOrUpdateTor(neuesTor);

            // UI aktualisieren
            RefreshTorList();
        }

        //Methode zum speichern von eingaben im Layout              
        private void SaveConfigurationToFile_Tor()
        {
            try
            {// Überprüfen, ob ein Tor ausgewählt ist

                if (listBoxTore.SelectedItem != null)
                {
                    var ausgewähltesTor = torVerwaltung.GetTorByName(listBoxTore.SelectedItem.ToString());
                    var raspZugriff = new RaspberryZugriff();
                    var speicherpfade = new HC_Speicherpfade(/* Konstruktorparameter mit echten Pfaden und Namen */);

                    string dateiPfad = speicherpfade.Speicherpfad_tor; // Pfad für Tor-Konfigurationen
                    string dateiName = $"{ausgewähltesTor.TorName}.json"; // Dynamischer Dateiname basierend auf dem Tor-Namen

                    raspZugriff.SpeichereDateiAufServer(ausgewähltesTor, dateiPfad, dateiName);

                    MessageBox.Show("Tor Konfiguration gespeichert.", "Erfolg", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fehler beim Speichern der Konfiguration: {ex.Message}", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ListBoxTore_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (listBoxTore.SelectedItem != null)
            {
                // Hole das ausgewählte Tor aus dem Dictionary
                var ausgewähltesTor = torVerwaltung.GetTorByName(listBoxTore.SelectedItem.ToString());
                
                // Aktualisiere die UI mit den Eigenschaften des ausgewählten Tors
                txtTorName.Text = ausgewähltesTor.TorName;
                txtGpioPinIndoor.Text = ausgewähltesTor.GpioPinIndoor.ToString();
                txtGpioPinOutdoor.Text = ausgewähltesTor.GpioPinOutdoor.ToString();
                txtKameraIndoorUrl.Text = ausgewähltesTor.KameraIndoorUrl;
                txtKameraOutdoorUrl.Text = ausgewähltesTor.KameraOutdoorUrl;
                txtSpeicherortKameraIndoor.Text = ausgewähltesTor.SpeicherortKameraIndoor;
                txtSpeicherortKameraOutdoor.Text = ausgewähltesTor.SpeicherortKameraOutdoor;
                txtSpeicherortCrash.Text = ausgewähltesTor.SpeicherortCrash;
            }
        }




        private void LoadConfigurationFromFile_Tor()
        {
            // Eine Methode in TorVerwaltungsstelle, die alle Tor-Namen zurückgibt
            var torNamen = torVerwaltung.GetAllToreNames();
            listBoxTore.ItemsSource = torNamen;
            RefreshTorList();
        }

        private void RefreshTorList()
        {
            // Angenommen, GetAllToreNames() gibt die aktualisierte Liste der Tornamen zurück
            listBoxTore.ItemsSource = null; // UI-Reset
            listBoxTore.ItemsSource = torVerwaltung.GetAllToreNames(); // Liste aktualisieren
        }


        // Überschreiben der OnClosing-Methode, um die Konfiguration beim Schließen der Anwendung zu speichern
        private void Tor_Page_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                LoadConfigurationFromFile_Tor();


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
                // Beispiel: MainFrame.Content = null; // Annahme, dass 'MainFrame' der Name Ihres Hauptframes ist
            }
            
                SaveConfigurationToFile_Tor();

                MessageBox.Show("Tor Konfiguration gespeichert.", "Erfolg", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fehler beim Speichern der Konfiguration: {ex.Message}", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        // Hier könnten Sie die Tor-Instanz in einer Liste speichern, an eine Datenbank senden oder eine andere Aktion ausführen


    }

}


