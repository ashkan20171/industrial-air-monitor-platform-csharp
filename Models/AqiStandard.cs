using System;
using System.Collections.Generic;
using System.Linq;

namespace AshkanAQMS.Models
{
    public static class AqiStandard
    {
        public sealed class Breakpoint
        {
            public double Clow { get; set; }
            public double Chigh { get; set; }
            public int Ilow { get; set; }
            public int Ihigh { get; set; }

            public Breakpoint(double clow, double chigh, int ilow, int ihigh)
            {
                Clow = clow;
                Chigh = chigh;
                Ilow = ilow;
                Ihigh = ihigh;
            }
        }

        // PM2.5 breakpoints (US EPA)
        public static readonly List<Breakpoint> PM25Breakpoints = new List<Breakpoint>
        {
            new Breakpoint(0.0,   12.0,   0,   50),
            new Breakpoint(12.1,  35.4,  51,  100),
            new Breakpoint(35.5,  55.4, 101, 150),
            new Breakpoint(55.5, 150.4, 151, 200),
            new Breakpoint(150.5, 250.4, 201, 300),
            new Breakpoint(250.5, 350.4, 301, 400),
            new Breakpoint(350.5, 500.4, 401, 500)
        };

        // PM10 breakpoints (US EPA)
        public static readonly List<Breakpoint> PM10Breakpoints = new List<Breakpoint>
        {
            new Breakpoint(0,    54,   0,   50),
            new Breakpoint(55,   154,  51,  100),
            new Breakpoint(155,  254, 101, 150),
            new Breakpoint(255,  354, 151, 200),
            new Breakpoint(355,  424, 201, 300),
            new Breakpoint(425,  504, 301, 400),
            new Breakpoint(505,  604, 401, 500)
        };

        // NO2 breakpoints - simplified / editable
        // In real industrial use, these should match the exact standard you want.
        public static readonly List<Breakpoint> NO2Breakpoints = new List<Breakpoint>
        {
            new Breakpoint(0,    53,   0,   50),
            new Breakpoint(54,  100,  51,  100),
            new Breakpoint(101, 360, 101, 150),
            new Breakpoint(361, 649, 151, 200),
            new Breakpoint(650, 1249, 201, 300),
            new Breakpoint(1250,1649, 301, 400),
            new Breakpoint(1650,2049, 401, 500)
        };

        public static string GetCategory(int aqi)
        {
            if (aqi <= 50) return "Good";
            if (aqi <= 100) return "Moderate";
            if (aqi <= 150) return "Unhealthy for Sensitive Groups";
            if (aqi <= 200) return "Unhealthy";
            if (aqi <= 300) return "Very Unhealthy";
            return "Hazardous";
        }

        public static int ClampAQI(int aqi)
        {
            if (aqi < 0) return 0;
            if (aqi > 500) return 500;
            return aqi;
        }

        public static bool IsValidConcentration(double value)
        {
            return !double.IsNaN(value) && !double.IsInfinity(value) && value >= 0;
        }

        public static int CalculateFromBreakpoints(double concentration, IEnumerable<Breakpoint> breakpoints)
        {
            if (!IsValidConcentration(concentration))
                return 0;

            var bp = breakpoints.FirstOrDefault(x => concentration >= x.Clow && concentration <= x.Chigh);
            if (bp == null)
                return 500;

            double aqi = ((double)(bp.Ihigh - bp.Ilow) / (bp.Chigh - bp.Clow)) *
                         (concentration - bp.Clow) + bp.Ilow;

            return ClampAQI((int)Math.Round(aqi));
        }
    }
}
