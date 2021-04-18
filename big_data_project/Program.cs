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
            List<(double, double)> allSectorsData = new List<(double, double)>();
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
                (double, double) temp = (plottableData[i].Item1, plottableData[i].Item2);
                allSectorsData.Add(temp);
            }

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
            chart.Titles.Add($"Earnings Surprise VS. Price Change (No k Query)");
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

            chart.SaveImage(@"C:\Users\vdmil\OneDrive\Documents\KSU_SPRING_2021\Big Data Analytics\project_code\image\no_top_k.png", ChartImageFormat.Png);
            Console.WriteLine("No Top-K Chart Produced :)");

            Console.WriteLine();

            Console.WriteLine("Unfiltered Data, Pearsons Correlation Coefficient is shown below:");
            //pearsonsCorrelationCoeff(allSectorsData, "All Sectors");
            double pearsonsCoeff = 0;

            pearsonsCoeff = pearsonsCorrelationCoeff(industrialData);
            Console.WriteLine("Pearsons Coefficient for Industrial Sector is: " + pearsonsCoeff.ToString());

            pearsonsCoeff = pearsonsCorrelationCoeff(healthCareData);
            Console.WriteLine("Pearsons Coefficient for Health Care Sector is: " + pearsonsCoeff.ToString());

            pearsonsCoeff = pearsonsCorrelationCoeff(informationTechData);
            Console.WriteLine("Pearsons Coefficient for Information Technology Sector is: " + pearsonsCoeff.ToString());

            pearsonsCoeff = pearsonsCorrelationCoeff(commServicesData);
            Console.WriteLine("Pearsons Coefficient for Communication Services Sector is: " + pearsonsCoeff.ToString());

            pearsonsCoeff = pearsonsCorrelationCoeff(consumerDiscData);
            Console.WriteLine("Pearsons Coefficient for Consumer Discretionary Sector is: " + pearsonsCoeff.ToString());

            pearsonsCoeff = pearsonsCorrelationCoeff(utilitiesData);
            Console.WriteLine("Pearsons Coefficient for Utilities Sector is: " + pearsonsCoeff.ToString());

            pearsonsCoeff = pearsonsCorrelationCoeff(financialsData);
            Console.WriteLine("Pearsons Coefficient for Financial Sector is: " + pearsonsCoeff.ToString());

            pearsonsCoeff = pearsonsCorrelationCoeff(materialsData);
            Console.WriteLine("Pearsons Coefficient for Materials Sector is: " + pearsonsCoeff.ToString());

            pearsonsCoeff = pearsonsCorrelationCoeff(realEstateData);
            Console.WriteLine("Pearsons Coefficient for Real Estate Sector is: " + pearsonsCoeff.ToString());

            pearsonsCoeff = pearsonsCorrelationCoeff(consumerStaplesData);
            Console.WriteLine("Pearsons Coefficient for Consumer Staples Sector is: " + pearsonsCoeff.ToString());

            pearsonsCoeff = pearsonsCorrelationCoeff(energyData);
            Console.WriteLine("Pearsons Coefficient for Energy Sector is: " + pearsonsCoeff.ToString());

            Console.WriteLine();

            // apply the top-k query to the datasets...

            Console.WriteLine("Top-k query where k = 10; Pearsons Correlation Coefficient:");

            List<(double, double)> industrialTopK = findTopK(industrialData, 10);
            pearsonsCoeff = pearsonsCorrelationCoeff(industrialTopK);
            Console.WriteLine("Pearsons Coefficient for Industral Sector is: " + pearsonsCoeff.ToString());

            List<(double, double)> healthCareTopK = findTopK(healthCareData, 10);
            pearsonsCoeff = pearsonsCorrelationCoeff(healthCareTopK);
            Console.WriteLine("Pearsons Coefficient for Health Care Sector is: " + pearsonsCoeff.ToString());

            List<(double, double)> informationTechTopK = findTopK(informationTechData, 10);
            pearsonsCoeff = pearsonsCorrelationCoeff(informationTechTopK);
            Console.WriteLine("Pearsons Coefficient for Information Technology Sector is: " + pearsonsCoeff.ToString());

            List<(double, double)> commServicesTopK = findTopK(commServicesData, 10);
            pearsonsCoeff = pearsonsCorrelationCoeff(commServicesTopK);
            Console.WriteLine("Pearsons Coefficient for Communication Services Sector is: " + pearsonsCoeff.ToString());

            List<(double, double)> consumerDiscTopK = findTopK(consumerDiscData, 10);
            pearsonsCoeff = pearsonsCorrelationCoeff(consumerDiscTopK);
            Console.WriteLine("Pearsons Coefficient for Consumer Discretionary Sector is: " + pearsonsCoeff.ToString());

            List<(double, double)> utilitiesTopK = findTopK(utilitiesData, 10);
            pearsonsCoeff = pearsonsCorrelationCoeff(utilitiesTopK);
            Console.WriteLine("Pearsons Coefficient for Utilities Sector is: " + pearsonsCoeff.ToString());

            List<(double, double)> financialTopK = findTopK(financialsData, 10);
            pearsonsCoeff = pearsonsCorrelationCoeff(financialTopK);
            Console.WriteLine("Pearsons Coefficient for Financial Sector is: " + pearsonsCoeff.ToString());

            List<(double, double)> materialsTopK= findTopK(materialsData, 10);
            pearsonsCoeff = pearsonsCorrelationCoeff(materialsTopK);
            Console.WriteLine("Pearsons Coefficient for Materials Sector is: " + pearsonsCoeff.ToString());

            List<(double, double)> realEstateTopK = findTopK(realEstateData, 10);
            pearsonsCoeff = pearsonsCorrelationCoeff(realEstateTopK);
            Console.WriteLine("Pearsons Coefficient for Real Estate Sector is: " + pearsonsCoeff.ToString());

            List<(double, double)> consumerStaplesTopK = findTopK(consumerStaplesData, 10);
            pearsonsCoeff = pearsonsCorrelationCoeff(consumerStaplesTopK);
            Console.WriteLine("Pearsons Coefficient for Consumer Staples Sector is: " + pearsonsCoeff.ToString());

            List<(double, double)> energyTopK = findTopK(energyData, 10);
            pearsonsCoeff = pearsonsCorrelationCoeff(energyTopK);
            Console.WriteLine("Pearsons Coefficient for Energy Sector is: " + pearsonsCoeff.ToString());


            // produce chart for the k query results...

            Chart kQueryChart = new Chart();
            ChartArea kArea = kQueryChart.ChartAreas.Add("K1");

            Series kIndustrials = new Series("kIndustrials");
            for (int i = 0; i < 10; i++)
            {
                kIndustrials.Points.AddXY(industrialTopK[i].Item1, industrialTopK[i].Item2);
            }
            kIndustrials.ChartType = SeriesChartType.Point;
            kIndustrials.MarkerStyle = MarkerStyle.Circle;
            kIndustrials.MarkerColor = System.Drawing.Color.Green;

            Series kHealthCare = new Series("kHealthCare");
            for (int i = 0; i < 10; i++)
            {
                kHealthCare.Points.AddXY(healthCareTopK[i].Item1, healthCareTopK[i].Item2);
            }
            kHealthCare.ChartType = SeriesChartType.Point;
            kHealthCare.MarkerStyle = MarkerStyle.Circle;
            kHealthCare.MarkerColor = System.Drawing.Color.Blue;

            Series kInformationTech = new Series("kInformationTech");
            for (int i = 0; i < 10; i++)
            {
                kInformationTech.Points.AddXY(informationTechTopK[i].Item1, informationTechTopK[i].Item2);
            }
            kInformationTech.ChartType = SeriesChartType.Point;
            kInformationTech.MarkerStyle = MarkerStyle.Circle;
            kInformationTech.MarkerColor = System.Drawing.Color.Red;

            Series kCommServices = new Series("kCommServices");
            for (int i = 0; i < 10; i++)
            {
                kCommServices.Points.AddXY(commServicesTopK[i].Item1, commServicesTopK[i].Item2);
            }
            kCommServices.ChartType = SeriesChartType.Point;
            kCommServices.MarkerStyle = MarkerStyle.Circle;
            kCommServices.MarkerColor = System.Drawing.Color.Black;

            Series kConsumerDisc = new Series("kConsumerDisc");
            for (int i = 0; i < 10; i++)
            {
                kConsumerDisc.Points.AddXY(consumerDiscTopK[i].Item1, consumerDiscTopK[i].Item2);
            }
            kConsumerDisc.ChartType = SeriesChartType.Point;
            kConsumerDisc.MarkerStyle = MarkerStyle.Circle;
            kConsumerDisc.MarkerColor = System.Drawing.Color.Yellow;

            Series kUtilities = new Series("kUtilities");
            for (int i = 0; i < 10; i++)
            {
                kUtilities.Points.AddXY(utilitiesTopK[i].Item1, utilitiesTopK[i].Item2);
            }
            kUtilities.ChartType = SeriesChartType.Point;
            kUtilities.MarkerStyle = MarkerStyle.Circle;
            kUtilities.MarkerColor = System.Drawing.Color.Orange;

            Series kFinancials = new Series("kFinancials");
            for (int i = 0; i < 10; i++)
            {
                kFinancials.Points.AddXY(financialTopK[i].Item1, financialTopK[i].Item2);
            }
            kFinancials.ChartType = SeriesChartType.Point;
            kFinancials.MarkerStyle = MarkerStyle.Circle;
            kFinancials.MarkerColor = System.Drawing.Color.Purple;

            Series kMaterials = new Series("kMaterials");
            for (int i = 0; i < 10; i++)
            {
                kMaterials.Points.AddXY(materialsTopK[i].Item1, materialsTopK[i].Item2);
            }
            kMaterials.ChartType = SeriesChartType.Point;
            kMaterials.MarkerStyle = MarkerStyle.Circle;
            kMaterials.MarkerColor = System.Drawing.Color.HotPink;

            Series kRealEstate = new Series("kRealEstate");
            for (int i = 0; i < 10; i++)
            {
                kRealEstate.Points.AddXY(realEstateTopK[i].Item1, realEstateTopK[i].Item2);
            }
            kRealEstate.ChartType = SeriesChartType.Point;
            kRealEstate.MarkerStyle = MarkerStyle.Circle;
            kRealEstate.MarkerColor = System.Drawing.Color.LightGreen;

            Series kConsumerStaples = new Series("kConsumerStaples");
            for (int i = 0; i < 10; i++)
            {
                kConsumerStaples.Points.AddXY(consumerStaplesTopK[i].Item1, consumerStaplesTopK[i].Item2);
            }
            kConsumerStaples.ChartType = SeriesChartType.Point;
            kConsumerStaples.MarkerStyle = MarkerStyle.Circle;
            kConsumerStaples.MarkerColor = System.Drawing.Color.LightBlue;

            Series kEnergy = new Series("kEnergy");
            for (int i = 0; i < 10; i++)
            {
                kEnergy.Points.AddXY(energyTopK[i].Item1, energyTopK[i].Item2);
            }
            kEnergy.ChartType = SeriesChartType.Point;
            kEnergy.MarkerStyle = MarkerStyle.Circle;
            kEnergy.MarkerColor = System.Drawing.Color.Gray;

            kQueryChart.Series.Clear();
            kQueryChart.Series.Add(kIndustrials);
            kQueryChart.Series.Add(kHealthCare);
            kQueryChart.Series.Add(kInformationTech);
            kQueryChart.Series.Add(kCommServices);
            kQueryChart.Series.Add(kConsumerDisc);
            kQueryChart.Series.Add(kUtilities);
            kQueryChart.Series.Add(kFinancials);
            kQueryChart.Series.Add(kMaterials);
            kQueryChart.Series.Add(kRealEstate);
            kQueryChart.Series.Add(kConsumerStaples);
            kQueryChart.Series.Add(kEnergy);

            kQueryChart.ResetAutoValues();
            kQueryChart.Titles.Clear();
            kQueryChart.Titles.Add($"Earnings Surprise VS. Price Changes (k = 10)");
            kQueryChart.ChartAreas[0].AxisX.Title = "Earnings Surprise (%)";
            kQueryChart.ChartAreas[0].AxisY.Title = "Price Change (%)";
            kQueryChart.ChartAreas[0].AxisX.MajorGrid.LineColor = System.Drawing.Color.LightGray;
            kQueryChart.AntiAliasing = AntiAliasingStyles.Graphics;
            kQueryChart.TextAntiAliasingQuality = TextAntiAliasingQuality.High;
            kQueryChart.Size = new System.Drawing.Size(1000, 800);
            kQueryChart.Titles.ElementAt(0).Font = new System.Drawing.Font("Arial", 25, System.Drawing.FontStyle.Bold);
            kArea.AxisX.TitleAlignment = System.Drawing.StringAlignment.Center;
            kArea.AxisY.TitleAlignment = System.Drawing.StringAlignment.Center;
            kArea.AxisX.TitleFont = new System.Drawing.Font("Arial", 15, System.Drawing.FontStyle.Bold);
            kArea.AxisY.TitleFont = new System.Drawing.Font("Arial", 15, System.Drawing.FontStyle.Bold);
            kQueryChart.BackColor = System.Drawing.Color.Transparent;
            kArea.BackColor = System.Drawing.Color.Transparent;

            kQueryChart.SaveImage(@"C:\Users\vdmil\OneDrive\Documents\KSU_SPRING_2021\Big Data Analytics\project_code\image\top_k_10.png", ChartImageFormat.Png);
            Console.WriteLine("Top-k = 10 Chart Produced :)");

        }

        public List<(double, double)> findTopK(List<(double, double)>workingData, int k)
        {
            double[] pearsonsCoeff = new double[k];
            //List<double> pearsonsCoeff = new List<double>();
            List<(double, double)> localWorkingSet = workingData;
            List<(double, double)> topKdata = new List<(double, double)>(); // subset of data
            for (int i = 0; i < k; i++)
            {
                topKdata.Add(localWorkingSet[i]); // load subset with first 10 items in whole data set
            }

            for (int j = k; j < localWorkingSet.Count; j++)
            {
                for (int i = 0; i < k; i++)
                {
                    (double, double) temp = topKdata[i];
                    topKdata.Remove(topKdata[i]);
                    pearsonsCoeff[i] = pearsonsCorrelationCoeff(topKdata);
                    //pearsonsCoeff.Add(pearsonsCorrelationCoeff(topKdata));
                    topKdata.Insert(i, temp);
                }

                // use this to find the strongest negative correlation...
                //double min = 2;
                //int indexOfMin = 0;
                //for (int i = 0; i < k; i++)
                //{
                //    if (pearsonsCoeff[i] < min)
                //    {
                //        min = pearsonsCoeff[i];
                //        indexOfMin = i;
                //    }
                //}
                //topKdata.RemoveAt(indexOfMin);
                //topKdata.Insert(indexOfMin, localWorkingSet[j]);

                // find the strongest positive correlation...
                double max = -2;
                int indexOfMax = 0;
                for (int i = 0; i < k; i++)
                {
                    if (pearsonsCoeff[i] > max)
                    {
                        max = pearsonsCoeff[i];
                        indexOfMax = i;
                    }
                }
                topKdata.RemoveAt(indexOfMax);
                topKdata.Insert(indexOfMax, localWorkingSet[j]);
            }           

            return topKdata;
        }

        public double pearsonsCorrelationCoeff(List<(double, double)>workingData)
        {
            double sumSurprise = 0;
            double avgSurprise = 0;
            for (int i = 0; i < workingData.Count; i++)
            {
                sumSurprise += workingData[i].Item1;
            }
            avgSurprise = sumSurprise / workingData.Count;

            double sumPriceChange = 0;
            double avgPriceChange = 0;
            for (int i = 0; i < workingData.Count; i++)
            {
                sumPriceChange += workingData[i].Item2;
            }
            avgPriceChange = sumPriceChange / workingData.Count;

            double numerator = 0;
            double sumDenomSurprise = 0;
            double sumDenomPrice = 0;
            double denominator = 0;
            double pearsonsCoeff = 0;
            for (int i = 0; i < workingData.Count; i++)
            {
                numerator += (workingData[i].Item1 - avgSurprise) * (workingData[i].Item2 - avgPriceChange);
            }
            for (int i = 0; i < workingData.Count; i++)
            {
                sumDenomSurprise += Math.Pow((workingData[i].Item1 - avgSurprise), 2);
                sumDenomPrice += Math.Pow((workingData[i].Item2 - avgPriceChange), 2);
            }
            denominator = Math.Sqrt((sumDenomSurprise * sumDenomPrice));
            pearsonsCoeff = numerator / denominator;
            return pearsonsCoeff;
        }
    }
}
