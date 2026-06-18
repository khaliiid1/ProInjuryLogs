using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProInjuryLogs.Model;
  public class Injuries
{
    public int InjuryID { get; set; }
    public String InjuryName { get; set; }
    public Injuries() { }


    public Injuries(int InjuryId, String Injuryname)
    {
        InjuryID = InjuryID;
        InjuryName = InjuryName;
    }
}

