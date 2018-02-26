using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CalendarPrototype.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        //gets events from the database as a JSONResult
        public JsonResult GetEvents()
        {
            using (EventDBEntities1 dc = new EventDBEntities1())
            {
                var events = dc.Events.ToList();
                return new JsonResult { Data = events, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }

        }
        [HttpPost]
        public JsonResult SaveEvent(Event evnt) {
            var status = false;
            using (EventDBEntities1 dc = new EventDBEntities1())
            {
                //this runs if you are updating an event instead of adding a new event
                if (evnt.EventId > 0)
                {
                    //Update the event
                    var temp = dc.Events.Where(a => a.EventId == evnt.EventId).FirstOrDefault();
                    if (temp != null)
                    {
                        temp.endTime = evnt.endTime;
                        temp.IsFullDay = evnt.IsFullDay;
                        temp.Organization = evnt.Organization;
                        temp.Organization_Division = evnt.Organization_Division;
                        temp.Requestor = evnt.Requestor;
                        temp.requestorDepartment = evnt.requestorDepartment;
                        temp.requestorPhone = evnt.requestorPhone;
                        temp.start = evnt.start;
                        temp.ThemeColor = evnt.ThemeColor;
                    }
                }
                else
                {
                    dc.Events.Add(evnt);
                }
                dc.SaveChanges();
                status = true;
            }
            return new JsonResult
            {
                Data = new { status = status }
            };
        }

        [HttpPost]
        public JsonResult DeleteEvent(int eventID)
        {
            var status = false;
            using (EventDBEntities1 dc = new EventDBEntities1())
            {
                var temp = dc.Events.Where(a => a.EventId == eventID).FirstOrDefault();
                if(temp != null)
                {
                    dc.Events.Remove(temp);
                    dc.SaveChanges();
                    status = true;
                }
            }
            return new JsonResult
            {
                Data = new { status = status }
            };
        }

    }
}