using Common;
using System;
using System.Collections.Generic;

namespace TrafficLightService
{

    public interface Iterator<T>
    {
        public abstract T First();
        public abstract T Last();
        public abstract IEnumerable<(T, int)> Next();
        public abstract T CurrentItem();
    }

    public class LightSequence : Iterator<Signal>
    {
        private Signal _currentItem = null;
        private List<Signal> _items = new List<Signal>();
        private int _current = 0;
        private bool _isForward = true;

        public LightSequence(List<Signal> item, int currentIndex)
        {
            _items = item;
            _current = currentIndex;

            _currentItem = _items[_current];
        }

        public Signal First()
        {
            return _items[0];
        }

        public Signal Last()
        {
            return _items[_items.Count - 1];
        }

        public IEnumerable<(Signal, int)> Next()
        {
            // execute rules for current signal  
            foreach (var result in CurrentItem().PrePerformRules())
            {
                var (item, usedTime) = result;
                if (item.GetStatus() == _items[_current].GetStatus())
                {
                    if (_isForward && _current < _items.Count - 1)
                        ++_current;

                    else
                        _current = 0;


                    _currentItem = _items[_current];
                }
                else _currentItem = item;

                yield return (item, usedTime);
            }
        }

        public Signal CurrentItem()
        {
            return _currentItem;
        }

    }

}
