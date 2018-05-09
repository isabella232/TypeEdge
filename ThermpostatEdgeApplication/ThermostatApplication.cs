﻿using Microsoft.Azure.IoT.TypeEdge;
using Microsoft.Azure.IoT.TypeEdge.Hubs;
using Microsoft.Azure.IoT.TypeEdge.Modules;
using Microsoft.Extensions.Configuration;
using ThermpostatEdgeApplication.Modules;

namespace ThermpostatEdgeApplication
{
    public class ThermostatApplication : TypeEdgeApplication
    {
        public ThermostatApplication(IConfigurationRoot configuration)
            : base(configuration)
        {
        }

        public override CompositionResult Compose()
        {
            //setup modules
            var temperatureModule = new TemperatureModule();
            var normalizeTemperatureModule = new NormalizeTemperatureModule();

            Modules.Add(temperatureModule);
            Modules.Add(normalizeTemperatureModule);

            //setup the modules pub/sub
            temperatureModule.DefaultInput.Subscribe(Hub.Downstream, async (msg) => { return MessageResult.OK; });

            normalizeTemperatureModule.Temperature.Subscribe(temperatureModule.Temperature, async (temp) =>
            {
                if (temp.Scale == TemperatureScale.Celsius)
                    temp.Temperature = temp.Temperature * 9 / 5 + 32;

                await normalizeTemperatureModule.NormalizedTemperature.PublishAsync(temp);

                return MessageResult.OK;
            });

            Hub.Upstream.Subscribe(normalizeTemperatureModule.NormalizedTemperature);

            //setup module startup depedencies
            normalizeTemperatureModule.DependsOn(temperatureModule);

            return CompositionResult.OK;
        }
    }
}
