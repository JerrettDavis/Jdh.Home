using System;

namespace Jdh.Home.Mobile.Models
{
    public class Device
    {
        public Device(
            string name, 
            string model, 
            Uri imageUrl, 
            string ssidPrefix,
            string defaultPassword)
        {
            Name = name;
            Model = model;
            ImageUrl = imageUrl;
            SsidPrefix = ssidPrefix;
            DefaultPassword = defaultPassword;
        }

        public Device() { }

        public string Name { get; set; }
        public string Model { get; set; }
        public Uri ImageUrl { get; set; }
        public string SsidPrefix { get; set; }
        public string DefaultPassword { get; set; }

        public override string ToString()
        {
            return $"{Name} - {Model}";
        }
    }
}
