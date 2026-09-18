using System;
using System.Collections.Generic;
using AshkanAQMS.Models;

namespace AshkanAQMS.Interfaces
{
    public interface IDbService
    {
        void SaveLog(SensorLog log);
        List<SensorLog> GetAllLogs();
        List<SensorLog> GetLogs(DateTime from, DateTime to);
        void ClearAllLogs();
        string GetStoragePath();
    }
}
