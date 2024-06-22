using System;
using System.Collections.Generic;

namespace GameAPI.Entities;

public partial class User
{
    public int Userid { get; set; }

    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;

    public virtual ICollection<LeaderBoard> LeaderBoards { get; set; } = new List<LeaderBoard>();
}
