using Vincente.WebApp.Models;

namespace Vincente.WebApp.App_Start
{
    public class NavigationConfig
    {
        public static NavTree GetNavigation()
        {
            var home =

                new NavTree("Home", "Index", "Default")
                {
                    new NavTree("Invoice", "List", "Invoice"),
                    new NavTree("WIP", "ByList", "Wip"),
                    new NavTree("Users", "Summary", "User"),
                    new NavTree("Top 10s", "ByBillable", "TopX"),

                    new NavTree("Data", "Index", "Data")
                    {
                        new NavTree("Budget Control", "BudgetControl", "Data", "1"),
                        new NavTree("SDN", "SDN", "Data", "1"),
                        new NavTree("Cards", "Cards", "Data", "2"),
                        new NavTree("Tasks", "Tasks", "Data", "2"),
                        new NavTree("Time Entries", "TimeEntries", "Data", "2"),
                        new NavTree("Tasks From Trello", "TasksFromTrello", "Data", "3"),
                        new NavTree("Tasks From Toggl", "TasksFromToggl", "Data", "3")
                    },

                    new NavTree("Errors", "Summary", "Error")
                };

            return home;
        }
    }
}