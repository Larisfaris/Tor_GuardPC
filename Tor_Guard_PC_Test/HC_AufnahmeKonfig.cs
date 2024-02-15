using System;
using System.IO;
using System.Text.Json;


namespace TorGuard
{
   
    public class HC_AufnahmeKonfig
    {
        public double StundenProDatei { get; set; }
        public double BackupDays { get; set; }
        public double AktualisierungServerDatei { get; set; }

        // Singleton-Instanz
       // private static HC_AufnahmeKonfig instance;

        // Privater Konstruktor, um direkte Instanziierung zu verhindernA
        private HC_AufnahmeKonfig() 
        {

        }

        // Öffentliche Methode zum Zugriff auf die Singleton-Instanz
        //public static HC_AufnahmeKonfig Instance
        //{
        //    get
        //    {   
                  
                
        //        if (instance == null)
        //        {
                
                        
        //                RaspberryZugriff raspZugriff = new RaspberryZugriff();

        //                // Korrekter Aufruf mit Pfad und Dateiname
        //                instance = raspZugriff.LadeDateiVomServer<HC_AufnahmeKonfig>(HC_Speicherpfade.Instance.Speicherpfad_Konfig, HC_Speicherpfade.Instance.Speichername_Aufnahemekonfig) ?? new HC_AufnahmeKonfig();
                    
        //        }
        //        return instance;
        //    }
        //}
    }
}
