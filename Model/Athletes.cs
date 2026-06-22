using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace ProInjuryLogs.Model;

public class Athletes
{
    public int AtheleteID { get; set; }
    public String InjuryName { get; set; }
    public String LastName { get; set; }
    public String FirstName { get; set; }
    public int SportID { get; set; }
    public String TeamName { get; set; }
    public String Phone { get; set; }

    public Athletes() { }


    public Athletes(int AthleteID, String AthleteName) 
    {
        AthleteID = AthleteID;
        AthleteName = AthleteName;
        LastName = LastName;
        FirstName = FirstName;
        SportID = SportID;
        TeamName = TeamName;
        Phone = Phone;

    }
}
