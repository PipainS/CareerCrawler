//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Newtonsoft.Json;

//namespace HHParser.Domain.Models.Vacancies
//{
//    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
//    public class Address
//    {
//        public string city { get; set; }
//        public string street { get; set; }
//        public string building { get; set; }
//        public double lat { get; set; }
//        public double lng { get; set; }
//        public object description { get; set; }
//        public string raw { get; set; }
//        public object metro { get; set; }
//        public List<object> metro_stations { get; set; }
//    }

//    public class Area
//    {
//        public string id { get; set; }
//        public string name { get; set; }
//        public string url { get; set; }
//    }

//    public class BillingType
//    {
//        public string id { get; set; }
//        public string name { get; set; }
//    }

//    public class Employer
//    {
//        public string id { get; set; }
//        public string name { get; set; }
//        public string url { get; set; }
//        public string alternate_url { get; set; }
//        public LogoUrls logo_urls { get; set; }
//        public string vacancies_url { get; set; }
//        public bool accredited_it_employer { get; set; }
//        public bool trusted { get; set; }
//    }

//    public class Employment
//    {
//        public string id { get; set; }
//        public string name { get; set; }
//    }

//    public class KeySkill
//    {
//        public string name { get; set; }
//    }
        
//    public class Root
//    {
//        public string id { get; set; }
//        public bool premium { get; set; }
//        public BillingType billing_type { get; set; }
//        public List<object> relations { get; set; }
//        public string name { get; set; }
//        public object insider_interview { get; set; }
//        public bool response_letter_required { get; set; }
//        public Area area { get; set; }
//        public object salary { get; set; }
//        public Type type { get; set; }
//        public Address address { get; set; }
//        public bool allow_messages { get; set; }
//        public Experience experience { get; set; }
//        public Schedule schedule { get; set; }
//        public Employment employment { get; set; }
//        public object department { get; set; }
//        public object contacts { get; set; }
//        public string description { get; set; }
//        public object branded_description { get; set; }
//        public object vacancy_constructor_template { get; set; }
//        public List<KeySkill> key_skills { get; set; }
//        public bool accept_handicapped { get; set; }
//        public bool accept_kids { get; set; }
//        public bool archived { get; set; }
//        public object response_url { get; set; }
//        public List<object> specializations { get; set; }
//        public List<ProfessionalRole> professional_roles { get; set; }
//        public object code { get; set; }
//        public bool hidden { get; set; }
//        public bool quick_responses_allowed { get; set; }
//        public List<object> driver_license_types { get; set; }
//        public bool accept_incomplete_resumes { get; set; }
//        public Employer employer { get; set; }
//        public DateTime published_at { get; set; }
//        public DateTime created_at { get; set; }
//        public DateTime initial_created_at { get; set; }
//        public object negotiations_url { get; set; }
//        public object suitable_resumes_url { get; set; }
//        public string apply_alternate_url { get; set; }
//        public bool has_test { get; set; }
//        public object test { get; set; }
//        public string alternate_url { get; set; }
//        public List<object> working_days { get; set; }
//        public List<object> working_time_intervals { get; set; }
//        public List<object> working_time_modes { get; set; }
//        public bool accept_temporary { get; set; }
//        public List<object> languages { get; set; }
//        public bool approved { get; set; }
//        public EmploymentForm employment_form { get; set; }
//        public List<object> fly_in_fly_out_duration { get; set; }
//        public bool internship { get; set; }
//        public bool night_shifts { get; set; }
//        public List<WorkFormat> work_format { get; set; }
//        public List<WorkScheduleByDay> work_schedule_by_days { get; set; }
//        public List<WorkingHour> working_hours { get; set; }
//        public object show_logo_in_search { get; set; }
//    }

//    public class Schedule
//    {
//        public string id { get; set; }
//        public string name { get; set; }
//    }

//    public class Type
//    {
//        public string id { get; set; }
//        public string name { get; set; }
//    }

//    public class WorkFormat
//    {
//        public string id { get; set; }
//        public string name { get; set; }
//    }

//    public class WorkingHour
//    {
//        public string id { get; set; }
//        public string name { get; set; }
//    }

//    public class WorkScheduleByDay
//    {
//        public string id { get; set; }
//        public string name { get; set; }
//    }


//}
