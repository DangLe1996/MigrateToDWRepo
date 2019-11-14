using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MigrateToDW1
{
    class Program
    {

        static void Main(string[] args)
        {


            

        



            using (var db = new DWModel())
            {

                string fxCode = "BBRLED";
                //string OpCode = "CUEX0000";
                //string ParamCode = "4";
                // int LengthID = 8;
                //int SizeID = 20;


                //var timingopt = db.Timing_Option.Where(r => r.Fixture.Code == fxCode && r.Timing_WorkStations.OpCode == OpCode).ToList();



                //var category = db.Categories.Where(r => r.id == LengthID).FirstOrDefault();
                //var param = db.Parameters.Where(r => r.Code == ParamCode).FirstOrDefault();
                //var workstation = db.Timing_WorkStations.Where(r => r.OpCode == OpCode).FirstOrDefault();
                var fixture = db.Fixtures.Where(r => r.Code == fxCode).FirstOrDefault();
                //var input = new Timing_Option()
                //{
                //    Fixture = fixture,
                //    Name = "Standard",
                //    Time = 1,
                //    Timing_WorkStations = workstation
                    
                //};
                //input.Categories.Add(category);
                //input.Parameters.Add(param);

                //db.Timing_Option.Add(input);
                //db.SaveChanges();

                int x = 3;
                var wks = db.Timing_WorkStations.FirstOrDefault();
                var param = db.Parameters.FirstOrDefault();
                var cat = db.Categories.FirstOrDefault();
                HashSet<Timing_Option> test = new HashSet<Timing_Option>();

                Timing_Option t1 = new Timing_Option()
                {
                    Name = "test",
                    Time = 1,
                    Timing_WorkStations = wks,
                    Fixture = fixture
                    

                };
                t1.Categories.Add(cat);
                t1.Parameters.Add(param);

                Timing_Option t2 = new Timing_Option()
                {
                    Name = "test",
                    Time = 2,
                    Timing_WorkStations = wks,
                    Fixture = fixture

                };
                t2.Categories.Add(cat);
                t2.Parameters.Add(param);

                t1.Parameters.Equals(t2.Parameters);
                t1.Parameters.SequenceEqual(t2.Parameters);
                //Enumerable.SequenceEqual<Parameter>(IEnumerable < Parameter > t1.Parameters, IEnumerable < Parameter > t2.Parameters);
                test.Add(t1);
                t1.Equals(t2);
                test.Remove(t2);
                test.Add(t2);

                //var fixture = db.Fixtures.Where(r => r.Code == "BBRLED").FirstOrDefault();
                //Utilities.addLengthTimeOption(fixture.Code);
            }


        }
    }
}
