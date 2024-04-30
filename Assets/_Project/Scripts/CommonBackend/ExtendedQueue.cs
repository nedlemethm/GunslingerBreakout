using System.Collections.Generic;
using UnityEngine;

public class ExtendedQueue<T>
{
    private Queue<T> _queue = new Queue<T>();

    public int Count { get { return _queue.Count; } }

    // Method to enqueue an item
    public void Enqueue(T item)
    {
        _queue.Enqueue(item);
    }

    // Method to dequeue an item
    public T Dequeue()
    {
        return _queue.Dequeue();
    }

    // Method to peek at the first item in the queue
    public T Peek()
    {
        return _queue.Peek();
    }

    // Method to clear the queue
    public void Clear()
    {
        _queue.Clear();
    }

    // Method to check if the queue contains an item
    public bool Contains(T item)
    {
        return _queue.Contains(item);
    }

    // Method to swap two elements at given indices
    public void Swap(int index1, int index2)
    {
        if (index1 < 0 || index1 >= _queue.Count || index2 < 0 || index2 >= _queue.Count)
        {
            Debug.LogError("Invalid indices for swapping.");
            return;
        }

        T[] array = _queue.ToArray();
        T temp = array[index1];
        array[index1] = array[index2];
        array[index2] = temp;

        _queue.Clear();
        foreach (T item in array)
        {
            _queue.Enqueue(item);
        }
    }

    // Method to move an element to a new position
    public void MoveElement(int fromIndex, int toIndex)
    {
        if (fromIndex < 0 || fromIndex >= _queue.Count || toIndex < 0 || toIndex >= _queue.Count)
        {
            Debug.LogError("Invalid indices for moving element.");
            return;
        }

        T[] array = _queue.ToArray();
        T item = array[fromIndex];

        if (fromIndex < toIndex)
        {
            for (int i = fromIndex; i < toIndex; i++)
            {
                array[i] = array[i + 1];
            }
        }
        else
        {
            for (int i = fromIndex; i > toIndex; i--)
            {
                array[i] = array[i - 1];
            }
        }

        array[toIndex] = item;

        _queue.Clear();
        foreach (T newItem in array)
        {
            _queue.Enqueue(newItem);
        }
    }

    // Method to print the contents of the queue
    public void PrintQueue()
    {
        foreach (T item in _queue)
        {
            Debug.Log(item);
        }
    }
}
