//   SortedList.java
//   Java Spatial Index Library
//   Copyright (C) 2002-2005 Infomatiq Limited.
//
//  This library is free software; you can redistribute it and/or
//  modify it under the terms of the GNU Lesser General Public
//  License as published by the Free Software Foundation; either
//  version 2.1 of the License, or (at your option) any later version.
//
//  This library is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
//  Lesser General Public License for more details.
//
//  You should have received a copy of the GNU Lesser General Public
//  License along with this library; if not, write to the Free Software
//  Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA

using System;
using System.Collections.Generic;

namespace RTreeDotNet.rtree
{

/**
 * <p>
 * Sorted List, backed by a TArrayList.
 *
 * The elements in the list are always ordered by priority.
 * Methods exists to remove elements with the highest and lowest priorities.
 *
 * If more than one element has the highest priority, they will
 * all be removed by calling removeHighest. Ditto for the lowest priority.
 *
 * The list has a preferred maximum size. If possible, entries with the lowest priority
 * will be removed to limit the maximum size. Note that entries with the lowest priority
 * will not be removed if this would leave fewer than the preferred maximum number
 * of entries.
 *
 * This class is not optimised for large values of preferredMaximumSize. Values greater than,
 * say, 5, are not recommended.
 * </p>
 */

    public class SortedList
    {

        private const int DEFAULT_PREFERRED_MAXIMUM_SIZE = 10;

        private int preferredMaximumSize = 1;
        private List<int> ids = null;
        private List<float> priorities = null;

        public void init(int preferredMaximumSize)
        {
            this.preferredMaximumSize = preferredMaximumSize;
            ids = new List<int>(preferredMaximumSize);
            priorities = new List<float>(preferredMaximumSize);
        }

        public void reset()
        {
            ids = new List<int>();
            priorities = new List<float>();
        }

        public SortedList()
        {
            ids = new List<int>(DEFAULT_PREFERRED_MAXIMUM_SIZE);
            priorities = new List<float>(DEFAULT_PREFERRED_MAXIMUM_SIZE);
        }

        public void add(int id, float priority)
        {
            float lowestPriority = float.NegativeInfinity;

            if (priorities.Count > 0)
            {
                lowestPriority = priorities[priorities.Count - 1];
                ;
            }

            if ((priority == lowestPriority) ||
                (priority < lowestPriority && ids.Count < preferredMaximumSize))
            {
                // simply add the new entry at the lowest priority end
                ids.Add(id);
                priorities.Add(priority);
            }
            else if (priority > lowestPriority)
            {
                if (ids.Count >= preferredMaximumSize)
                {
                    int lowestPriorityIndex = ids.Count - 1;
                    while ((lowestPriorityIndex - 1 >= 0) &&
                           (priorities[lowestPriorityIndex - 1] == lowestPriority) )
                    {
                        lowestPriorityIndex--;
                    }

                    if (lowestPriorityIndex >= preferredMaximumSize - 1)
                    {
                        ids.RemoveRange(lowestPriorityIndex, ids.Count - lowestPriorityIndex);
                        priorities.RemoveRange(lowestPriorityIndex, priorities.Count - lowestPriorityIndex);
                    }
                }

                // put the new entry in the correct position. Could do a binary search here if the
                // preferredMaximumSize was large.
                int insertPosition = ids.Count;
                while (insertPosition - 1 >= 0 && priority > priorities[insertPosition - 1] )
                {
                    insertPosition--;
                }

                ids.Insert(insertPosition, id);
                priorities.Insert(insertPosition, priority);
            }
        }

        /**
   * return the lowest priority currently stored, or float.NegativeInfinity if no
   * entries are stored
   */

        public float getLowestPriority()
        {
            float lowestPriority = float.NegativeInfinity;
            if (priorities.Count >= preferredMaximumSize)
            {
                lowestPriority = priorities[priorities.Count - 1]
                ;
            }
            return lowestPriority;
        }

        public void forEachId(Predicate<int> v)
        {
            for (int i = 0; i < ids.Count; i++)
            {
                if (!v(ids[i]))
                {
                    break;
                }
            }
        }

        public int[] toNativeArray()
        {
            return ids.ToArray();
        }
    }
}