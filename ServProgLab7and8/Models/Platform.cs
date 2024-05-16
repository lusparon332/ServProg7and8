using System;
using System.Collections.Generic;

namespace ServProgLab7and8.Models;

public partial class Platform
{
    public int Id { get; set; }

    public string? PlatformName { get; set; }

    public virtual ICollection<GamePlatform> GamePlatforms { get; set; } = new List<GamePlatform>();
}
