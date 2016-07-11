using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace Library.Classes
{
    class Dictionary<T>
    {
        private List<DictionaryEntry<T>> _entries;

        public Dictionary()
        {
            _entries = new List<DictionaryEntry<T>>();
        }

        public void Register(string label, T defaultValue)
        {
            _entries.Add(new DictionaryEntry<T>(label, defaultValue));
        }

        public void Register(string label, T initValue, T defaultValue)
        {
            _entries.Add(new DictionaryEntry<T>(label, initValue, defaultValue));
        }

        public void Reset(string label)
        {
            _entries[FindEntry(label)].Reset();
        }

        public void ResetAll()
        {
            foreach (DictionaryEntry<T> entry in _entries)
            {
                entry.Reset();
            }
        }

        private int FindEntry(string label)
        {
            int blu = _entries.FindIndex(
            delegate(DictionaryEntry<T> entry)
            {
                return entry.Label.Equals(label);
            }
            );
            return blu;
        }

        public T this[string label]
        {
            get
            {
                return _entries[FindEntry(label)].Value;
            }
            set
            {
                _entries[FindEntry(label)].SetValue(value);
            }
        }

        public void Set(string label, T value)
        {
            _entries.ElementAt(FindEntry(label)).SetValue(value);
        }
    }

    class DictionaryEntry<T>
    {
        public string Label;
        public T Value;
        public T Default;

        public DictionaryEntry(string label, T defaultValue)
        {
            Label = label;
            Value = defaultValue;
            Default = defaultValue;
        }

        public DictionaryEntry(string label, T initValue, T defaultValue)
        {
            Label = label;
            Value = initValue;
            Default = defaultValue;
        }

        public void Reset()
        {
            Value = Default;
        }

        public void SetValue(T value)
        {
            Value = value;
        }
    }
}
