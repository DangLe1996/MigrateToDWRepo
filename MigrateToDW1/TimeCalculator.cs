using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AXISAutomation.FixtureConfiguration;

namespace TimeCalculator
{
    class TimeCalculator
    {
        //private Dictionary<string,>

        private string fixtureCode;
        private int? productID;
        private decimal fixtureLength;
        public Dictionary<string, double> ProductionTimeERP = new Dictionary<string, double>();

        public TimeCalculator(_Fixture fixture, int customSection = 0)
        {

            productID = fixture.ProductID;
            fixtureCode = fixture.ProductCode;
            fixtureLength = fixture.Selection.RequestedLengthNormalizedToDecimalInch/12;
            if (fixture.Sections.Items.Count != 0)
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

        private void getSectionTime(decimal Length)
        {
            string lengthCode = Length >= 12 ? "12" : Length >= 8 ? "8" : "4";
            using (var db = new DWModel())
            {
                var SectionTime = db.Fixtures.Where(r => r.id == productID
                && r.Timing_Option.Any(q => q.Name == "Standard")
                ).Select(r => r.Timing_Option.Where(z => z.Parameters.Any(k => k.Code == lengthCode))
                .ToList()).FirstOrDefault()
                .ToDictionary(o => o.Timing_WorkStations.OpCode, o => o.Time);

                if (ProductionTimeERP == null)
                {
                    ProductionTimeERP = SectionTime;
                }
                else
                {
                    ProductionTimeERP = ProductionTimeERP.Concat(SectionTime).GroupBy(x => x.Key).ToDictionary(x => x.Key, x => x.Sum(y => y.Value));
                }
            }


        }


        
    }
}
