using System;
using System.Collections.Generic;

namespace GameAPI.Entities;

public partial class LeaderBoard
{
    public int Id { get; set; }

    public int Userid { get; set; }

    public double Time { get; set; }

    public virtual User User { get; set; } = null!;
}
