using SecretProject.Class.NPCStuff;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XMLData.RouteStuff;

namespace SecretProject.Class.RouteStuff
{
    public static class RouteLibrary
    {


        /// <summary>
        /// Returns the dialogue skeleton which corresponds to the time. Skeleton will always default to the one which is greater or equal
        /// to the time given, but less than the next time slot. Will give the maximum time slot if less than the minimum
        /// </summary>
        /// <param name="character"></param>
        /// <param name="month"></param>
        /// <param name="day"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        public static Route RetrieveRoute(RouteSchedule routeSchedule, Month month, int day, string time)
        {

            int routeIndex = routeSchedule.Routes.BinarySearch(new Route { Time = time }, new RouteTimeComparer());

            if (routeIndex < 0)
            {
                routeIndex = ~routeIndex - 1;
                if (routeIndex < 0)
                {
                    routeIndex = routeSchedule.Routes.Count - 1;
                }
            }



            Route route= routeSchedule.Routes[routeIndex];

            if (route == null)
            {
                return null;
            }

            return route;
        }



    }

    public class RouteTimeComparer : IComparer<Route>
    {
        public int Compare(Route x, Route y)
        {
            return Game1.GlobalClock.GetTimeFromString(x.Time).CompareTo(Game1.GlobalClock.GetTimeFromString(y.Time));
        }
    }
}

