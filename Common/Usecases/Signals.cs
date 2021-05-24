
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Common
{
    public class Signals : List<Signal> { }

    public abstract class Signal
    {
        [JsonProperty]
        protected Signal _parent { get; set; }

        [JsonProperty]
        protected Signal _child { get; set; }

        [JsonProperty]
        protected Rules _rules { get; set; }

        public string getParentStatus() => _parent != null ? _parent.GetStatus() : "";
        public Signal GetChild() => _child;


        public abstract string GetStatus(); 

        public IEnumerable<(Signal, int)> PrePerformRules()
        {  
            if (_child != null)
            {  
                yield return (_child, _child._rules.GetStayTime());
            }
             
            yield return (this, _rules.GetStayTime());
        }

        public void Perform(int usedTime=0)
        {
            _rules.Perform(usedTime);
        }

        public void setParent(Signal parent)
        {
            _parent = parent;
        } 

        public void AppendRules(IRule rule)
        {
            _rules.Add(rule);
        } 
    }


    public class YellowLight : Signal
    {
        public YellowLight() { }
        public YellowLight(Rules rules)
        {
            _rules = rules;
        }

        public YellowLight(Rules rules, Signal child)
        {
            _rules = rules;
            _child = child;
        }

        public override string GetStatus()
        {
            return this.GetType().Name;
        }
    }

    public class RedLight : Signal
    {
        public RedLight() { }
        public RedLight(Rules rules)
        {
            _rules = rules;
        }

        public RedLight(Rules rules, Signal child)
        {
            _rules = rules;
            _child = child;
        }
        public override string GetStatus()
        {
            return this.GetType().Name;
        }
    }

    public class GreenLight : Signal
    {
        public GreenLight() { }
        public GreenLight(Rules rules)
        {
            _rules = rules;
        }

        public GreenLight(Rules rules, Signal child)
        {
            _rules = rules;
            _child = child; 
        }

        public override string GetStatus()
        {
            return this.GetType().Name;
        }

    }

    public class GreenRightArrowLight : Signal
    {
        public GreenRightArrowLight() { }
        public GreenRightArrowLight(Rules rules)
        {
            _rules = rules;
        }

        public override string GetStatus()
        {
            return this.GetType().Name;
        }
    }


}
