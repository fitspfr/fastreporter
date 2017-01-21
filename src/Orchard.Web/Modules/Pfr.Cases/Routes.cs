using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Routing;
using Orchard.Mvc.Routes;

namespace Pfr.Cases {
    public class Routes : IRouteProvider {
        public void GetRoutes(ICollection<RouteDescriptor> routes) {
            foreach (var routeDescriptor in GetRoutes())
                routes.Add(routeDescriptor);
        }

        public IEnumerable<RouteDescriptor> GetRoutes() {
            return new[] {
                             new RouteDescriptor {   Priority = 5,
                                                     Route = new Route(
                                                         "pfr/email/{id}",
                                                         new RouteValueDictionary {
                                                                                      {"area", "Pfr.Cases"},
                                                                                      {"controller", "Cases"},
                                                                                      {"action", "SendMail"},

                                                         },
                                                         new RouteValueDictionary (),
                                                         new RouteValueDictionary {
                                                                                      {"area", "Pfr.Cases"}
                                                                                  },
                                                         new MvcRouteHandler())
                             },
                             new RouteDescriptor {   Priority = 6,
                                                     Route = new Route(
                                                         "pfr/{id}",
                                                         new RouteValueDictionary {
                                                                                      {"area", "Pfr.Cases"},
                                                                                      {"controller", "Cases"},
                                                                                      {"action", "Index"},
                                                         },
                                                         new RouteValueDictionary (),
                                                         new RouteValueDictionary {
                                                                                      {"area", "Pfr.Cases"}
                                                                                  },
                                                         new MvcRouteHandler())

                             }
                         };
        }
    }
}