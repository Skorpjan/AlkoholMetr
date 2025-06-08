using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Media3D;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using static Finalni_Projekt_Vzhled.Calculation;

namespace Finalni_Projekt_Vzhled
{
    public class Graf
    {
        public PlotModel GrafModel { get; set; }
        public Graf(double promileStart, double timeToZeroHours)
        {
            GrafModel = new PlotModel { Title = "Vývoj množství alkoholu v krvi" }; //<-- nápis ned grafem, možno měnit dle potřeby 
            

            GrafModel.Axes.Add(new LinearAxis
            {
                Position = AxisPosition.Bottom,
                Title = "Čas (hodiny)", //<-- text popisku
                FontSize = 18,     //velikost textu popisku
                Minimum = 0,             
                Maximum = timeToZeroHours * 1.1

            });

            GrafModel.Axes.Add(new LinearAxis
            {
                Position = AxisPosition.Left,
                Title = "Hladina alkoholu (‰)",
                FontSize = 18,
                Minimum = 0,
                Maximum = promileStart * 1.1

            });

            var lineSeries = new LineSeries
            {
                StrokeThickness = 4, //tloušťka čáry
                Color = OxyColors.Green //barva čáry
            };

            lineSeries.Points.Add(new DataPoint(0, promileStart)); //<-- body v grafu normálně [X,Y]
            lineSeries.Points.Add(new DataPoint(timeToZeroHours, 0));
            

            GrafModel.Series.Add(lineSeries);
        }
    }
}
