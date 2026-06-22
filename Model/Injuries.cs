using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProInjuryLogs.Model;
  public class Injuries
{
    public int InjuryID { get; set; }
    public int AthleteID { get; set; }
    public String InjuryType { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime RecoveryDate { get; set; }
    public String InjuryName { get; set; }
    public Injuries() { }


    public Injuries(int InjuryId, String Injuryname)
    {
        InjuryID = InjuryID;
        AthleteID = AthleteID;
        InjuryType = InjuryType;
        StartDate = StartDate;
        RecoveryDate = RecoveryDate;
        InjuryName = InjuryName;

    }
}

