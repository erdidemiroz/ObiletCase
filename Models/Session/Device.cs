﻿namespace ObiletCase.Models.Session
{
    public class Device
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; } = "Chrome";
    }
}
