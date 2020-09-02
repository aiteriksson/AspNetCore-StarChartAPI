﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace StarChart.Models
{
    public class CelestialObject
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        
        public int? OrbitedObjectId { get; set; }

        public List<CelestialObject> Satellites { get; set; }

        public TimeSpan OrbitalPeriod { get; set; }
    }
}