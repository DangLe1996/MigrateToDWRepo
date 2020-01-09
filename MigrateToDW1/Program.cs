using AXISAutomation.FixtureConfiguration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeCalculator
{
    class Program
    {

        static void Main(string[] args)
        {

            //Utilities.updateTime();

            //Utilities.UploadStandardTime();

            string lineDesc = "BBRLED-1000-80-35-RG-35'-W-UNV-DP-1-DF";
            lineDesc = "SKPLED-22-4000-80-35-0.5-W-UNV-DP-1-TB9";
            string DWConnectiontring = "metadata = res://*/DBConnection.csdl|res://*/DBConnection.ssdl|res://*/DBConnection.msl;provider=System.Data.SqlClient;provider connection string='Data Source=VAULT\\DRIVEWORKS;Initial Catalog=\"AXIS Automation\";Integrated Security=True;MultipleActiveResultSets=True'";

            using (AXISAutomation.Tools.DBConnection.AXIS_AutomationEntities _AutomationEntities = new AXISAutomation.Tools.DBConnection.AXIS_AutomationEntities(DWConnectiontring))
            {
                _Fixture fixture = new AXISAutomation.FixtureConfiguration._Fixture(lineDesc, _AutomationEntities);

                fixture.SPM.ConfigureAll();
                var Time = new TimeCalculator(fixture,4);

            }

            using (var db = new DWModel())
            {
                //string codeid = "BBRLED";
                //string optionName = "Standard";
                //string workStation = "Cutting Extrusion";
                //string CategoryName = "Length";
                //string parameter = "8";

                //var time = db.Timing_Option.First(r => r.Fixture.Code == codeid
                //&& r.Name == optionName && r.Timing_WorkStations.Name == workStation
                //&& r.Categories.Any( t => t.Name == CategoryName) 
                //&& r.Parameters.Any(e => e.Code == parameter));

                //int optionID = 72636;
                //var time2 = db.Timing_Option.Where(r => r.OptionID == optionID).FirstOrDefault();
                //time2.Time = (decimal)1.40;
                //db.SaveChanges();
            }


        }
    }


}


















//using (var db = new DWModel())
//{

//    string fxCode = "BBRLED";
//    //string OpCode = "CUEX0000";
//    //string ParamCode = "4";
//    // int LengthID = 8;
//    //int SizeID = 20;


//    //var timingopt = db.Timing_Option.Where(r => r.Fixture.Code == fxCode && r.Timing_WorkStations.OpCode == OpCode).ToList();



//    //var category = db.Categories.Where(r => r.id == LengthID).FirstOrDefault();
//    //var param = db.Parameters.Where(r => r.Code == ParamCode).FirstOrDefault();
//    //var workstation = db.Timing_WorkStations.Where(r => r.OpCode == OpCode).FirstOrDefault();
//    var fixture = db.Fixtures.Where(r => r.Code == fxCode).FirstOrDefault();
//    var option = fixture.Timing_Option.FirstOrDefault();
//    fixture.Timing_Option.FirstOrDefault().GetHashCode();
//    var option2 = new Timing_Option()
//    {
//        Name = option.Name,
//        Time = 4,
//        Timing_WorkStations = option.Timing_WorkStations,
//        Categories = option.Categories,
//        Parameters = option.Parameters,
//        Fixture = option.Fixture

//    };

//    var test = Utilities.ContainsTimeOption(fixture, option2);

//    //var test2 = fixture.Timing_Option.Where(r => r.Name == option2.Name
//    //&& r.Time == option2.Time
//    //&& r.Categories.Equals(option2.Categories)
//    //&& r.Parameters.Equals(option2.Parameters)
//    //&& r.Fixture == option2.Fixture
//    //&& r.Timing_WorkStations == option2.Timing_WorkStations).FirstOrDefault();

//    option.Time = 10;
//    int x = 3;
//    //var input = new Timing_Option()
//    //{
//    //    Fixture = fixture,
//    //    Name = "Standard",
//    //    Time = 1,
//    //    Timing_WorkStations = workstation

//    //};
//    //input.Categories.Add(category);
//    //input.Parameters.Add(param);

//    //db.Timing_Option.Add(input);
//    //db.SaveChanges();

//    //int x = 3;
//    //var wks = db.Timing_WorkStations.FirstOrDefault();
//    //var param = db.Parameters.FirstOrDefault();
//    //var cat = db.Categories.FirstOrDefault();
//    //HashSet<Timing_Option> test = new HashSet<Timing_Option>();

//    //Timing_Option t1 = new Timing_Option()
//    //{
//    //    Name = "test",
//    //    Time = 1,
//    //    Timing_WorkStations = wks,
//    //    Fixture = fixture


//    //};
//    //t1.Categories.Add(cat);
//    //t1.Parameters.Add(param);

//    //Timing_Option t2 = new Timing_Option()
//    //{
//    //    Name = "test",
//    //    Time = 2,
//    //    Timing_WorkStations = wks,
//    //    Fixture = fixture

//    //};
//    //t2.Categories.Add(cat);
//    //t2.Parameters.Add(param);

//    //t1.Parameters.Equals(t2.Parameters);
//    //t1.Parameters.SequenceEqual(t2.Parameters);
//    ////Enumerable.SequenceEqual<Parameter>(IEnumerable < Parameter > t1.Parameters, IEnumerable < Parameter > t2.Parameters);
//    //test.Add(t1);
//    //t1.Equals(t2);
//    //test.Remove(t2);
//    //test.Add(t2);

//    //var fixture = db.Fixtures.Where(r => r.Code == "BBRLED").FirstOrDefault();
//    //Utilities.addLengthTimeOption(fixture.Code);
//}

