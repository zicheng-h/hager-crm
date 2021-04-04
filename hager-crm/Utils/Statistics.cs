using hager_crm.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace hager_crm.Utils
{
    //Util Class for the Statistics card and Birthday card in the Home page
    public class Statistics
    {
        public static double Active(double active, double all)
        {
            return active / all;
        }

        public static List<Employee> Birthdays(List<Employee> employees)
        {
            DateTime today = DateTime.Today;
            List<Employee> birthday = new List<Employee>();
            DateTime range = today.AddDays(30);
            foreach (Employee employee in employees)
            {
                if (employee.DOB != null)
                {
                    if(employee.DOB?.Month == today.Month && employee.DOB?.Day >= today.Day || employee.DOB?.Month == range.Month && employee.DOB?.Day <= range.Day)
                    {
                        birthday.Add(employee);
                    }
                }
            }
            return birthday;
        }

        public static List<Calendar> Event(List<Calendar> calendars)
        {
            DateTime today = DateTime.Today;
            List<Calendar> evtdate = new List<Calendar>();
            DateTime range = today.AddDays(60);
            foreach (Calendar calendar in calendars)
            {
                if (calendar.Date.Month == today.Month && calendar.Date.Day >= today.Day || calendar.Date.Month == range.Month && calendar.Date.Day <= range.Day)
                {
                    evtdate.Add(calendar);
                }
            }
            return evtdate;
        }

    }
}
