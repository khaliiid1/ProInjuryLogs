using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace ProInjuryLogs.Model;

public class Athletes
{
    public int AtheleteID { get; set; }
    public String InjuryName { get; set; }
    public Athletes() { }


    public Athletes(int AthleteID, String AthleteName) //
    {
        AthleteID = AthleteID;
        AthleteName = AthleteName;
    }
}
