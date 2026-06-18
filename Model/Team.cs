using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Team
{
    public int AtheleteID { get; set; }
    public String InjuryName { get; set; }
    public Team() { }


    public Team(int TeamID, String TeamName)
    {
        TeamID = TeamID;
        TeamName = TeamName;
    }
}
