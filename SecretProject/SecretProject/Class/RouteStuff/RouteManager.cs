using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SecretProject.Class.NPCStuff;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XMLData.RouteStuff;

namespace SecretProject.Class.RouteStuff
{
    public class RouteManager : Component
    {

        public RouteSchedule DobbinRouteSchedule { get; private set; }
        public RouteSchedule ElixirRouteSchedule { get; private set; }
        public RouteSchedule KayaRouteSchedule { get; private set; }
        public RouteSchedule JulianRouteSchedule { get; private set; }
        public RouteSchedule SarahRouteSchedule { get; private set; }
        public RouteSchedule MippinRouteSchedule { get; private set; }
        public RouteSchedule NedRouteSchedule { get; private set; }
        public RouteSchedule TealRouteSchedule { get; private set; }
        public RouteSchedule MarcusRouteSchedule { get; private set; }
        public RouteSchedule CasparRouteSchedule { get; private set; }
        public static List<RouteSchedule> AllSchedules { get; private set; }

        public RouteManager(GraphicsDevice graphics, ContentManager content) : base(graphics, content)
        {
        }

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

        public override void Load()
        {
            DobbinRouteSchedule = content.Load<RouteSchedule>("Route/DobbinRouteSchedule");
            ElixirRouteSchedule = content.Load<RouteSchedule>("Route/ElixerRouteSchedule");
            KayaRouteSchedule = content.Load<RouteSchedule>("Route/KayaRouteSchedule");
            JulianRouteSchedule = content.Load<RouteSchedule>("Route/JulianRouteSchedule");
            SarahRouteSchedule = content.Load<RouteSchedule>("Route/SarahRouteSchedule");
            MippinRouteSchedule = content.Load<RouteSchedule>("Route/MippinRouteSchedule");
            NedRouteSchedule = content.Load<RouteSchedule>("Route/NedRouteSchedule");
            TealRouteSchedule = content.Load<RouteSchedule>("Route/TealRouteSchedule");
            MarcusRouteSchedule = content.Load<RouteSchedule>("Route/MarcusRouteSchedule");
            CasparRouteSchedule = content.Load<RouteSchedule>("Route/CasparRouteSchedule");
            AllSchedules = new List<RouteSchedule>() { DobbinRouteSchedule, ElixirRouteSchedule, KayaRouteSchedule,
                JulianRouteSchedule, SarahRouteSchedule, MippinRouteSchedule, NedRouteSchedule, TealRouteSchedule, MarcusRouteSchedule, CasparRouteSchedule };
            for (int i = 0; i < AllSchedules.Count; i++)
            {
                foreach (Route route in AllSchedules[i].Routes)
                {
                    route.ProcessStageToEndAt();
                }
            }
        }

        public override void Unload()
        {
            throw new NotImplementedException();
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

