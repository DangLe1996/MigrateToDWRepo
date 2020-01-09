using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AXISAutomation.FixtureConfiguration;

namespace TimeCalculator
{
    public class TimeCalculator
    {

        private string fixtureCode;
        private int? productID;
        private decimal fixtureLength;
        public Dictionary<string, decimal> ProductionTimeERP = new Dictionary<string, decimal>();
        public List<section> SectionResults = new List<section>();

        public struct section
        {
            public string length;
            public Dictionary<string, decimal> result;
            public string size;
        }

        public TimeCalculator(_Fixture fixture, int customSection = 0)
        {
            SectionResults = new List<section>();
            productID = fixture.ProductID;
            fixtureCode = fixture.ProductCode;
            fixtureLength = fixture.Selection.RequestedLengthNormalizedToDecimalInch/12;
            if (fixture.Sections.Items.Count != 0 && customSection == 0)
            {
                foreach (var section in fixture.Sections.Items)
                {
                    decimal Length = section.Length / 12;
                    getSectionTime(Length);
                }
            }
            else if(fixtureLength != 0 && customSection != 0)
            {
                decimal Length = fixtureLength / customSection;
                for(int i = 0; i < customSection; i++)
                {
                    getSectionTime(Length);
                }
            }

            else if (fixture.Selection.Size != null)
            {
                getSectionTime(fixture.Selection.Size.SelectionDescription);
            }

            else
            {
                if(fixtureLength == 0)
                {
                    Console.WriteLine("Cannot determine Length");
                }
                if(customSection == 0)
                {
                    Console.WriteLine("Cannot determine Section Number");
                }
            }


        }
        private void getSectionTime(string size)
        {
            using (var db = new DWModel(true))
            {
                var SectionTime = db.Fixtures.Where(r => r.id == productID
               && r.Timing_Option.Any(q => q.Name == "Standard")
               ).Select(r => r.Timing_Option.Where(z => z.Parameters.Any(k => k.Description == size))
               .ToList()).FirstOrDefault()
               .ToDictionary(o => o.Timing_WorkStations.OpCode, o => o.Time);
                ProductionTimeERP = ProductionTimeERP == null ? SectionTime : ProductionTimeERP.Concat(SectionTime).GroupBy(x => x.Key).ToDictionary(x => x.Key, x => x.Sum(y => y.Value));
                SectionResults.Add(new section() { size = size, result = ProductionTimeERP });

            }
        }


        private void getSectionTime(decimal Length)
        {
            List<string> productParams = new List<string>();
            string lengthCode = Length >= 12 ? "12'" : Length >= 8 ? "8'" : "4'";
            productParams.Add(lengthCode);
            getStandardTime(productParams);
            SectionResults.Add(new section() { length = lengthCode, result = ProductionTimeERP });

        }

        void updateTime(Dictionary<string, decimal> SectionTime)
        {
            if (ProductionTimeERP == null)
            {
                ProductionTimeERP = SectionTime;
            }
            else
            {
                ProductionTimeERP = ProductionTimeERP.Concat(SectionTime).GroupBy(x => x.Key).ToDictionary(x => x.Key, x => x.Sum(y => y.Value));
            }
           
        }
     

        void getStandardTime(List<string> Parameters)
        {
            Dictionary<string, decimal> SectionTime = new Dictionary<string, decimal>();
            
            using (var db = new DWModel())
            {

                var optionList = (db.Fixtures.Where(r => r.id == productID).Select(r => r.Timing_Option).ToList()).FirstOrDefault();
                foreach (var item in optionList.AsEnumerable())
                {
                    var paramValue = item.Parameters.Select(r => r.Description).ToList();
                    if (Parameters.All(paramValue.Contains))
                    {
                        SectionTime.Add(item.Timing_WorkStations.OpCode, item.Time);
                        
                    }
                }

            }
            updateTime(SectionTime);
            ProductionTimeERP = ProductionTimeERP == null ? SectionTime : ProductionTimeERP.Concat(SectionTime).GroupBy(x => x.Key).ToDictionary(x => x.Key, x => x.Sum(y => y.Value));

        }


    }
}
