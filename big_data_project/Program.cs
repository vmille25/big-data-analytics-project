using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using YahooFinanceApi;
using System.Windows.Forms.DataVisualization.Charting;
//using System.Drawing;
namespace big_data_project
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] symbols = File.ReadAllLines(@"C:\Users\vdmil\OneDrive\Documents\KSU_SPRING_2021\Big Data Analytics\project_code\big_data_project\big_data_project\symbolList.txt");
            string[] sectors = File.ReadAllLines(@"C:\Users\vdmil\OneDrive\Documents\KSU_SPRING_2021\Big Data Analytics\project_code\big_data_project\big_data_project\sectorList.txt");
            string[] surprise = File.ReadAllLines(@"C:\Users\vdmil\OneDrive\Documents\KSU_SPRING_2021\Big Data Analytics\project_code\big_data_project\big_data_project\EPS_surprise.txt");
            string[] earningsDateStr = File.ReadAllLines(@"C:\Users\vdmil\OneDrive\Documents\KSU_SPRING_2021\Big Data Analytics\project_code\big_data_project\big_data_project\earningsDate.txt");
            string[] endDateStr = File.ReadAllLines(@"C:\Users\vdmil\OneDrive\Documents\KSU_SPRING_2021\Big Data Analytics\project_code\big_data_project\big_data_project\endDate.txt");
            DateTime[] earningsDate = new DateTime[earningsDateStr.Length];
            DateTime[] endDate = new DateTime[earningsDateStr.Length];
            for (int i = 0; i < earningsDateStr.Length; i++)
            {
                var parsedDate = DateTime.Parse(earningsDateStr[i]);
                earningsDate[i] = parsedDate.AddDays(-1);
                parsedDate = DateTime.Parse(endDateStr[i]);
                endDate[i] = parsedDate.AddDays(-1);
               // Console.WriteLine(earningsDate[i]);
               // Console.WriteLine(endDate[i]);
               // Console.WriteLine();
            }
            retrievePriceData(symbols, earningsDate, endDate, sectors, surprise);
            Console.ReadKey();
        }
        public static void retrievePriceData(string[] symbols, DateTime[] earningsDate, DateTime[] endDate, string[] sectors, string[] surprise)
        {
            int i = 0;
            stockData stockDataObj = new stockData();
            while (i < symbols.Length)
            {
                var awaiter = stockDataObj.getStockData(symbols[i], earningsDate[i], endDate[i], sectors[i], surprise[i]);
                if (awaiter.Result == 1)
                {
                    i++;
                }
            }
            stockDataObj.genChart();
            return;
        }
    }
    class stockData
    {
        public List<(double, double, string)> plottableData = new List<(double, double, string)>();
        public async Task<int> getStockData(string symbol, DateTime startDate, DateTime endDate, string sector, string surprise)
        {
            try
            {
                var historic_data = await Yahoo.GetHistoricalAsync(symbol, startDate, endDate, Period.Daily);
                double priceDifference = Convert.ToDouble((historic_data[historic_data.Count - 1].Close - historic_data[0].Close) / historic_data[0].Close) * 100;
                double surprisePercentNum = Convert.ToDouble(surprise);
                (double, double, string) dataToEnter = (surprisePercentNum, priceDifference, sector);
                if (surprisePercentNum < 100 && surprisePercentNum > -100)
                {
                    plottableData.Add(dataToEnter);
                }
            }
            catch
            {
                Console.WriteLine("Failed to get historical data for: " + symbol);
            }
            return 1;
        }
        public void genChart()
        {
            List<(double, double)> industrialData = new List<(double, double)>();
            List<(double, double)> healthCareData = new List<(double, double)>();
            List<(double, double)> informationTechData = new List<(double, double)>();
            List<(double, double)> commServicesData = new List<(double, double)>();
            List<(double, double)> consumerDiscData = new List<(double, double)>();
            List<(double, double)> utilitiesData = new List<(double, double)>();
            List<(double, double)> financialsData = new List<(double, double)>();
            List<(double, double)> materialsData = new List<(double, double)>();
            List<(double, double)> realEstateData = new List<(double, double)>();
            List<(double, double)> consumerStaplesData = new List<(double, double)>();
            List<(double, double)> energyData = new List<(double, double)>();

            for (int i = 0; i < plottableData.Count; i++)
            {
                if (plottableData[i].Item3 == "Industrials")
                {
                    (double, double) temp = (plottableData[i].Item1, plottableData[i].Item2);
                    industrialData.Add(temp);
                }
            }
            for (int i = 0; i < plottableData.Count; i++)
            {
                if (plottableData[i].Item3 == "Health Care")
                {
                    (double, double) temp = (plottableData[i].Item1, plottableData[i].Item2);
                    healthCareData.Add(temp);
                }
            }
            for (int i = 0; i < plottableData.Count; i++)
            {
                if (plottableData[i].Item3 == "Information Technology")
                {
                    (double, double) temp = (plottableData[i].Item1, plottableData[i].Item2);
                    informationTechData.Add(temp);
                }
            }
            for (int i = 0; i < plottableData.Count; i++)
            {
                if (plottableData[i].Item3 == "Communication Services")
                {
                    (double, double) temp = (plottableData[i].Item1, plottableData[i].Item2);
                    commServicesData.Add(temp);
                }
            }
            for (int i = 0; i < plottableData.Count; i++)
            {
                if (plottableData[i].Item3 == "Consumer Discretionary")
                {
                    (double, double) temp = (plottableData[i].Item1, plottableData[i].Item2);
                    consumerDiscData.Add(temp);
                }
            }
            for (int i = 0; i < plottableData.Count; i++)
            {
                if (plottableData[i].Item3 == "Utilities")
                {
                    (double, double) temp = (plottableData[i].Item1, plottableData[i].Item2);
                    utilitiesData.Add(temp);
                }
            }
            for (int i = 0; i < plottableData.Count; i++)
            {
                if (plottableData[i].Item3 == "Financials")
                {
                    (double, double) temp = (plottableData[i].Item1, plottableData[i].Item2);
                    financialsData.Add(temp);
                }
            }
            for (int i = 0; i < plottableData.Count; i++)
            {
                if (plottableData[i].Item3 == "Materials")
                {
                    (double, double) temp = (plottableData[i].Item1, plottableData[i].Item2);
                    materialsData.Add(temp);
                }
            }
            for (int i = 0; i < plottableData.Count; i++)
            {
                if (plottableData[i].Item3 == "Real Estate")
                {
                    (double, double) temp = (plottableData[i].Item1, plottableData[i].Item2);
                    realEstateData.Add(temp);
                }
            }
            for (int i = 0; i < plottableData.Count; i++)
            {
                if (plottableData[i].Item3 == "Consumer Staples")
                {
                    (double, double) temp = (plottableData[i].Item1, plottableData[i].Item2);
                    consumerStaplesData.Add(temp);
                }
            }
            for (int i = 0; i < plottableData.Count; i++)
            {
                if (plottableData[i].Item3 == "Energy")
                {
                    (double, double) temp = (plottableData[i].Item1, plottableData[i].Item2);
                    energyData.Add(temp);
                }
            }

            Chart chart = new Chart();
            ChartArea CA = chart.ChartAreas.Add("A1");

            Series industrials = new Series("Industrials");
            for(int i = 0; i < industrialData.Count; i++)
            {
                industrials.Points.AddXY(industrialData[i].Item1, industrialData[i].Item2);
            }
            industrials.ChartType = SeriesChartType.Point;
            industrials.MarkerStyle = MarkerStyle.Circle;
            industrials.MarkerColor = System.Drawing.Color.Green;

            Series healthCare = new Series("Health Care");
            for (int i = 0; i < healthCareData.Count; i++)
            {
                healthCare.Points.AddXY(healthCareData[i].Item1, healthCareData[i].Item2);
            }
            healthCare.ChartType = SeriesChartType.Point;
            healthCare.MarkerStyle = MarkerStyle.Circle;
            healthCare.MarkerColor = System.Drawing.Color.Blue;

            Series informationTech = new Series("Information Technology");
            for (int i = 0; i < informationTechData.Count; i++)
            {
                informationTech.Points.AddXY(informationTechData[i].Item1, informationTechData[i].Item2);
            }
            informationTech.ChartType = SeriesChartType.Point;
            informationTech.MarkerStyle = MarkerStyle.Circle;
            informationTech.MarkerColor = System.Drawing.Color.Red;

            Series commServices = new Series("Communication Services");
            for (int i = 0; i < commServicesData.Count; i++)
            {
                commServices.Points.AddXY(commServicesData[i].Item1, commServicesData[i].Item2);
            }
            commServices.ChartType = SeriesChartType.Point;
            commServices.MarkerStyle = MarkerStyle.Circle;
            commServices.MarkerColor = System.Drawing.Color.Black;

            Series consumerDisc = new Series("Consumer Discretionary");
            for (int i = 0; i < consumerDiscData.Count; i++)
            {
                consumerDisc.Points.AddXY(consumerDiscData[i].Item1, consumerDiscData[i].Item2);
            }
            consumerDisc.ChartType = SeriesChartType.Point;
            consumerDisc.MarkerStyle = MarkerStyle.Circle;
            consumerDisc.MarkerColor = System.Drawing.Color.Yellow;

            Series utilities = new Series("Utilities");
            for (int i = 0; i < utilitiesData.Count; i++)
            {
                utilities.Points.AddXY(utilitiesData[i].Item1, utilitiesData[i].Item2);
            }
            utilities.ChartType = SeriesChartType.Point;
            utilities.MarkerStyle = MarkerStyle.Circle;
            utilities.MarkerColor = System.Drawing.Color.Orange;

            Series financials = new Series("Financials");
            for (int i = 0; i < financialsData.Count; i++)
            {
                financials.Points.AddXY(financialsData[i].Item1, financialsData[i].Item2);
            }
            financials.ChartType = SeriesChartType.Point;
            financials.MarkerStyle = MarkerStyle.Circle;
            financials.MarkerColor = System.Drawing.Color.Purple;

            Series materials = new Series("Materials");
            for (int i = 0; i < materialsData.Count; i++)
            {
                materials.Points.AddXY(materialsData[i].Item1, materialsData[i].Item2);
            }
            materials.ChartType = SeriesChartType.Point;
            materials.MarkerStyle = MarkerStyle.Circle;
            materials.MarkerColor = System.Drawing.Color.HotPink;

            Series realEstate = new Series("Real Estate");
            for (int i = 0; i < realEstateData.Count; i++)
            {
                realEstate.Points.AddXY(realEstateData[i].Item1, realEstateData[i].Item2);
            }
            realEstate.ChartType = SeriesChartType.Point;
            realEstate.MarkerStyle = MarkerStyle.Circle;
            realEstate.MarkerColor = System.Drawing.Color.LightGreen;

            Series consumerStaples = new Series("Consumer Staples");
            for (int i = 0; i < consumerStaplesData.Count; i++)
            {
                consumerStaples.Points.AddXY(consumerStaplesData[i].Item1, consumerStaplesData[i].Item2);
            }
            consumerStaples.ChartType = SeriesChartType.Point;
            consumerStaples.MarkerStyle = MarkerStyle.Circle;
            consumerStaples.MarkerColor = System.Drawing.Color.LightBlue;

            Series energy = new Series("Energy");
            for (int i = 0; i < energyData.Count; i++)
            {
                energy.Points.AddXY(energyData[i].Item1, energyData[i].Item2);
            }
            energy.ChartType = SeriesChartType.Point;
            energy.MarkerStyle = MarkerStyle.Circle;
            energy.MarkerColor = System.Drawing.Color.Gray;

            chart.Series.Clear();
            chart.Series.Add(industrials);
            chart.Series.Add(healthCare);
            chart.Series.Add(informationTech);
            chart.Series.Add(commServices);
            chart.Series.Add(consumerDisc);
            chart.Series.Add(utilities);
            chart.Series.Add(financials);
            chart.Series.Add(materials);
            chart.Series.Add(realEstate);
            chart.Series.Add(consumerStaples);
            chart.Series.Add(energy);

            chart.ResetAutoValues();
            chart.Titles.Clear();
            chart.Titles.Add($"Earnings Surprise VS. Price Change");
            chart.ChartAreas[0].AxisX.Title = "Earnings Surprise (%)";
            chart.ChartAreas[0].AxisY.Title = "Price Change (%)";
            chart.ChartAreas[0].AxisX.MajorGrid.LineColor = System.Drawing.Color.LightGray;
            chart.AntiAliasing = AntiAliasingStyles.Graphics;
            chart.TextAntiAliasingQuality = TextAntiAliasingQuality.High;
            chart.Size = new System.Drawing.Size(1000, 800);
            chart.Titles.ElementAt(0).Font = new System.Drawing.Font("Arial", 25, System.Drawing.FontStyle.Bold);
            CA.AxisX.TitleAlignment = System.Drawing.StringAlignment.Center;
            CA.AxisY.TitleAlignment = System.Drawing.StringAlignment.Center;
            CA.AxisX.TitleFont = new System.Drawing.Font("Arial", 15, System.Drawing.FontStyle.Bold);
            CA.AxisY.TitleFont = new System.Drawing.Font("Arial", 15, System.Drawing.FontStyle.Bold);
            chart.BackColor = System.Drawing.Color.Transparent;
            CA.BackColor = System.Drawing.Color.Transparent;

            chart.SaveImage(@"C:\Users\vdmil\OneDrive\Documents\KSU_SPRING_2021\Big Data Analytics\project_code\image\produced_chart.png", ChartImageFormat.Png);
            Console.WriteLine("All Done :)))");
        }
    }
}
