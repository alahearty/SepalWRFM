using System;
using System.Collections.Generic;
using SepalWRFM.Core;
using SepalWRFM.Core.Models;
using SepalWRFM.Services.Interfaces;

namespace SepalWRFM.Services
{
    /// <summary>
    /// Service for managing WRFM module data.
    /// </summary>
    public class WRFMModuleService : IWRFMModuleService
    {
        public IEnumerable<WRFMModuleData> GetWRFMModules()
        {
            // In a real application, this data would come from:
            // - Database
            // - Configuration file
            // - Web API
            // For now, we'll keep it here but in a service for easy migration

            return new List<WRFMModuleData>
            {
                new WRFMModuleData
                {
                    Name = "Production Analysis",
                    Description = "Analyze production trends, decline curves, and performance metrics",
                    Category = AppConstants.ModuleCategories.Production,
                    IconKind = "ChartLine",
                    GradientStartColor = "#1E3C72",
                    GradientEndColor = "#2A5298",
                    IconBackgroundColor = "#1E3C72"
                },
                new WRFMModuleData
                {
                    Name = "Production Geology",
                    Description = "Geological mapping, reservoir characterization, and stratigraphy",
                    Category = AppConstants.ModuleCategories.Geology,
                    IconKind = "Map",
                    GradientStartColor = "#8B4513",
                    GradientEndColor = "#CD853F",
                    IconBackgroundColor = "#8B4513"
                },
                new WRFMModuleData
                {
                    Name = "Petrophysics",
                    Description = "Rock properties, log analysis, and reservoir evaluation",
                    Category = AppConstants.ModuleCategories.Analysis,
                    IconKind = "ChartBar",
                    GradientStartColor = "#D32F2F",
                    GradientEndColor = "#F44336",
                    IconBackgroundColor = "#D32F2F"
                },
                new WRFMModuleData
                {
                    Name = "Schematic",
                    Description = "Well schematics, facility diagrams, and network visualization",
                    Category = AppConstants.ModuleCategories.Visualization,
                    IconKind = "Network",
                    GradientStartColor = "#00796B",
                    GradientEndColor = "#009688",
                    IconBackgroundColor = "#00796B"
                },
                new WRFMModuleData
                {
                    Name = "Well Integrity",
                    Description = "Monitor well integrity, casing condition, and safety compliance",
                    Category = AppConstants.ModuleCategories.Safety,
                    IconKind = "ShieldCheck",
                    GradientStartColor = "#F57C00",
                    GradientEndColor = "#FF9800",
                    IconBackgroundColor = "#F57C00"
                },
                new WRFMModuleData
                {
                    Name = "Reservoir Management",
                    Description = "Reservoir modeling, simulation, and optimization strategies",
                    Category = AppConstants.ModuleCategories.Reservoir,
                    IconKind = "Database",
                    GradientStartColor = "#512DA8",
                    GradientEndColor = "#673AB7",
                    IconBackgroundColor = "#512DA8"
                },
                new WRFMModuleData
                {
                    Name = "Well Testing",
                    Description = "Pressure transient analysis, flow testing, and diagnostics",
                    Category = AppConstants.ModuleCategories.Testing,
                    IconKind = "Speedometer",
                    GradientStartColor = "#0288D1",
                    GradientEndColor = "#03A9F4",
                    IconBackgroundColor = "#0288D1"
                },
                new WRFMModuleData
                {
                    Name = "Facility Management",
                    Description = "Surface facility operations, equipment tracking, and maintenance",
                    Category = AppConstants.ModuleCategories.Facilities,
                    IconKind = "Factory",
                    GradientStartColor = "#388E3C",
                    GradientEndColor = "#4CAF50",
                    IconBackgroundColor = "#388E3C"
                },
                new WRFMModuleData
                {
                    Name = "Economic Analysis",
                    Description = "Production economics, forecasting, and financial planning",
                    Category = AppConstants.ModuleCategories.Economics,
                    IconKind = "CurrencyUsd",
                    GradientStartColor = "#C2185B",
                    GradientEndColor = "#E91E63",
                    IconBackgroundColor = "#C2185B"
                },
                new WRFMModuleData
                {
                    Name = "Data Analytics",
                    Description = "Advanced analytics, machine learning, and predictive modeling",
                    Category = AppConstants.ModuleCategories.Analytics,
                    IconKind = "Brain",
                    GradientStartColor = "#5D4037",
                    GradientEndColor = "#795548",
                    IconBackgroundColor = "#5D4037"
                }
            };
        }
    }
}