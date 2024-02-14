using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TorGuard
{
    public class HC_Speicherpfade
    {
        public string Speicherpfad_tor { get; set; }
        public string Speicherpfad_Konfig { get; set; }
        public string Speichername_Torkonfig { get; set; }
        public string Speichername_Netzwerkkonfig { get; set; }
        public string Speichername_Emailkonfig { get; set; }
        public string Speichername_Aufnahemekonfig { get; set; }

        public HC_Speicherpfade()
        {

        }

        //Konstruktor von HC_Speicherpfade aktualisieren
        //public HC_Speicherpfade(string speicherpfad_tor, string speicherpfad_Konfig, string speichername_Torkonfig, string speichername_Netzwerkkonfig, string speichername_Emailkonfig, string speichername_Aufnahemekonfig)
        //{
        //    Speicherpfad_tor = speicherpfad_tor;
        //    Speicherpfad_Konfig = speicherpfad_Konfig;
        //    Speichername_Torkonfig = speichername_Torkonfig;
        //    Speichername_Netzwerkkonfig = speichername_Netzwerkkonfig;
        //    Speichername_Emailkonfig = speichername_Emailkonfig;
        //    Speichername_Aufnahemekonfig = speichername_Aufnahemekonfig;
        


        //      Speicherpfad_tor = @"\\192.168.1.4\TorGuard_Konfig\Documents\tore\";
        //Speicherpfad_Konfig = @"\\192.168.1.4\TorGuard_Konfig\Documents\konfig\";
        //Speichername_Torkonfig = "Tor_config.json";
        //Speichername_Netzwerkkonfig = "Netzwerk_config.json";
        //Speichername_Emailkonfig = "Empfängerlist_config.json";
        //Speichername_Aufnahemekonfig = "AufnahmeKonfig.json";

        //}
    }
}   
