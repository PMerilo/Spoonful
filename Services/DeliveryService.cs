using Microsoft.EntityFrameworkCore;
using Spoonful.Models;
using System.Text.RegularExpressions;

namespace Spoonful.Services
{
    public class DeliveryService
    {
        private readonly AuthDbContext _context;

        private string[] DistrictNames = {"North", "South", "West" , "East", "Central"};

        Dictionary<string, List<string>> Districts = new Dictionary<string, List<string>>
        {
            {"North", new List<string> { "72", "73", "77", "78", "75", "76", "79", "80" }},
            {"South", new List<string> {"01", "02", "03", "04", "05", "06", "07", "08", "14", "15", "16", "09", "10", "11", "12", "13", "17", "18", "19", "20", "21", "22", "23", "24", "25", "26", "27"}},
            {"West", new List<string> {"60", "61", "62", "63", "64", "65", "66", "67", "68", "69", "70", "71"}},
            {"East", new List<string> {"46", "47", "48", "49", "50", "81", "51", "52", "53", "54", "55", "82"}},
            {"Central", new List<string> {"28", "29", "30", "31", "32", "33", "34", "35", "36", "37", "38", "39", "40", "41", "42", "43", "44", "45", "56", "57", "58", "59" }}
        };

        public DeliveryService(AuthDbContext context)
        {
            _context = context;
        }

        public Stops? GetStopById(int id)
        {
            Stops? stop = _context.Stops.FirstOrDefault(x => x.Id.Equals(id));
            return stop;
        }

        public Delivery? GetDeliveryById(int id)
        {
            Delivery? delivery = _context.Delivery.FirstOrDefault(x => x.Id.Equals(id));
            return delivery;
        }

        public Routes? GetRouteById(int id)
        {
            Routes? route = _context.Route.FirstOrDefault(x => x.Id.Equals(id));
            return route;
        }

        public Routes? GetRouteByRegion(string region)
        {
            Routes? route = _context.Route.FirstOrDefault(x => x.Region.Equals(region));
            return route;
        }

        public OrderDetails? GetOrderDetailsbyPostalCode(string code)
        {
            OrderDetails? details = _context.OrderDetails.FirstOrDefault(x => x.Address.Contains(code));
            return details;
        }

        public List<string>? GetOrderDetailsfortheDay()
        {
            //var today = DateTime.Now.ToString("yyyy-MM-dd");
            var today = "2023-02-13";
            var details = _context.OrderDetails.Where(x => x.OrderDate == today)
                .OrderByDescending(x => x.Address)
                .ToList();
            List<string> list = new();
            foreach (OrderDetails item in details)
            {
                string pattern = @"\b\d{6}\b";
                Match match = Regex.Match(item.Address, pattern);
                if (match.Success)
                {
                    list.Add(match.Value);
                }
            }
            return list;
        }

        public List<Stops> GetAllStops()
        {
            return _context.Stops.OrderBy(v => v.Id).ToList();
        }

        public List<Delivery> GetAllDeliveries()
        {
            return _context.Delivery.OrderBy(v => v.Id).ToList();
        }

        public List<Delivery> GetAllDeliveriesWithIncludes()
        {
            var deliveries = _context.Delivery.Include(u => u.OrderDetailsId).Include(u => u.stopsId).ToList();
            return deliveries;
        }

        public List<Routes> GetAllRoutes()
        {
            return _context.Route.OrderBy(v => v.Id).ToList();
        }

        public void AddRoute(Routes route)
        {
            _context.Route.Add(route);
            _context.SaveChanges();
        }
        public void UpdateRoute(Routes route)
        {
            _context.Route.Update(route);
            _context.SaveChanges();
        }
        public void DeleteRoute(Routes route)
        {
            _context.Route.Remove(route);
            _context.SaveChanges();
        }


        public void AddDelivery(Delivery delivery)
        {
            _context.Delivery.Add(delivery);
            _context.SaveChanges();
        }
        public void UpdateDelivery(Delivery delivery)
        {
            _context.Delivery.Update(delivery);
            _context.SaveChanges();
        }
        public void DeleteDelivery(Delivery delivery)
        {
            _context.Delivery.Remove(delivery);
            _context.SaveChanges();
        }



        public void AddStop(Stops stop)
        {
            _context.Stops.Add(stop);
            _context.SaveChanges();
        }
        public void UpdateStop(Stops stop)
        {
            _context.Stops.Update(stop);
            _context.SaveChanges();
        }
        public void DeleteStop(Stops stop)
        {
            _context.Stops.Remove(stop);
            _context.SaveChanges();
        }

        public void CreateRoutes(List<string> postalCodes)
        {
            List<Routes> routeList = new();
            routeList = GetAllRoutes();
            if (routeList.Count == 0)
            {
                Routes North = new();
                North.Region = "North";
                North.Town = "North";
                Routes South = new();
                South.Region = "South";
                South.Town = "South";
                Routes East = new();
                East.Region = "East";
                East.Town = "East";
                Routes West = new();
                West.Region = "West";
                West.Town = "West";
                Routes Central = new();
                Central.Region = "Central";
                Central.Town = "Central";
                AddRoute(North);
                AddRoute(South);
                AddRoute(East);
                AddRoute(West);
                AddRoute(Central);
                Console.WriteLine("Routes got initialized since there were nun");
            }
            foreach (string postalCode in postalCodes)
            {
                foreach (string Dname in DistrictNames)
                {
                    foreach (string dis in Districts[Dname])
                    {
                        if (postalCode.Substring(0, 2) == dis)
                        {
                            Stops newStop = new();
                            newStop.Address = postalCode;
                            Routes? routeId = GetRouteByRegion(Dname);
                            newStop.RoutesId = routeId.Id;
                            AddStop(newStop);


                            Delivery newDelivery = new();
                            OrderDetails deets = GetOrderDetailsbyPostalCode(postalCode);
                            newDelivery.stopsId = newStop.Id;
                            newDelivery.OrderDetailsId = deets.Id;
                            newDelivery.status = "Scheduled";
                            AddDelivery(newDelivery);

                            newStop = null;
                            newDelivery = null;
                            //Console.WriteLine(postalCode + " " + dis);
                        }
                    }
                }
            }
        }

        public Dictionary<string, List<string>> GroupDeliveries(List<string> postalCodes)
        {
            List<Routes> routeList = new();
            routeList = GetAllRoutes();
            if (routeList.Count == 0)
            {
                Routes North = new();
                North.Region = "North";
                North.Town = "North";
                Routes South = new();
                South.Region = "South";
                South.Town = "South";
                Routes East = new();
                East.Region = "East";
                East.Town = "East";
                Routes West = new();
                West.Region = "West";
                West.Town = "West";
                Routes Central = new();
                Central.Region = "Central";
                Central.Town = "Central";
                AddRoute(North);
                AddRoute(South);
                AddRoute(East);
                AddRoute(West);
                AddRoute(Central);
                Console.WriteLine("Routes got initialized since there were nun");
            }
            Dictionary<string, List<string>> SortedRoutes = new();
            foreach (string postalCode in postalCodes)
            {
                foreach (string Dname in DistrictNames)
                {
                    foreach (string dis in Districts[Dname])
                    {
                        if (!SortedRoutes.ContainsKey(Dname))
                        {
                            SortedRoutes[Dname] = new List<string>();
                        }
                        if (postalCode.Substring(0, 2) == dis)
                        {
                            SortedRoutes[Dname].Add(postalCode);
                        }
                    }
                }
            }

            return SortedRoutes;
        }
    }
}
