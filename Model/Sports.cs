using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
public class Sports
{
    public int AtheleteID { get; set; }
    public String InjuryName { get; set; }
    public Sports() { }


    public Sports (int SportID, String SportName)
    {
        SportID = SportID;
        SportName = SportName;
    }
}
