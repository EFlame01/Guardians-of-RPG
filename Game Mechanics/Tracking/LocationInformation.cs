using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
    
    /// <summary>
    /// LocationInformation is a class that
    /// shows the details of the locations in the
    /// map in the <c>Map</c> class.
    /// </summary>
    public class LocationInformation
    {
        public Vector2 Coordinates {get; private set;}
        public string Name {get; private set;}
        public string Territory {get; private set;}
        public string Continent {get; private set;}
        public string Description {get; private set;}

        public LocationInformation(string name, float x, float y, string territory, string continent, string description)
        {
            Name = name;
            Coordinates = new Vector2(x, y);
            Territory = territory;
            Continent = continent;
            Description = description;
        }
    }