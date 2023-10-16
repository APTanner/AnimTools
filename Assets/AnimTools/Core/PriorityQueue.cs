using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace AnimTools
{ 
    public class PriorityQueue<T> where T : IComparable<T>
    {
        private readonly List<T> _data = new List<T>();

        public int Count { get { return _data.Count; } }

        public void Enqueue(T element)
        {
            _data.Add(element);
            maxHeapifyUp();
        }

        public T Dequeue()
        {
            T elem = _data[0];
            moveLastItem();
            maxHeapifyDown();
            return elem;
        }

        private void maxHeapifyUp()
        {
            int index = _data.Count - 1;
            while (index > 0)
            {
                int parentIndex = getParent(index);
                if (_data[index].CompareTo(_data[parentIndex]) <= 0)
                {
                    break;
                }
                swap(index, parentIndex);
                index = parentIndex;
            }
        }

        private void maxHeapifyDown()
        {
            bool notFoundPlace = true;
            int index = 0;
            int l, r, largest;
            while (notFoundPlace)
            {
                l = getLeft(index);
                r = getRight(index);
                largest = index;
                if (l < _data.Count && _data[l].CompareTo(_data[largest]) >= 0)
                {
                    largest = l;
                }
                if (r < _data.Count && _data[r].CompareTo(_data[largest]) >= 0)
                {
                    largest = r;
                }
                if (largest != index)
                {
                    swap(index, largest);
                    index = largest;
                }
                else
                {
                    notFoundPlace = false;
                }
            }
        }

        private int getParent(int index)
        {
            return (index-1) / 2;
        }

        private int getRight(int index)
        {
            return index * 2 + 2;
        }

        private int getLeft(int index)
        {
            return index * 2 + 1;
        }

        private void moveLastItem()
        {
            _data[0] = _data[_data.Count - 1];
            _data.RemoveAt(_data.Count - 1);
        }

        private void swap(int a, int b)
        {
            T temp = _data[a];
            _data[a] = _data[b];
            _data[b] = temp;
        }
    }
}