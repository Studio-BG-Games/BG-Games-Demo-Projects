/* The point class allows developers to create a point in a 3 dimensional space.
 * The point class is complete with many concrete methods and operator methods.
 * 
 * Author: Corey St-Jacques
 */

using System;

namespace Gameplay.Map.ChunkMaster.Scripts.ChunkMaster.ChunkMaster.src.Utils
{
    /// <summary>
    /// The point class allows the world class to easy manipulate points in a 3 dimensional space.
    /// </summary>
    public class Point
    {
        #region Data Properties
        /// <summary>
        /// Retrieves a point object with coordinates (0, 0, 0).
        /// </summary>
        private static Point zeroPoint = new Point(0.0, 0.0, 0.0);

        /// <summary>
        /// This property returns a point with zero values (0, 0, 0).
        /// </summary>
        public static Point zero
        {
            get
            {
                return zeroPoint;
            }
        }

        /// <summary>
        /// Retrieves a point object with coordinates (1, 1, 1).
        /// </summary>
        private static Point onePoint = new Point(1.0, 1.0, 1.0);

        /// <summary>
        /// This property returns a point with 1 values (1, 1, 1).
        /// </summary>
        public static Point one
        {
            get
            {
                return onePoint;
            }
        }

        /// <summary>
        /// The X coordinate.
        /// </summary>
        public double x { get; set; }

        /// <summary>
        /// The Y coordinate.
        /// </summary>
        public double y { get; set; }

        /// <summary>
        /// The Z coordinate.
        /// </summary>
        public double z { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor with parameters using doubles.
        /// </summary>
        /// <param name="x">The x value.</param>
        /// <param name="y">The y value.</param>
        /// <param name="z">The z value.</param>
        public Point(double x, double y, double z = 0.0)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        #endregion

        #region Concrete Methods

        /// <summary>
        /// This method converts an index value into a 3D point in space.
        /// </summary>
        /// <param name="index">The index you would like to convert.</param>
        /// <param name="size">The size of your grid.</param>
        /// <returns>Point</returns>
        public static Point GetPointByIndex(int index, double size)
        {
            double z = (index / (size * size));
            double y = Math.Floor((z - Math.Floor(z)) * size);
            z = Math.Floor(z);
            return new Point(index % size, y, z);
        }

        /// <summary>
        /// This method converts a point in 3D space to a single value.
        /// </summary>
        /// <param name="point">The 3D point you would like to index.</param>
        /// <param name="size">The size of your grid.</param>
        /// <returns>int</returns>
        public static int GetIndexByPoint(Point point, int size)
        {
            int index;
            index = (int)point.x + (int)point.y * size + (int)point.z * (size * size);
            return index;
        }

        /// <summary>
        /// Checks if this point is equal to another point.
        /// </summary>
        /// <param name="obj">The point object you would like to compare with.</param>
        /// <returns>bool</returns>
        public override bool Equals(object obj)
        {
            return (obj is Point) ? this == (Point)obj : false;
        }

        /// <summary>
        /// Retrieves the base object's hash code as an integer.
        /// </summary>
        /// <returns>int</returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <summary>
        /// This method snaps the current point to a grid given the grid scale value.
        /// </summary>
        /// <param name="value">This is the grid size for snapping.</param>
        /// <returns>Point</returns>
        public Point Snap(int value = 1)
        {
            return SnapToGrid.snap(this, value);
        }

        /// <summary>
        /// Dsiplays this point's details.
        /// </summary>
        /// <returns>string</returns>
        public override string ToString()
        {
            return string.Format("({0}, {1}, {2})", new object[]
            {
                this.x.ToString("0.0###########"),
                this.y.ToString("0.0###########"),
                this.z.ToString("0.0###########")
            });
        }

        /// <summary>
        /// Dsiplays this point's details.
        /// </summary>
        /// <param name="format">The string's format.</param>
        /// <returns>string</returns>
        public string ToString(string format)
        {
            return string.Format("({0}, {1}, {2})", new object[]
            {
                this.x.ToString(format),
                this.y.ToString(format),
                this.z.ToString(format)
            });
        }

        #endregion

        #region Operators
        /// <summary>
        /// Checks if both objects are equal.
        /// </summary>
        /// <param name="value1">Requires the first object.</param>
        /// <param name="value2">Requires the second object.</param>
        /// <returns>bool</returns>
        public static bool operator ==(Point value1, Point value2)
        {
            return value1.x == value2.x
                && value1.y == value2.y
                && value1.z == value2.z;
        }

        /// <summary>
        /// Checks if objects are not equal.
        /// </summary>
        /// <param name="value1">Requires the first object.</param>
        /// <param name="value2">Requires the second object.</param>
        /// <returns>bool</returns>
        public static bool operator !=(Point value1, Point value2)
        {
            return !(value1 == value2);
        }

        /// <summary>
        /// Adds both objects together.
        /// </summary>
        /// <param name="value1">Requires the first object.</param>
        /// <param name="value2">Requires the second object.</param>
        /// <returns>Point</returns>
        public static Point operator +(Point value1, Point value2)
        {
            Point tmp = new Point(value1.x + value2.x, value1.y + value2.y, value1.z + value2.z);
            return tmp;
        }

        /// <summary>
        /// Subtracts both objects together.
        /// </summary>
        /// <param name="value">Requires the first object to subtract.</param>
        /// <returns>Point</returns>
        public static Point operator -(Point value)
        {
            Point value2 = new Point(-value.x, -value.y, -value.z);
            return value2;
        }

        /// <summary>
        /// Subtracts both objects together.
        /// </summary>
        /// <param name="value1">Requires the first object.</param>
        /// <param name="value2">Requires the second object.</param>
        /// <returns>Point</returns>
        public static Point operator -(Point value1, Point value2)
        {
            Point tmp = new Point(value1.x - value2.x, value1.y - value2.y, value1.z - value2.z);
            return tmp;
        }

        /// <summary>
        /// Multiplies both objects together.
        /// </summary>
        /// <param name="value1">Requires the first object.</param>
        /// <param name="value2">Requires the second object.</param>
        /// <returns>Point</returns>
        public static Point operator *(Point value1, Point value2)
        {
            Point tmp = new Point(value1.x * value2.x, value1.y * value2.y, value1.z * value2.z);
            return tmp;
        }

        /// <summary>
        /// Multiplies both objects together.
        /// </summary>
        /// <param name="value">Requires the first object to subtract.</param>
        /// <param name="scaleFactor">Requires a multiplier factor.</param>
        /// <returns>Point</returns>
        public static Point operator *(Point value, double scaleFactor)
        {
            Point tmp = new Point(value.x * scaleFactor, value.y * scaleFactor, value.z * scaleFactor);
            return tmp;
        }

        /// <summary>
        /// Multiplies both objects together.
        /// </summary>
        /// <param name="scaleFactor">Requires a multiplier factor.</param>
        /// <param name="value">Requires the first object to subtract.</param>
        /// <returns>Point</returns>
        public static Point operator *(double scaleFactor, Point value)
        {
            Point tmp = new Point(value.x * scaleFactor, value.y * scaleFactor, value.z * scaleFactor);
            return tmp;
        }

        /// <summary>
        /// Divides both objects together.
        /// </summary>
        /// <param name="value1">Requires the first object.</param>
        /// <param name="value2">Requires the second object.</param>
        /// <returns>Point</returns>
        public static Point operator /(Point value1, Point value2)
        {
            Point tmp = new Point(value1.x / value2.x, value1.y / value2.y, value1.z / value2.z);
            return tmp;
        }

        /// <summary>
        /// Divides both objects together.
        /// </summary>
        /// <param name="value">Requires the first object.</param>
        /// <param name="scaleFactor">Requires a scale factor.</param>
        /// <returns>Point</returns>
        public static Point operator /(Point value, double scaleFactor)
        {
            Point tmp = new Point(value.x / scaleFactor, value.y / scaleFactor, value.z / scaleFactor);
            return tmp;
        }

        /// <summary>
        /// Divides both objects together.
        /// </summary>
        /// <param name="scaleFactor">Requires a scale factor.</param>
        /// <param name="value">Requires the first object.</param>
        /// <returns>Point</returns>
        public static Point operator /(double scaleFactor, Point value)
        {
            Point tmp = new Point(value.x / scaleFactor, value.y / scaleFactor, value.z / scaleFactor);
            return tmp;
        }

        #endregion
    }
}
