using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
public class Sports
{
    public int AtheleteID { get; set; }
    public String InjuryName { get; set; }
    public String LeagueName { get; set; }
 public int TeamsCount { get; set; }
    public int AthletesCounnt { get; set; }
    public int managerCount { get; set; }

    public Sports() { }


    public Sports (int SportID, String SportName)
    {
        SportID = SportID;
        SportName = SportName;
        LeagueName = LeagueName;
        TeamsCount = TeamsCount;
        AthletesCounnt = AthletesCounnt;    
        managerCount = managerCount;

    }
}
