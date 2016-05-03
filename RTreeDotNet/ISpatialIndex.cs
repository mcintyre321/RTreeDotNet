//   SpatialIndex.java
//   Java Spatial Index Library
//   Copyright (C) 2002-2005 Infomatiq Limited.
//
//  This library is free software; you can redistribute it and/or
//  modify it under the terms of the GNU Lesser General 
//  License as published by the Free Software Foundation; either
//  version 2.1 of the License, or (at your option) any later version.
//
//  This library is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
//  Lesser General  License for more details.
//
//  You should have received a copy of the GNU Lesser General 
//  License along with this library; if not, write to the Free Software
//  Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA

using System;

namespace RTreeDotNet
{

/**
 * Defines methods that must be implemented by all
 * spatial indexes. This includes the RTree and its variants.
 */

    public interface ISpatialIndex
    {

        /**
   * Initializes any implementation dependent properties
   * of the spatial index. For example, RTree implementations
   * will have a NodeSize property.
   *
   * @param props The set of properties used to initialize the spatial index.
   */
        // void init(Properties props);

        /**
   * Adds a new rectangle to the spatial index
   *
   * @param r  The rectangle to add to the spatial index.
   * @param id The ID of the rectangle to add to the spatial index.
   *           The result of adding more than one rectangle with
   *           the same ID is undefined.
   */
        void add(Rectangle r, int id);

        /**
   * Deletes a rectangle from the spatial index
   *
   * @param r  The rectangle to delete from the spatial index
   * @param id The ID of the rectangle to delete from the spatial
   *           index
   *
   * @return true  if the rectangle was deleted
   *         false if the rectangle was not found, or the
   *               rectangle was found but with a different ID
   */
        bool Delete(Rectangle r, int id);

        /**
   * Finds the nearest rectangles to the passed rectangle and calls
   * v.execute(id) for each one.
   *
   * If multiple rectangles are equally near, they will
   * all be returned.
   *
   * @param p The point for which this method finds the
   * nearest neighbours.
   *
   * @param v The IntProcedure whose execute() method is is called
   * for each nearest neighbour.
   *
   * @param furthestDistance The furthest distance away from the rectangle
   * to search. Rectangles further than this will not be found.
   *
   * This should be as small as possible to minimise
   * the search time.
   *
   * Use float.PositiveInfinity to guarantee that the nearest rectangle is found,
   * no matter how far away, although this will slow down the algorithm.
   */
        void Nearest(Point p, Predicate<int> v, float furthestDistance);

        /**
   * Finds the N nearest rectangles to the passed rectangle, and calls
   * execute(id, distance) on each one, in order of increasing distance.
   *
   * Note that fewer than N rectangles may be found if fewer entries
   * exist within the specified furthest distance, or more if rectangles
   * N and N+1 have equal distances.
   *
   * @param p The point for which this method finds the
   * nearest neighbours.
   *
   * @param v The IntfloatProcedure whose execute() method is is called
   * for each nearest neighbour.
   *
   * @param n The desired number of rectangles to find (but note that
   * fewer or more may be returned)
   *
   * @param distance The furthest distance away from the rectangle
   * to search. Rectangles further than this will not be found.
   *
   * This should be as small as possible to minimise
   * the search time.
   *
   * Use float.PositiveInfinity to guarantee that the nearest rectangle is found,
   * no matter how far away, although this will slow down the algorithm.
   */
        void NearestN(Point p, Predicate<int> v, int n, float distance);

        /**
   * Same as nearestN, except the found rectangles are not returned
   * in sorted order. This will be faster, if sorting is not required
   */
        void NearestNUnsorted(Point p, Predicate<int> v, int n, float distance);

        /**
   * Finds all rectangles that intersect the passed rectangle.
   *
   * @param  r The rectangle for which this method finds
   *           intersecting rectangles.
   *
   * @param ip The IntProcedure whose execute() method is is called
   *           for each intersecting rectangle.
   */
        void Intersects(Rectangle r, Predicate<int> ip);

        /**
   * Finds all rectangles contained by the passed rectangle.
   *
   * @param r The rectangle for which this method finds
   *           contained rectangles.
   *
   * @param ip The procedure whose visit() method is is called
   *           for each contained rectangle.
   */
        void Contains(Rectangle r, Predicate<int> ip);

        /**
   * Returns the number of entries in the spatial index
   */
        int Size { get; }


        /**
   * Returns the bounds of all the entries in the spatial index,
   * or null if there are no entries.
   */
        Rectangle GetBounds();


        void init();
    }
}