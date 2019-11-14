using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MigrateToDW1
{
    class Utilities
    {
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

            Input.Categories.Add(Category);
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



        public static List<string> getLengthParams(string fixtureCode)
        {
            List<string> list = new List<string>();
            using (var db = new DWModel())
            {
                 list = (from fx in db.Fixtures
                            join fica in db.CategoryAtFixtures on fx.id equals fica.FixtureId
                            join cat in db.Categories on fica.CategoryId equals cat.id
                            join paca in db.ParameterAtCategoryAtFixtures on fica.id equals paca.CategoryAtFixtureId
                            join param in db.Parameters on paca.ParameterId equals param.id
                            where fx.Code == fixtureCode && cat.id == LengthID
                         select param.Code).ToList();
            }
            return list;
        }
        public static List<string> getSizeParams(string fixtureCode)
        {
            List<string> list = new List<string>();
            using (var db = new DWModel())
            {
                list = (from fx in db.Fixtures
                        join fica in db.CategoryAtFixtures on fx.id equals fica.FixtureId
                        join cat in db.Categories on fica.CategoryId equals cat.id
                        join paca in db.ParameterAtCategoryAtFixtures on fica.id equals paca.CategoryAtFixtureId
                        join param in db.Parameters on paca.ParameterId equals param.id
                        where fx.Code == fixtureCode && cat.id == SizeID
                        select param.Code).ToList();
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
                    foreach (var param in db.Parameters.Where(r => LengthParams.Contains(r.Code)).ToList())
                    {
                        var newoption = addTimeOptionCustom(fixture, category, param, wks, "Standard",1);
                        db.Timing_Option.Add(newoption);
                        db.SaveChanges();
                    }
                }

            }
        }

        public static void addSizeTimeOption(string fixtureCode)
        {
            using (var db = new DWModel())
            {
                var fixture = db.Fixtures.Where(r => r.Code == fixtureCode).FirstOrDefault();
                var category = db.Categories.Where(r => r.id == SizeID).FirstOrDefault();

                List<string> SizeParams = new List<string>()
                {
                   
                    "11","12","14","22","24","26","315","33","35","44"
                };
                foreach (var wks in db.Timing_WorkStations.ToList())
                {
                    foreach (var param in db.Parameters.Where(r => SizeParams.Contains(r.Code)).ToList())
                    {
                        var newoption = addTimeOptionCustom(fixture, category, param, wks, "Standard", 1);
                        db.Timing_Option.Add(newoption);
                        db.SaveChanges();
                    }
                }


            }
        }


        public static void UploadStandardTime()
        {
            using (var db = new DWModel())
            {
                foreach (var fixture in db.Fixtures.ToList())
                {
                    var lengthParam = Utilities.getLengthParams(fixture.Code);
                    var sizeParam = Utilities.getSizeParams(fixture.Code);
                    if (lengthParam.Count > 0)
                    {
                        Utilities.addLengthTimeOption(fixture.Code);
                    }
                    else if (sizeParam.Count > 0)
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

    }
}
