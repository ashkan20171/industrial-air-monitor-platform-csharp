using System;
using System.Collections.Generic;
using System.Linq;
using AshkanAQMS.Models;

namespace AshkanAQMS.Drivers
{
    public sealed class AnalyzerMeasurementDescriptor
    {
        public string Name { get; set; }
        public string Unit { get; set; }
        public int MapperId { get; set; }
        public override string ToString() { return Name + "  [" + Unit + "]"; }
    }
    public sealed class AnalyzerDriverDescriptor
    {
        public string Id { get; set; }
        public string Manufacturer { get; set; }
        public string DisplayName { get; set; }
        public string Transport { get; set; }
        public string ValidationStatus { get; set; }
        public string Notes { get; set; }
        public List<AnalyzerMeasurementDescriptor> Measurements { get; set; } = new List<AnalyzerMeasurementDescriptor>();
        public override string ToString() { return Manufacturer + " — " + DisplayName; }
    }

    /// <summary>Analyzer catalog derived from the supplied SadraAQMS sources. Protocol validation status is explicit.</summary>
    public static class AnalyzerDriverCatalog
    {
        private static AnalyzerMeasurementDescriptor M(string n,string u,int id) { return new AnalyzerMeasurementDescriptor{Name=n,Unit=u,MapperId=id}; }
        private static readonly AnalyzerMeasurementDescriptor[] Gases = { M("O3","ppb",1),M("CO","ppm",2),M("NO","ppb",3),M("NO2","ppb",4),M("NOx","ppb",5),M("SO2","ppb",6) };
        private static readonly AnalyzerMeasurementDescriptor[] Pm = { M("PM2.5","µg/m³",7),M("PM10","µg/m³",8) };
        private static readonly AnalyzerMeasurementDescriptor[] Weather = { M("Wind Speed","m/s",101),M("Wind Direction","°",102),M("Temperature","°C",103),M("Humidity","%",104),M("Pressure","hPa",105),M("Rain","mm",106),M("Solar Radiation","W/m²",120),M("UV Index","index",121) };
        private static List<AnalyzerMeasurementDescriptor> L(params AnalyzerMeasurementDescriptor[][] sets) { return sets.SelectMany(x=>x).Select(x=>M(x.Name,x.Unit,x.MapperId)).ToList(); }
        private static AnalyzerDriverDescriptor D(string id,string m,string n,string t,string s,string notes,List<AnalyzerMeasurementDescriptor> ms) { return new AnalyzerDriverDescriptor{Id=id,Manufacturer=m,DisplayName=n,Transport=t,ValidationStatus=s,Notes=notes,Measurements=ms}; }
        private static readonly List<AnalyzerDriverDescriptor> ItemsInternal = new List<AnalyzerDriverDescriptor>
        {
            D("generic-ascii","Generic","Generic ASCII / Numeric","Serial / TCP","Production","Configurable text protocol.",L(Gases,Pm,Weather)),
            D("aeroqual","Aeroqual","Aeroqual","HTTP","Migration reference","Legacy login/JSON PM protocol.",L(Pm)),
            D("aio2-9800","AIO2","AIO2 9800","Serial","Migration reference","CSV multi-channel response.",L(Gases,Pm)),
            D("ama","AMA","AMA Binary","Serial","Migration reference","STX/ETX + checksum.",L(Gases)),
            D("api-enviro","API / Enviro","API Enviro Technology","Serial","Migration reference","Gas concentration/alarm commands.",L(Gases,Pm)),
            D("bam","Met One","BAM / E-BAM","Serial","Migration reference","Particulate current/history protocol.",L(Pm)),
            D("bc","Magee / Met One","Aethalometer AE / AE33","Serial","Migration reference","Black carbon response family.",new List<AnalyzerMeasurementDescriptor>{M("Black Carbon","ng/m³",201)}),
            D("ecotech","Ecotech","Ecotech / Serinus","Serial / USB","Migration reference","Gas analyzer family.",L(Gases)),
            D("serinus-s50","Ecotech","Serinus S50 SO2","Serial / TCP","Migration reference","DCONC multidrop command.",new List<AnalyzerMeasurementDescriptor>{M("SO2","ppb",6)}),
            D("esa","ESA","ESA Analyzer","Serial / UDP","Migration reference","Gas/PM + calibration/remote.",L(Gases,Pm)),
            D("esa-modbus","ESA","ESA Modbus","Modbus RTU","Migration reference","CRC16 binary protocol.",L(Gases,Pm)),
            D("gc995","Synspec","GC995 / GC Alpha","Serial","Migration reference","GC gas analyzer mapping.",L(Gases)),
            D("grimm","GRIMM","GRIMM Dust","Serial","Migration reference","PM response + alarms.",L(Pm)),
            D("horiba-370","HORIBA","HORIBA 3XX / 370","Serial","Migration reference","Framed multi-channel protocol.",L(Gases)),
            D("horiba-lan","HORIBA","HORIBA LAN","HTTP / CGI","Migration reference","Realtime/history recovery.",L(Gases,Pm)),
            D("horiba-logger","HORIBA","HORIBA Logger","Serial","Migration reference","Mapper based logger channels.",L(Gases,Pm)),
            D("leq-hd2110","Delta OHM","HD2110L LEQ","Serial","Migration reference","Sound level measurement.",new List<AnalyzerMeasurementDescriptor>{M("LEQ","dB",301)}),
            D("modbus-tm1240","TM","TM1240","Modbus RTU","Migration reference","Function 03 + CRC16.",L(Gases)),
            D("palas","Palas","Palas Particle Analyzer","Serial","Migration reference","Framed concentration/alarm.",L(Pm)),
            D("swam","FAI","SWAM","Serial","Migration reference","Indexed A/B particulate acquisition.",L(Pm)),
            D("teledyne","Teledyne","Teledyne Gas Analyzer","Serial","Migration reference","Framed gas analyzer protocol.",L(Gases)),
            D("thermo","Thermo Scientific","Thermo Scientific Gas Analyzer","Serial / TCP","Migration reference","Framed multi-channel protocol.",L(Gases)),
            D("tp-analog","TP","TP Analog","Serial","Migration reference","Per-channel voltage acquisition.",new List<AnalyzerMeasurementDescriptor>{M("Analog Channel","V",401)}),
            D("unitec","Unitec","Unitec Analyzer","TCP","Migration reference","Mapper-based @#iny response.",L(Gases,Pm,Weather)),
            D("vantage-weather","Davis","Vantage Weather","Serial","Migration reference","LOOP packet + CRC16.",L(Weather)),
            D("vaisala-wxt510","Vaisala","WXT510","Serial","Migration reference","Composite weather messages.",L(Weather)),
            D("weather-hd52","Delta OHM","HD52 Weather","Serial / Modbus-like","Migration reference","CRC16 weather registers.",L(Weather)),
            D("deltaohm-weather","Delta OHM","Delta OHM Weather","Serial","Migration reference","Weather parameter parser.",L(Weather)),
            D("esa-analog","ESA / Advantech","ESA Analog / Advantech","DAQ Analog","Migration reference","Analog input mapping.",new List<AnalyzerMeasurementDescriptor>{M("Analog Input","V",401)}),
            D("enviro-relcomm","Enviro","Enviro Relative Communication","Serial","Migration reference","Per-pollutant gas commands.",L(Gases,Pm))
        };
        public static IReadOnlyList<AnalyzerDriverDescriptor> Items { get { return ItemsInternal; } }
        public static AnalyzerDriverDescriptor Find(string id) { return ItemsInternal.FirstOrDefault(x=>string.Equals(x.Id,id,StringComparison.OrdinalIgnoreCase)); }
        public static void ApplyDefaults(AnalyzerConfig config,string driverId)
        {
            if(config==null)return; var d=Find(driverId); if(d==null)return; config.DriverId=d.Id; config.Manufacturer=d.Manufacturer; config.Model=d.DisplayName;
            config.ConnectionType=(d.Transport.Contains("TCP")||d.Transport.Contains("HTTP"))?"IP":"COM";
            if(driverId=="serinus-s50") { config.RequestCommand="DCONC,{DEVICE_ID_3}\\r"; config.ResponseRegex=@"(?<value>[+-]?\d+(?:\.\d+)?)"; }
            else if(driverId=="unitec") { config.RequestCommand="@#iny\\r\\n"; config.ResponseRegex=@"#\d+;(?<value>[+-]?\d+(?:\.\d+)?)"; }
        }
    }
}
