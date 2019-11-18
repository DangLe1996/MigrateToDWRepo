using AXISAutomation.FixtureConfiguration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeCalculator
{
    class Utilities
    {

        public static List<Timing_Option> OptionList;

        public static int LengthID = 8;
        public static int SizeID = 20;

        public static void addWorkstations()
        {
            using (var db = new SSRS())
            {
                //var wks = db.ProdTBs.ToList();
                var wks = db.ProdTBs.Select(x => new { x.WorkCenter, x.OpCode }).Distinct().ToList();


                foreach (var item in wks)
                {
                    if (item.OpCode != null)
                    {
                        var wk = new Timing_WorkStations()
                        {
                            Name = item.WorkCenter,

                            OpCode = item.OpCode
                        };

                        using (var DWdb = new DWModel())
                        {
                            DWdb.Timing_WorkStations.Add(wk);
                            DWdb.SaveChanges();
                        }

                    }
                }
            }
        }

        public static void Test()
        {
            using (var DWdb = new DWModel())
            {
                var optionlist = DWdb.Timing_Option.ToList();
                var fixture = DWdb.Fixtures.FirstOrDefault();
                var test = fixture.CategoryAtFixtures.Where(r => r.Category.Name == "CIRCUITS").FirstOrDefault();
                var test2 = test.ParameterAtCategoryAtFixtures.Where(r => r.Parameter.Code == "1").FirstOrDefault();
                var param = DWdb.Parameters.FirstOrDefault();
                
            }

        }

        public static void LinkParams()
        {
            using (var DWdb = new DWModel())
            {
                using (var db = new SSRS())
                {
                    foreach (var timeoption in DWdb.Timing_Option.ToList())
                    {

                        var prodTD = db.ProdTBs.Where(r => r.Code == timeoption.Fixture.Code
                        && r.WorkCenter == timeoption.Timing_WorkStations.Name).FirstOrDefault();
                        foreach (var item in prodTD.OptionTBs)
                        {
                            foreach (var param in item.ParametersTBs)
                            {

                                var fixture = timeoption.Fixture;
                                var category = fixture.CategoryAtFixtures.Where(r => r.Category.Name == param.ParamName).FirstOrDefault();
                                if (category != null)
                                {
                                    var parameter = category.ParameterAtCategoryAtFixtures.Where(r => r.Parameter.Code == param.ParamValue).FirstOrDefault();

                                    if (parameter != null)
                                    {
                                        //Console.WriteLine("OK");
                                        timeoption.Parameters.Add(parameter.Parameter);
                                        //DWdb.SaveChanges();
                                    }
                                }

                            }

                        }
                    }
                }

            }
            
        }

        public static void LinkCategory()
        {
            using (var DWdb = new DWModel())
            {
                using (var db = new SSRS())
                {
                    foreach (var timeoption in DWdb.Timing_Option.ToList())
                    {

                        var prodTD = db.ProdTBs.Where(r => r.Code == timeoption.Fixture.Code
                        && r.WorkCenter == timeoption.Timing_WorkStations.Name).FirstOrDefault();
                        foreach (var item in prodTD.OptionTBs)
                        {
                            foreach (var param in item.ParametersTBs)
                            {
                                var cat = DWdb.Categories.Where(r => r.Name == param.ParamName).FirstOrDefault();
                                if (cat != null)
                                {
                                    Console.WriteLine(cat.Name);
                                }

                            }

                        }
                    }
                }
            }
        }

        public static void addTimeOption()
        {
            HashSet<Timing_Option> globalopts = new HashSet<Timing_Option>();

            List<Fixture> fixtures = new List<Fixture>();

            using (var DWdb = new DWModel())
            {

                using (var db = new SSRS())
                {
                    var prod = db.ProdTBs.ToList();
                    foreach (var item in prod)
                    {
                        foreach (var opt in item.OptionTBs)
                        {
                            var fixture = DWdb.Fixtures.Where(r => r.Code == item.Code).FirstOrDefault();
                            var workstation = DWdb.Timing_WorkStations.Where(r => r.Name == item.WorkCenter).FirstOrDefault();
                            if (fixture != null && workstation != null)
                            {
                                var optnew = new Timing_Option()
                                {
                                    Name = opt.OptionName,
                                    Time = opt.ProdTime,
                                    Fixture = fixture,

                                    Timing_WorkStations = workstation


                                };
                                //DWdb.Timing_Option.Add(optnew);
                                //DWdb.SaveChanges();
                                globalopts.Add(optnew);
                            }

                        }

                    }
                }

                foreach (var item in globalopts)
                {
                    DWdb.Timing_Option.Add(item);
                    DWdb.SaveChanges();
                }
            }


        }

        public static Timing_Option addTimeOptionCustom(Fixture fixture, List<Category> Categories, List<Parameter> Parameters, Timing_WorkStations workStations, string OptionName, double OptionValue)
        {


           

            Timing_Option Input = new Timing_Option()
            {
                Name = OptionName,
                Time = OptionValue,
                Fixture = fixture,
                Categories = Categories,
                Parameters = Parameters,
                Timing_WorkStations = workStations
                
            };

            return Input;
          
        }

        public static Timing_Option addTimeOptionCustom(Fixture fixture, Category Category, Parameter Parameter, Timing_WorkStations workStations, string OptionName, double OptionValue)
        {
            Timing_Option Input = new Timing_Option()
            {
                Name = OptionName,
                Time = OptionValue,
                Fixture = fixture,
                Timing_WorkStations = workStations
            };

            //Input.Categories.Add(Category);
            Input.Parameters.Add(Parameter);
            return Input;
            
        }

        public static List<Timing_Option> GetTiming_Options()
        {
            using (var db = new DWModel())
            {
                return db.Timing_Option.ToList();
            }
        }



        //public static List<string> getLengthParams(string fixtureCode)
        //{
        //    List<string> list = new List<string>();
        //    using (var db = new DWModel())
        //    {
        //         list = (from fx in db.Fixtures
        //                    join fica in db.CategoryAtFixtures on fx.id equals fica.FixtureId
        //                    join cat in db.Categories on fica.CategoryId equals cat.id
        //                    join paca in db.ParameterAtCategoryAtFixtures on fica.id equals paca.CategoryAtFixtureId
        //                    join param in db.Parameters on paca.ParameterId equals param.id
        //                    where fx.Code == fixtureCode && cat.id == LengthID
        //                 select param.Code).ToList();
        //    }
        //    return list;
        //}

        public static List<Parameter> getLengthParams(string fixtureCode)
        {
            List<Parameter> list = new List<Parameter>();
            List<string> LengthParams = new List<string>()
                {
                    "4","8","12"
                };
            using (var db = new DWModel())
            {
                list = (from fx in db.Fixtures
                        join fica in db.CategoryAtFixtures on fx.id equals fica.FixtureId
                        join cat in db.Categories on fica.CategoryId equals cat.id
                        join paca in db.ParameterAtCategoryAtFixtures on fica.id equals paca.CategoryAtFixtureId
                        join param in db.Parameters on paca.ParameterId equals param.id
                        where fx.Code == fixtureCode && cat.id == LengthID && LengthParams.Contains(param.Code)
                        select param).ToList();
            }
            return list;
        }
        public static List<Parameter> getSizeParams(string fixtureCode)
        {
            List<Parameter> list = new List<Parameter>();
            List<string> SizeParams = new List<string>()
                {

                    "11","12","14","22","24","26","315","33","35","44"
                };
            using (var db = new DWModel())
            {
                list = (from fx in db.Fixtures
                        join fica in db.CategoryAtFixtures on fx.id equals fica.FixtureId
                        join cat in db.Categories on fica.CategoryId equals cat.id
                        join paca in db.ParameterAtCategoryAtFixtures on fica.id equals paca.CategoryAtFixtureId
                        join param in db.Parameters on paca.ParameterId equals param.id
                        where fx.Code == fixtureCode && cat.id == SizeID && SizeParams.Contains(param.Code)
                        select param).ToList();
            }
            return list;
        }


        public void addTimeOption(string fixtureCode, List<string> CategoryCode, List<string> ParamsCode, string WorkCenter, string Name, double TimeValue)
        {
           
        }




        public static void addLengthTimeOption(string fixtureCode)
        {
            using (var db = new DWModel())
            {
                var fixture = db.Fixtures.Where(r => r.Code == fixtureCode).FirstOrDefault();
                var category = db.Categories.Where(r => r.id == LengthID).FirstOrDefault();
                List<string> LengthParams = new List<string>()
                {
                    "4","8","12"
                };

                foreach (var wks in db.Timing_WorkStations.ToList()) {
                
                    var paramlist = (from fx in db.Fixtures
                                     join fica in db.CategoryAtFixtures on fx.id equals fica.FixtureId
                                     join cat in db.Categories on fica.CategoryId equals cat.id
                                     join paca in db.ParameterAtCategoryAtFixtures on fica.id equals paca.CategoryAtFixtureId
                                     join param in db.Parameters on paca.ParameterId equals param.id
                                     where fx.Code == fixtureCode && cat.id == LengthID && LengthParams.Contains(param.Code)
                                     select param).ToList();
                    foreach (var param in paramlist)
                    {
                        var newoption = addTimeOptionCustom(fixture, category, param, wks, "Standard",1);
                        OptionList.Add(newoption);

                        db.Timing_Option.Add(newoption);
                        db.SaveChanges();
                    }
                }

            }
        }

        public static void addSizeTimeOption(string fixtureCode)
        {
            List<string> SizeParams = new List<string>()
                {

                    "11","12","14","22","24","26","315","33","35","44"
                };
            using (var db = new DWModel())
            {
                var fixture = db.Fixtures.Where(r => r.Code == fixtureCode).FirstOrDefault();
                var category = db.Categories.Where(r => r.id == SizeID).FirstOrDefault();
                //category.Footnote = " ";
                
                foreach (var wks in db.Timing_WorkStations.ToList())
                {
                    var paramList =  (from fx in db.Fixtures
                                            join fica in db.CategoryAtFixtures on fx.id equals fica.FixtureId
                                            join cat in db.Categories on fica.CategoryId equals cat.id
                                            join paca in db.ParameterAtCategoryAtFixtures on fica.id equals paca.CategoryAtFixtureId
                                            join param in db.Parameters on paca.ParameterId equals param.id
                                            where fx.Code == fixtureCode && cat.id == SizeID && SizeParams.Contains(param.Code)
                                            select param).ToList();
                    foreach (var param in paramList)
                    {
                        var newoption = addTimeOptionCustom(fixture, category, param, wks, "Standard", 1);
                        OptionList.Add(newoption);
                        db.Timing_Option.Add(newoption);
                        db.SaveChanges();
                    }
                }


            }
        }



        public static Timing_Option ContainsTimeOption(Fixture fixture, Timing_Option timing_Option)
        {



            var result = fixture.Timing_Option.Where(r => r.Name == timing_Option.Name
                && r.Categories.Equals(timing_Option.Categories)
                && r.Parameters.Equals(timing_Option.Parameters)
                && r.Fixture == timing_Option.Fixture
                && r.Timing_WorkStations == timing_Option.Timing_WorkStations).FirstOrDefault();


            return result;
        }

      



        public static void UploadStandardTime()
        {

            List<string> listA = new List<string>();
            using (var reader = new StreamReader(@"C:\Users\dangl\source\repos\MigrateToDW\TimeCalculator\ProductEpicor.csv"))
            {
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split(';');
                    listA.Add(values[0]);

                }
            }


                OptionList = new List<Timing_Option>();
            using (var db = new DWModel())
            {
                
                foreach (var fixture in db.Fixtures.ToList())
                {
                    if (listA.Contains(fixture.Code))
                    {

                        Console.WriteLine(fixture.Code);
                        if (Utilities.getLengthParams(fixture.Code).Count > 0)
                        {
                            Utilities.addLengthTimeOption(fixture.Code);
                        }
                        else if (Utilities.getSizeParams(fixture.Code).Count > 0)
                        {
                            Utilities.addSizeTimeOption(fixture.Code);
                        }
                        else
                        {
                            Console.WriteLine("{0} does not have size or legnth param", fixture.Code);
                        }
                    }

                }
            }

            //before your loop
            var csv = new StringBuilder();
            
            csv.AppendLine("FamilyName,Fixture, WorkStation, Length, Time");
            foreach (var item in OptionList)
            {
                if (listA.Contains(item.Fixture.Code))
                {
                    var newLine = string.Format("{0},{1},{2},{3}",item.Fixture.FamilyName, item.Fixture.Code,
                        item.Timing_WorkStations.Name, item.Parameters.FirstOrDefault().Description);
                    csv.AppendLine(newLine);
                }
            }
            string filepath = @"C:\Users\dangl\source\repos\MigrateToDW\TimeCalculator\Input.csv";
            File.WriteAllText(filepath, csv.ToString());

        }




        public static AXISAutomation.FixtureConfiguration._Fixture ConfigDW(string lineDesc)
        {
            string DWConnectiontring = "metadata = res://*/DBConnection.csdl|res://*/DBConnection.ssdl|res://*/DBConnection.msl;provider=System.Data.SqlClient;provider connection string='Data Source=VAULT\\DRIVEWORKS;Initial Catalog=\"AXIS Automation\";Integrated Security=True;MultipleActiveResultSets=True'";
            using (AXISAutomation.Tools.DBConnection.AXIS_AutomationEntities _AutomationEntities = new AXISAutomation.Tools.DBConnection.AXIS_AutomationEntities(DWConnectiontring))
            {
                _Fixture fixture = new AXISAutomation.FixtureConfiguration._Fixture(lineDesc, _AutomationEntities);

                fixture.SPM.ConfigureAll();

                return fixture;
            }
        }
    }
}
