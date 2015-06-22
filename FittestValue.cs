using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenService
{
    /// <summary>
    /// Class to store fittest value
    /// </summary>
    public class FittestValue
    {
        /// <summary>
        /// X coordinate
        /// </summary>
        private double x;
        public double X
        {
            get { return x; }
            set { x = value; }
        }
        /// <summary>
        /// Y coordinate
        /// </summary>
        private double y;
        public double Y
        {
            get { return y; }
            set { y = value; }
        }
        /// <summary>
        /// Function value of X and Y value
        /// </summary>
        private double fit;
        public double Fit
        {
            get { return fit; }
            set { fit = value; }
        }
        /// <summary>
        /// empty contructor
        /// </summary>
        public FittestValue() { }
        /// <summary>
        /// Assign X,Y,FitValue in constructor
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="fit"></param>
        public FittestValue(double x, double y, double fit)
        {
            this.X = x;
            this.Y = y;
            this.Fit = fit;
        }
    }
}
