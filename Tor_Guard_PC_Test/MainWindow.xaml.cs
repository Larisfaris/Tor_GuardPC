using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Windows;
using System.Windows.Controls;
using System.Net;
using System.Diagnostics;
using System.Runtime.Intrinsics.X86;
using System.IO;
using System.Text.Json;
using System.Xml.Linq;
using System.Reflection.Metadata;

//Login Benutzername: Tor_Guard@web.de
//Login Passwort:TorGuard

//Posteingang(IMAP)
//Server: imap.web.de
//Port: 993
//Verschlüsselung: SSL oder Verschlüsselung
//Postausgang(SMTP)
//Server: smtp.web.de
//Port: 587
//Verschlüsselung: STARTTLS, TLS oder Verschlüsselung

//Steht in einem Programm "STARTTLS" nicht zur Verfügung, nutzen Sie bitte das Protokoll "TLS". Existiert auch hierfür keine Option, genügt es, die Option "Verschlüsselung" zu aktivieren. Alternativ können Sie für den Postausgangsserver auch Port 465 mit der Verschlüsselung "SSL" nutzen.

//Bitte beachten Sie: Ein TLS-Protokoll können Sie nur dann verwenden, wenn Ihr E-Mail-Programm über die aktuellen TLS-Versionen 1.2 oder 1.3 verfügt. Die TLS-Versionen 1.0 und 1.1 werden aus Sicherheitsgründen nicht mehr von uns unterstützt.

namespace TorGuard
{

    public partial class MainWindow : Window
    {
        // Liste zum Speichern der E-Mail-Adressen
        private List<string> emailAddresses = new List<string>();
      


        public MainWindow()
        {
            InitializeComponent();
            string benutzername = "Tor.Guard";
            string passwort = "Tor.Guard";

          

            LoadConfigurationFromFile_Mail();
        }


        // Event Handler für den "Hinzufügen"-Button
        private void btnAddEmail_Click(object sender, RoutedEventArgs e)
        {
            // Überprüfen, ob die Textbox für E-Mail-Adressen nicht leer ist
            if (!string.IsNullOrWhiteSpace(txtEmailAddress.Text))
            {
                // E-Mail-Adresse zur Liste hinzufügen
                emailAddresses.Add(txtEmailAddress.Text);

                // E-Mail-Adressen in der Listbox aktualisieren
                RefreshEmailList();
            }
            else
            {
                MessageBox.Show("Bitte geben Sie eine gültige E-Mail-Adresse ein.", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            // Textbox leeren
            txtEmailAddress.Clear();
        }

        // Event Handler für den "Löschen"-Button
        private void btnRemoveEmail_Click(object sender, RoutedEventArgs e)
        {
            // Überprüfen, ob mindestens eine E-Mail-Adresse ausgewählt ist
            if (lstEmailAddresses.SelectedItems.Count > 0)
            {
                // Ausgewählte E-Mail-Adressen aus der Liste entfernen
                foreach (var selectedEmail in lstEmailAddresses.SelectedItems.Cast<string>().ToList())
                {
                    emailAddresses.Remove(selectedEmail);
                }

                // E-Mail-Adressen in der Listbox aktualisieren
                RefreshEmailList();
            }
            else
            {
                MessageBox.Show("Bitte wählen Sie mindestens eine E-Mail-Adresse zum Löschen aus.", "Hinweis", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        //public async void btnTest_Click(object sender, RoutedEventArgs e)
        //{
        //    // Überprüfen, ob mindestens eine E-Mail-Adresse vorhanden ist
        //    if (emailAddresses.Count > 0)
        //    {       
        //        C_EmailService service = new C_EmailService();
        //        service.EmailsAsync();

        //    }
        //    else
        //    {
        //        MessageBox.Show("Bitte fügen Sie mindestens eine E-Mail-Adresse hinzu.", "Hinweis", MessageBoxButton.OK, MessageBoxImage.Information);
        //    }
        //}



        // Methode zur Aktualisierung der E-Mail-Liste in der Listbox
        private void RefreshEmailList()
        {
            // Listbox leeren und mit aktualisierten E-Mail-Adressen füllen
            lstEmailAddresses.Items.Clear();
            foreach (var empfängeradresse in emailAddresses)
            {
                lstEmailAddresses.Items.Add(empfängeradresse);
            }
        }



        //Methode zum Verbindung mit WLAN
        private void ConnectToWifi(string ssid, string password)
        {
            try
            {
                // Überprüfen, ob WLAN-SSID und Passwort nicht null sind
                if (!string.IsNullOrWhiteSpace(ssid) && !string.IsNullOrWhiteSpace(password))
                {
                    // WLAN-Verbindung herstellen
                    Process.Start("netsh", $"wlan connect name=\"{ssid}\" ssid=\"{ssid}\" keyMaterial=\"{password}\"");
                    MessageBox.Show($"Erfolgreich mit WLAN \"{ssid}\" verbunden.", "Erfolg", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("WLAN-SSID und Passwort dürfen nicht leer sein.", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fehler beim Herstellen der WLAN-Verbindung: {ex.Message}", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnTorConfig_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new Torkonfig_Page());
        }



        // Ereignishandler für den Klick auf den "Netzwerk Konfiguration" Button
        private void btnNetworkConfig_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new Netzwerkkonfig_Page()); // Hier wird die NetworkConfigPage angezeigt
        }

        // Ereignishandler für den Klick auf den "Aufnahmekonfig" Button
        private void btnAufnahemConfig_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new AufnahmeKonfig_Page()); // Hier wird die NetworkConfigPage angezeigt
        }



        private void btnSpeichern_Click(object sender, RoutedEventArgs e)
        { //Wenn der Butten betätigt wird soll abgespeichert werden 
            SaveConfigurationToFile_Mail();

            MessageBox.Show("Die Eintragungen der Anwendung wurden gespeichert.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);

        }

        //Methode zum speichern von eingaben im Layout              
        private void SaveConfigurationToFile_Mail()
        {
            try
            {
                var configData_Email = new
                {

                    EmailAddresses = emailAddresses,
                    SMTP = new
                    {
                        Email = txtsEmail.Text,
                        Password = txtEmailpassword.Password,
                        Address = txtsmtpadre.Text,
                        Port = txtSmtpNr.Text
                    }
                };

                string json = JsonSerializer.Serialize(configData_Email, new JsonSerializerOptions { WriteIndented = true });

                // Instanz von RaspberryZugriff erstellen
                RaspberryZugriff raspZugriff = new RaspberryZugriff();
                // Pfad und Dateiname holen
                //HC_Speicherpfade speicherpfade = new HC_Speicherpfade();

                string dateiPfad = HC_Speicherpfade.Instance.Speicherpfad_Konfig; // Annahme, dass dies der richtige Pfad ist
                string dateiName = HC_Speicherpfade.Instance.Speichername_Emailkonfig;

                // Speichere auf dem Server
                raspZugriff.SpeichereDateiAufServer(configData_Email, dateiPfad, dateiName);

                MessageBox.Show("Die E-Mail-Konfiguration wurde gespeichert.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fehler beim Speichern der E-Mail-Konfiguration: {ex.Message}", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        // Methode zum Laden der Labels und ListBox-Inhalte aus einer Datei
        private void LoadConfigurationFromFile_Mail()
        {


           // MessageBox.Show("Die Anwendung wurde geöffnet und initialisiert.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
            try
            {
                // Erstellen einer Instanz von RaspberryZugriff
                RaspberryZugriff raspZugriff = new RaspberryZugriff();

                // Pfadinformationen aus der Speicherpfade-Klasse
                
                string dateiPfad = HC_Speicherpfade.Instance.Speicherpfad_Konfig;
                string dateiName = HC_Speicherpfade.Instance.Speichername_Emailkonfig;
                // Überprüfen, ob die Werte nicht null sind
                if (dateiPfad != null && dateiName != null)
                {

                    // Laden der Konfiguration vom Server
                    var configData = raspZugriff.LadeDateiVomServer<dynamic>(dateiPfad, dateiName);

                    
                    if (configData.TryGetProperty("SMTP", out JsonElement smtpElement))
                    {
                        // Zugriff auf die SMTP-Eigenschaften

                        if (configData.TryGetProperty("EmailAddresses", out JsonElement emailAddressesElement))
                        {
                            emailAddresses = emailAddressesElement.EnumerateArray().Select(email => email.GetString()).ToList();
                        }
                        if (smtpElement.TryGetProperty("Email", out JsonElement emailElement))
                        {
                            txtsEmail.Text = emailElement.GetString();
                        }

                        if (smtpElement.TryGetProperty("Password", out JsonElement passwordElement))
                        {
                            txtEmailpassword.Password = passwordElement.GetString();
                        }

                        if (smtpElement.TryGetProperty("Address", out JsonElement addressElement))
                        {
                            txtsmtpadre.Text = addressElement.GetString();
                        }

                        if (smtpElement.TryGetProperty("Port", out JsonElement portElement))
                        {
                            txtSmtpNr.Text = portElement.GetString();
                        }
                    }
                    //emailAddresses = ((JsonElement)configData.EmailAddresses).EnumerateArray().Select(email => email.GetString()).ToList();

                    //txtsEmail.Text = configData?.SMTP?.Email;
                    //    txtEmailpassword.Password = configData?.SMTP?.Password;
                    //    txtsmtpadre.Text = configData?.SMTP?.Address;
                    //    txtSmtpNr.Text = configData?.SMTP?.Port;

                    RefreshEmailList();


                   
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fehler beim Laden der Konfiguration: {ex.Message}", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

    }
}

