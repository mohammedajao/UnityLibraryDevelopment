using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TimeWizard.Persistence
{
    public struct SaveContainer
    {
        public string Name { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}