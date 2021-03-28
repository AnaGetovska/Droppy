using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Green_Assistant.Models;
using InfluxDB.Client;
using InfluxDB.Client.Api.Domain;
using InfluxDB.Client.Writes;
using Microsoft.AspNetCore.Mvc;

namespace Green_Assistant.Controllers
{

    [Route("api/")]
    public class SensorController : Controller
    {
        private readonly InfluxDBClient _infux;

        public SensorController(InfluxDBClient infux)
        {
            _infux = infux;
        }

        // POST: api/update
        [HttpPost("update")]
        public SensorDataModel UpdateSensor(SensorDataModel sensorData)
        {
            using (var writeApi = _infux.GetWriteApi())
            {
                var point = PointData.Measurement("moisture")
                    .Tag("device", sensorData.Device)
                    .Field("value", sensorData.Reading)
                    .Timestamp(DateTime.UtcNow, WritePrecision.Ns);

                writeApi.WritePoint("Sensors", "Droppy", point);
            }

            return sensorData;
        }

        // GET api/value
        [HttpGet("value/{guid}")]
        public async Task<SensorDataModel> Get(string guid)
        {
            var flux = "from(bucket: \"Sensors\")" +
                "|> range(start: -1d)" +
                "|> filter(fn: (r) => r[\"_measurement\"] == \"moisture\")" +
                "|> filter(fn: (r) => r[\"_field\"] == \"value\")" +
                $"|> filter(fn: (r) => r[\"device\"] == \"{guid}\")" +
                "|> last()";

            var fluxTables = await _infux.GetQueryApi().QueryAsync(flux, "Droppy");
            double lastValue = (double)fluxTables.FirstOrDefault()?.Records.FirstOrDefault()?.GetValue();

            return new SensorDataModel() {
                Device = guid,
                Reading = (float)lastValue
            };
        }
    }
}
