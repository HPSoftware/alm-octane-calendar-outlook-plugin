using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharedCalendar
{
  public class MilestoneDataContainer
  {
    private static String[] PARAMS_LIST = new string[] { "Color", "Reminder" };
    
    public String Category;
    public int ReminderMinutesBeforeStart;
    public Boolean ReminderSet;

    public Boolean AddPhrase(String phrase)
    {
      string[] parts = phrase.Split(':');
      if (parts.Length == 2) {
        var param = parts[0].Trim();
        var value = parts[1].Trim();
        
        for (int i = 0; i < PARAMS_LIST.Length ; i++) {
          if (String.Compare(param, PARAMS_LIST[i], StringComparison.OrdinalIgnoreCase) == 0) {
            if (i == 0) {
              Category = value;
              break;
            } 
            else if(i==1) {
              ReminderMinutesBeforeStart = getMinutes(value);
              ReminderSet = true;
              break;
            }
            break;
          }
        }

      }

      return true;
    }

    private int getMinutes(string value)
    {
      string[] parts = value.Split(new string[] { "&nbsp;" }, StringSplitOptions.RemoveEmptyEntries);
      if (parts.Length == 1)
      {
        parts = value.Split(' ');
      }
 	    int time;
      if (Int32.TryParse(parts[0], out time))
      {
        if (String.Compare(parts[1], "Day", StringComparison.OrdinalIgnoreCase) == 0 || String.Compare(parts[1], "Days", StringComparison.OrdinalIgnoreCase) == 0) {
          time = time * (24*60);
        }

        else if (String.Compare(parts[1], "hour", StringComparison.OrdinalIgnoreCase) == 0 || String.Compare(parts[1], "hours", StringComparison.OrdinalIgnoreCase) == 0) {
          time = time * 60;
        }
      }
      return time;
    }

  }
}
